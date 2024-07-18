using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    //manage tutorial object, each tutorial page = 1 tutorial obj.
    public DialogueSO[] dialogue;

    public GameObject tutorialPanel;

    public List<Tutorial> farmTutorials;
    public List<Tutorial> cauldronTutorials;

    private List<Tutorial> currentList;
    public Image imageDisplay;
    public TextMeshProUGUI headerText;
    public TextMeshProUGUI contentText;

    //[SerializeField] private int closeCounter = 0; //to count how many times a tutorial is closed = how many tutorial it completed

    public Button closeButton;

    private int currentIndex = 0;
    public void InteractWithNPC(DialogueSO dialogue, float delay = 0f)
    {
        StartCoroutine(StartDialogueAfterDelay(dialogue, delay));
    }

    private void Start()
    {
        tutorialPanel.SetActive(false);
        InteractWithNPC(dialogue[0], 1f);
    }
    private IEnumerator StartDialogueAfterDelay(DialogueSO dialogue, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.instance.dialogueManager.StartDialogue(dialogue);
    }

    //tutorial images
    public void TutorialControl()
    {
        if (SceneManager.GetActiveScene().name == "Overworld")
        {
            currentList = farmTutorials;
        }
        else currentList = cauldronTutorials;
        currentIndex = 0;
        ShowTutorial();
    }

    public void SetCurrentTutorial(int i)
    {
        //based on index, assign tutorial to show
        switch (i)
        {
            case 0:
                currentList = farmTutorials;
                break;
            case 1:
                currentList = cauldronTutorials;
                break;
            default:
                break;
        }
        currentIndex = 0;
        ShowTutorial();
    }
    public void ShowTutorial()
    {
        tutorialPanel.SetActive(true);
        Tutorial currentTutorial = currentList[currentIndex];

        // Display image
        imageDisplay.sprite = currentTutorial.image;

        // Display header and content
        headerText.text = currentTutorial.header;
        contentText.text = currentTutorial.content;
    }

    //button
    public void ShowNext()
    {
        if (currentIndex < currentList.Count - 1)
        {
            currentIndex++;
            ShowTutorial();
        }
        //last slide
    }

    public void Show(bool show)
    {
        //closeButton.gameObject.SetActive(!show);
        tutorialPanel.SetActive(show);
        //closeCounter++;
    }

    public void ShowPrevious()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            ShowTutorial();
        }

    }

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Scene targetScene = SceneManager.GetSceneByName("PlayerInterior");
        if (targetScene.isLoaded)
        {
            InteractWithNPC(dialogue[1], 2f);
        }

    }
}
