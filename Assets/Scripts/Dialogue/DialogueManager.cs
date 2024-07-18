using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText;
    public Image characterImage;
    public TextMeshProUGUI dialogueText;
    public GameObject choicesPanel;
    public GameObject choiceButtonPrefab;

    private Queue<DialogueLine> lines;
    private List<Button> choiceButtons = new();

    private DialogueSO currentDialogue;
    private bool isDisplayingSentence;
    private Coroutine typingCoroutine;
    private bool isContinue = true;

    private void Awake()
    {
        lines = new Queue<DialogueLine>();
    }
    private void Start()
    {
        dialoguePanel.SetActive(false);
    }
    public void StartDialogue(DialogueSO dialogue)
    {
        //freeze movement
        GameManager.instance.Movable = false;

        dialoguePanel.SetActive(true);
        currentDialogue = dialogue;

        lines.Clear();
        foreach (DialogueLine line in dialogue.lines)
        {
            lines.Enqueue(line);
        }
        DisplayNextSentence();
    }

    void DisplayNextSentence()
    {

        //if there is tutorial, display it
        if (lines.Count == 0)
        {
            if (currentDialogue.hasTutorial)
            {
                ShowTutorial();
            }
            if (currentDialogue.nextDialogue != null)
            {
                StartDialogue(currentDialogue.nextDialogue);
                return;
            }
            EndDialogue();
            return;
        }

        DialogueLine line = lines.Dequeue();
        nameText.text = line.characterName;
        characterImage.sprite = line.characterSprite;

        if (line.triggerChoice)
        {
            //if player speaks, make choice.
            HandleChoices();
            // Flip the image horizontally
            characterImage.rectTransform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            // Set the image back to normal
            characterImage.rectTransform.localScale = new Vector3(1, 1, 1);

            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            typingCoroutine = StartCoroutine(TypeSentence(line.sentence));
        }

    }

    private void ShowTutorial()
    {
        GameManager.instance.tutorialManager.TutorialControl();

    }

    private IEnumerator TypeSentence(string sentence)
    {
        isDisplayingSentence = true;
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }

        isDisplayingSentence = false;
    }

    private void OnChoiceSelected(DialogueChoice choice)
    {
        isContinue = true; //is to stop player from pressing space
        foreach (var button in choiceButtons)
        {
            Destroy(button.gameObject);
        }
        choiceButtons.Clear();

        choicesPanel.SetActive(false);

        if (choice.nextDialogue != null)
        {
            StartDialogue(choice.nextDialogue);
        }
        else
        {
            EndDialogue();
        }
    }
    void EndDialogue()
    {
        GameManager.instance.Movable = true;
        dialoguePanel.SetActive(false);
    }

    private void HandleChoices()
    {
        isContinue = false;
        choicesPanel.SetActive(true);
        foreach (var choice in currentDialogue.choices)
        {
            Button choiceButton = Instantiate(choiceButtonPrefab, choicesPanel.transform).GetComponent<Button>();
            choiceButton.GetComponentInChildren<TextMeshProUGUI>().text = choice.choiceText;
            choiceButton.onClick.AddListener(() => OnChoiceSelected(choice));
            choiceButtons.Add(choiceButton);
        }

    }

    private void Update()
    {
        if (dialoguePanel.activeSelf && Input.GetKeyDown(KeyCode.Space) && isContinue)
        {
            if (isDisplayingSentence)
            {
                // If currently typing, stop coroutine and display full sentence
                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                }
                if (lines.Count > 0)
                    dialogueText.text = lines.Peek().sentence; // Display the full sentence
                isDisplayingSentence = false;
            }
            else
            {
                // If not currently typing, display next sentence
                DisplayNextSentence();
            }
        }
    }
}
