using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuestionnaireManager : MonoBehaviour
{
    public GameObject rowPrefab;  // Reference to the row prefab
    public Transform contentPanel;  // The parent panel to hold the rows
    public TextMeshProUGUI resultText;  // Reference to the Text component to display results
    public Button nextButton;  // Button to go to the next page
    public Button prevButton;  // Button to go to the previous page
    public Button submitButton;
    public TextMeshProUGUI pageNumberText;

    public GameObject alertDialogue;
    public TextMeshProUGUI alertMessage;
    public Button alertCloseButton;

    private List<QuestionnaireItem> questionaireItems = new List<QuestionnaireItem>();

    private string[] labels = { "Tense", "Angry", "Worn Out", "Unhappy", "Proud", "Lively", "Confused", "Sad", "Active", "On-edge",
    "Grouchy", "Ashamed", "Energetic", "Hopeless", "Uneasy", "Restless", "Unable to concentrate", "Fatigued", "Competent", "Annoyed", "Discouraged",
    "Resentful", "Nervous", "Miserable", "Discouraged", "Confident", "Bitter", "Exhausted", "Anxious", "Helpless", "Weary",
    "Satisfied", "Bewildered", "Furious", "Full of Pep", "Worthless", "Forgetful", "Vigorous" ,"Uncertain about things" ,"Bushed" ,"Embarrassed" };
    private int currentPage = 0;
    private int rowsPerPage = 6;

    void Start()
    {
        // Instantiate rows based on labels
        foreach (string label in labels)
        {
            GameObject newRow = Instantiate(rowPrefab, contentPanel);
            QuestionnaireItem currentRow = newRow.GetComponent<QuestionnaireItem>();

            currentRow.SetLabel(label);
            questionaireItems.Add(currentRow);
            newRow.SetActive(false);  // Hide all rows initially, because we want to show rows by page
        }
        prevButton.onClick.AddListener(PrevPage);
        nextButton.onClick.AddListener(NextPage);
        submitButton.onClick.AddListener(SubmitQuestionnaire);

        ShowAlertDialogue(false);
        alertCloseButton.onClick.AddListener(delegate { ShowAlertDialogue(false); });
        ShowPage(currentPage);
    }

    // Function to show a specific page of rows
    public void ShowPage(int page)
    {
        int startRow = page * rowsPerPage;
        int endRow = Mathf.Min(startRow + rowsPerPage, questionaireItems.Count);//index of row to show in the collection, 0-5. 6-11

        // Hide all rows
        foreach (var row in questionaireItems)
        {
            row.gameObject.SetActive(false);
        }

        // Show only the rows for the current page
        for (int i = startRow; i < endRow; i++)
        {
            questionaireItems[i].gameObject.SetActive(true);
        }

        // Update button states
        prevButton.interactable = page > 0; //cannot use when its first page
        nextButton.interactable = endRow < questionaireItems.Count; //cannot use when its last page
        submitButton.gameObject.SetActive(endRow >= questionaireItems.Count);  // Show submit button on the last page
        pageNumberText.text = "Page number: " + (currentPage + 1).ToString() + " out of " + GetMaxPageNumber().ToString();
    }
    // Function to calculate the maximum page number
    public int GetMaxPageNumber()
    {
        return Mathf.CeilToInt((float)questionaireItems.Count / rowsPerPage);
        //round up to integer, -1 is to adjust for zero-based indexing
    }

    public void NextPage()
    {
        if ((currentPage + 1) * rowsPerPage < questionaireItems.Count)
        {
            currentPage++;
            ShowPage(currentPage);
        }
    }

    public void PrevPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            ShowPage(currentPage);
        }
    }

    // when click submit, calculate and submit to server
    public void CalculateScores()
    {
        int tensionAnxietyScore = 0;
        int depressionDejectionScore = 0;
        int angerHostilityScore = 0;
        int vigorActivityScore = 0;
        int fatigueInertiaScore = 0;
        int confusionBewildermentScore = 0;

        for (int i = 0; i < questionaireItems.Count; i++)
        {
            int score = questionaireItems[i].GetSelectedValue();
            if (score == -1) continue;  // Skip if no toggle is selected

            if (i < 6)  // Assuming first 6 labels are for Tension-Anxiety
            {
                tensionAnxietyScore += score;
            }
            else if (i < 12)  // Assuming next 6 labels are for Depression-Dejection
            {
                depressionDejectionScore += score;
            }
            // Add more conditions for other mood states
        }

        // Calculate TMD (Total Mood Disturbance) Score
        int tmdScore = tensionAnxietyScore + depressionDejectionScore + angerHostilityScore
                       + fatigueInertiaScore + confusionBewildermentScore - vigorActivityScore;


    }

    // Function to check if any row is incomplete
    public void SubmitQuestionnaire()
    {
        bool isComplete = true;

        for (int page = 0; page * rowsPerPage < questionaireItems.Count; page++)
        {
            int startRow = page * rowsPerPage;
            int endRow = Mathf.Min(startRow + rowsPerPage, questionaireItems.Count);

            for (int i = startRow; i < endRow; i++)
            {
                if (!questionaireItems[i].IsAnyToggleOn())
                {
                    isComplete = false;
                    ShowAlertDialogue(true);
                    alertMessage.text = $"Incomplete item found on page {page + 1}, item: {questionaireItems[i].label.text}";

                }
            }
        }

        if (isComplete)
        {
            CalculateScores();
            //************SUBMIT TO BOFS
            //GET THE SCENE NUMBER FROM BOFS
            if (!LevelManager.instance.isMainGameReached) //whether player has played the game
                SceneManager.LoadScene("Title"); //start
            else SceneManager.LoadScene("End2"); 

        }
    }

    private void ShowAlertDialogue(bool show)
    {
        alertDialogue.SetActive(show);
    }
}

[System.Serializable]
public class POMSData
{
    public int gameLevel;

    public int tensionAnxietyScore;
    public int depressionDejectionScore;
    public int angerHostilityScore;
    public int fatigueInertiaScore;
    public int confusionBewildermentScore;
    public int totalMoodDisturbanceScore;
}

[System.Serializable] 
public class QuestionnaireData
{
    public string ageRange;
    public string gender;
    public string occupationStatus;
    public string gameFrequency;
    public string cosyExperience;
}