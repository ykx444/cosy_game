using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorRecognitionQuestion : Question
{
    private TextMeshProUGUI questionText;
    private TextMeshProUGUI colorWordText;

    private List<string> colorNames = new List<string> { "Red", "Green", "Blue", "Yellow", "Purple", "Orange", "Pink", "Brown" };
    private List<Color> colors = new List<Color> {new Color(0.7608f, 0.0118f, 0, 1), new Color(0.5059f, 0.7608f, 0, 1), new Color(0, 0.4706f, 0.7608f, 1),
                    new Color(0.9882f, 0.9608f, 0.1490f, 1), new Color(0.7922f, 0.1490f, 0.9882f, 1), new Color(0.9882f, 0.3373f, 0.1020f, 1),new Color(1,
                        0.5020f, 0.7255f, 1), new Color(0.5686f, 0.2706f, 0.0588f, 1) };
    private string correctColorName;
    private string displayedColorName;
    private int correctAnswerIndex;

    public ColorRecognitionQuestion(TextMeshProUGUI uiQuestionText, TextMeshProUGUI colorWordText, GameObject buttonContainer, GameObject answerButtonPrefab)
    {
        questionText = uiQuestionText;
        this.colorWordText = colorWordText;
        this.buttonContainer = buttonContainer;
        this.answerButtonPrefab = answerButtonPrefab;
        GenerateQuestion();
        timerDuration = 3f;
    }

    private void GenerateQuestion()
    {
        answered = false;

        // Pick a random color name and a random color
        int colorNameIndex = Random.Range(0, colorNames.Count); //this is the colour of the word
        int displayColorIndex = Random.Range(0, colors.Count); //this is the word to display

        correctColorName = colorNames[colorNameIndex];
        displayedColorName = colorNames[displayColorIndex];

        // Set the question text and color word text
        questionText.text = "What is the color of the text (hint: not the meaning)?";
        colorWordText.text = displayedColorName;
        colorWordText.color = colors[colorNameIndex];

        correctAnswerIndex = Random.Range(0, 4);

        ClearButtons();

        // List to store the used incorrect answers
        List<string> usedIncorrectAnswers = new List<string> { correctColorName, displayedColorName };

        // Instantiate answer buttons
        for (int i = 0; i < 4; i++)
        {
            GameObject button = GameObject.Instantiate(answerButtonPrefab, buttonContainer.transform);
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();

            if (i == correctAnswerIndex)
            {
                buttonText.text = correctColorName;
                button.GetComponent<Button>().onClick.AddListener(() => OnAnswerButtonClicked(correctColorName));
            }
            else if (i == (correctAnswerIndex + 1) % 4 && displayedColorName != correctColorName)
            {
                buttonText.text = displayedColorName;
                button.GetComponent<Button>().onClick.AddListener(() => OnAnswerButtonClicked(displayedColorName));
            }
            else
            {
                string incorrectColorName;
                do
                {
                    incorrectColorName = colorNames[Random.Range(0, colorNames.Count)];
                } while (usedIncorrectAnswers.Contains(incorrectColorName));


                usedIncorrectAnswers.Add(incorrectColorName);
                buttonText.text = incorrectColorName;
                button.GetComponent<Button>().onClick.AddListener(() => OnAnswerButtonClicked(incorrectColorName));

            }
        }

    }


    public override void Display()
    {
        questionText.gameObject.SetActive(true);
    }

    public override bool CheckAnswer(string answer)
    {
        return answer == correctColorName;
    }

    private bool IncorrectAnswerExists(string answer)
    {
        foreach (Transform child in buttonContainer.transform)
        {
            if (child.GetComponentInChildren<TextMeshProUGUI>().text == answer)
            {
                return true;
            }
        }
        return false;
    }

    public override void HandleTimeExpired()
    {
        if (!answered)
        {
            OnAnswerButtonClicked(""); // Treat as incorrect answer
        }
    }
    public override void OnAnswerButtonClicked(string v)
    {
        bool isCorrect = CheckAnswer(v);

        // Trigger the GameController to handle the result
        GameObject.FindObjectOfType<QuestionManager>().CheckAnswer(isCorrect);

    }
}


