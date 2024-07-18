using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionActionManager : MonoBehaviour
{

    //such as to interact with something and it shows options
    public GameObject interactionCanvas;
    public TextMeshProUGUI messageTextField;
    public Image speakerSprite;
    public Button choiceButton;
    public Transform buttonContainer;

    private List<Button> activeButtons = new List<Button>();

    private void Start()
    {
        interactionCanvas.gameObject.SetActive(false);
    }

    public void HandleChoiceOpen(string choice)
    {
        if (choice == "Yes")
        {
            // Open the shop
            //show animation
            Debug.Log("Start operation");
            GameManager.instance.customerManager.StartOperation();
           
        }

    }
    public void HandleChoiceClose(string choice)
    {
        if (choice == "Yes")
        {
            // Open the shop
            //show animation
            Debug.Log("end operation");
            GameManager.instance.customerManager.CloseOperation();
            GameManager.instance.scoreManager.ShowResult();
            GameManager.instance.audioManager.PlayBackgroundMusic(GameManager.instance.audioManager.backgroundMusicClips[2]);
        }

    }

    public void HandleShopOpen(string choice)
    {
        if (choice == "Yes")
        {
            //open shop
            //shopPanel.setActive(true);
            GameManager.instance.shopManager.Open();
        }
    }

    public void HandleChoiceTimeSkip(string choice)
    {
        if (choice == "Yes")
        {
            GameManager.instance.timeController.SkipToMorning();
        }
    }

    public void ShowInteraction(string message, List<string> choices, Action<string> onChoiceSelected, Sprite image)
    {
        interactionCanvas.gameObject.SetActive(true);
        messageTextField.text = message;


        if (image == null)
        {
            // Make the sprite invisible by setting its transparency to 0
            Color color = speakerSprite.color;
            color.a = 0;
            speakerSprite.color = color;
        }
        else
        {
            Color color = speakerSprite.color;
            color.a = 1;
            speakerSprite.color = color;
            speakerSprite.sprite = image;
        }

        // Clear any previously active buttons
        foreach (var button in activeButtons)
        {
            Destroy(button.gameObject);
        }
        activeButtons.Clear();

        // Create buttons for each choice
        foreach (var choice in choices)
        {
            Button button = Instantiate(choiceButton, buttonContainer);
            button.gameObject.SetActive(true);
            button.GetComponentInChildren<TextMeshProUGUI>().text = choice;
            button.onClick.AddListener(() => OnChoiceButtonClicked(choice, onChoiceSelected));
            activeButtons.Add(button);
        }

    }

    void OnChoiceButtonClicked(string choice, Action<string> onChoiceSelected)
    {
        interactionCanvas.gameObject.SetActive(false);
        onChoiceSelected(choice);
    }

}
