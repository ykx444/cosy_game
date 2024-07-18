using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ReCaptchaQuestion : Question
{
    private TextMeshProUGUI uiQuestionText;
    private string prompt;
    private Sprite mainImage;
    private bool[,] correctAnswers;
    private Button[,] gridButtons;
    private Button verifyButton;
    private const int GRIDSIZE = 3;
    public ReCaptchaQuestion(TextMeshProUGUI uiQuestionText, GameObject buttonContainer, GameObject buttonPrefab, string prompt, Sprite mainImage, bool[,] correctAnswers, Button[,] gridButtons, Button verifyButton)
    {
        this.uiQuestionText = uiQuestionText;
        this.buttonContainer = buttonContainer;
        gridButtnPrefab = buttonPrefab;
        this.prompt = prompt;
        this.mainImage = mainImage;
        this.correctAnswers = correctAnswers;
        this.gridButtons = gridButtons;
        this.verifyButton = verifyButton;
        timerDuration = 5f;
        // RemoveGridButtons();
    }

    public override void Display()
    {

        verifyButton.gameObject.SetActive(true);
        // Display the prompt
        uiQuestionText.text = prompt;

        // Crop the source image into a grid of sprites
        Sprite[,] gridImages = CropImageIntoGrid(mainImage.texture, GRIDSIZE, GRIDSIZE);
        // Instantiate the grid buttons
        gridButtons = new Button[GRIDSIZE, GRIDSIZE];
        // Get the dimensions of the container
        RectTransform containerRect = buttonContainer.GetComponent<RectTransform>();
        float containerWidth = containerRect.rect.width;
        float containerHeight = containerRect.rect.height;
        // Calculate button size
        float buttonWidth = containerWidth / GRIDSIZE;
        float buttonHeight = containerHeight / GRIDSIZE;
        for (int i = 0; i < GRIDSIZE; i++)
        {
            for (int j = 0; j < GRIDSIZE; j++)
            {
                GameObject buttonObj = GameObject.Instantiate(gridButtnPrefab, buttonContainer.transform);
                Button button = buttonObj.GetComponent<Button>();
                RectTransform buttonRect = button.GetComponent<RectTransform>();
                buttonRect.sizeDelta = new Vector2(buttonWidth, buttonHeight);

                Image buttonImage = button.GetComponent<Image>();
                buttonImage.sprite = gridImages[i, j];
                int x = i, y = j; // Capture the indices for use in the listener
                button.onClick.AddListener(() => OnGridButtonClicked(x, y));
                gridButtons[i, j] = button;
            }
        }
        verifyButton.onClick.AddListener(VerifyAnswers);
    }

    private Sprite[,] CropImageIntoGrid(Texture2D image, int rows, int columns)
    {
        int pieceWidth = image.width / columns;
        int pieceHeight = image.height / rows;
        Sprite[,] sprites = new Sprite[rows, columns];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                //we inverse the row indexing so it print from bottom to top
                Rect rect = new Rect(j * pieceWidth, (rows - 1 - i) * pieceHeight, pieceWidth, pieceHeight);
                Texture2D piece = new Texture2D(pieceWidth, pieceHeight);
                piece.SetPixels(image.GetPixels(j * pieceWidth, (rows - 1 - i) * pieceHeight, pieceWidth, pieceHeight));
                piece.Apply();

                sprites[i, j] = Sprite.Create(piece, new Rect(0, 0, pieceWidth, pieceHeight), new Vector2(0.5f, 0.5f));
            }
        }
        return sprites;
    }

    private void OnGridButtonClicked(int x, int y)
    {
        Image img = gridButtons[x, y].GetComponent<Image>();
        img.color = img.color == Color.white ? Color.green : Color.white;
    }

    public override void HandleTimeExpired()
    {
        // Check the answers when the time expires
        bool isCorrect = CheckAnswers();
        base.answered = true;
        // Trigger the GameController to handle the result
        GameObject.FindObjectOfType<QuestionManager>().CheckAnswer(isCorrect);
        RemoveButtons();
    }

    private bool CheckAnswers()
    {
        for (int i = 0; i < gridButtons.GetLength(0); i++)
        {
            for (int j = 0; j < gridButtons.GetLength(1); j++)
            {
                if (gridButtons[i, j].gameObject != null)
                {
                    bool isSelected = gridButtons[i, j].GetComponent<Image>().color == Color.green;
                    if (isSelected != correctAnswers[i, j])
                    {
                        return false;
                    }
                }

            }
        }
        RemoveButtons();
        return true;
    }

    private void VerifyAnswers()
    {
        bool isCorrect = CheckAnswers();
        base.answered = true;
        // Trigger the GameController to handle the result
        GameObject.FindObjectOfType<QuestionManager>().CheckAnswer(isCorrect);
     
    }

    private void RemoveButtons()
    {
        foreach (Transform child in buttonContainer.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public override bool CheckAnswer(string answer)
    {
        throw new System.NotImplementedException();
    }

    public override void OnAnswerButtonClicked(string v)
    {
        throw new System.NotImplementedException();
    }
}
