using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public GameObject answerButtonPrefab;
    public GameObject answerButtonTransform;
    public GameObject gridButtonPrefab;
    public GameObject gridButtonTransform;
    public GameObject colourWordObject;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI questionNumberText;
    public TextMeshProUGUI timerText;
    public Button[,] gridButtons;

    public AudioClip correctSound;
    public AudioClip wrongSound;
    public GameObject feedbackText;

    private AudioSource audioSource;
    [SerializeField] private Question currentQuestion;
    private int score;
    private int questionNumber;
    private float timer;
    private bool timerRunning = false;

    public Button verifyButton;
    public List<ReCaptchaQuestion> reCaptchaQuestions;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        score = 0;
        questionNumber = 0;
        InitializeReCaptchQuestions();

        UpdateScore();
        NextQuestion();
        feedbackText.SetActive(false);

    }

    void NextQuestion()
    {
        //go to next scene if it reaches 20
        if (questionNumber >= 20)
        {
            SceneManager.LoadScene("POMS");
        }
        //*****preparation
        feedbackText.SetActive(false);
        colourWordObject.SetActive(false);
        verifyButton.gameObject.SetActive(false);

        int questionType = 2; // Default to ReCaptcha
        bool foundUnansweredReCaptcha = false;
        ReCaptchaQuestion tempQuestion = null;
        // Check if there are any unanswered ReCaptcha questions
        foreach (ReCaptchaQuestion item in reCaptchaQuestions)
        {
            if (!item.answered)
            {
                tempQuestion = item;
                foundUnansweredReCaptcha = true;
                break;
            }
        }

        if (!foundUnansweredReCaptcha)
        {
            // If all ReCaptcha questions are answered, choose another type of question
            int randomNumber = Random.Range(0, 2); // Randomly choose between Math (0) or Color Recognition (1)
            questionType = randomNumber;
        }
     

        switch (questionType)
        {
            case 0:
                currentQuestion = new MathQuestion(questionText, answerButtonTransform, answerButtonPrefab, questionNumber);
                break;
            case 1:
                colourWordObject.SetActive(true);
                currentQuestion = new ColorRecognitionQuestion(questionText, colourWordObject.GetComponentInChildren<TextMeshProUGUI>(), answerButtonTransform, answerButtonPrefab);
                break;
            case 2:
                currentQuestion = tempQuestion;
                break;
        }

        // Display the chosen question
        currentQuestion.Display();
        questionNumber++;
        questionNumberText.text = "Question " + questionNumber.ToString() + " out of 20";
        ResetTimer();
    }


    private void InitializeReCaptchQuestions()
    {
        reCaptchaQuestions = new List<ReCaptchaQuestion>
        {
            new ReCaptchaQuestion(
                questionText,
                gridButtonTransform,
                gridButtonPrefab,
                "Tap on all the grids that have camel and click verify button",
                Resources.Load<Sprite>("Images/camel"),
                new bool[,]
                {
                    { false, false, false },
                    { false, true, true },
                    { true, true, true }
                },
                gridButtons,
                verifyButton
            ),
            new ReCaptchaQuestion(
                  questionText,
                  gridButtonTransform,
                   gridButtonPrefab,
                "Tap on all the grids that have lamp post and click verify button",
                Resources.Load<Sprite>("Images/lamppost"),
                new bool[,]
                {
                    { false, true, false },
                    { false, true, false },
                    { false, true, false }
                },
                gridButtons,
                verifyButton
            )
            // Add more ReCaptcha questions here
        };
    }

    public void CheckAnswer(bool isCorrect)
    {
        StartCoroutine(DisplayFeedback(isCorrect));
    }
    private IEnumerator DisplayFeedback(bool isCorrect)
    {
        feedbackText.SetActive(true); // Show feedback text when an answer is selected
        timerRunning = false; //pause timer
        if (isCorrect)
        {
            feedbackText.GetComponent<TextMeshProUGUI>().text = "CORRECT";
            score++;
            audioSource.PlayOneShot(correctSound);
        }
        else
        {
            feedbackText.GetComponent<TextMeshProUGUI>().text = "WRONG";
            audioSource.PlayOneShot(wrongSound);
        }

        UpdateScore();

        // Wait for 2 seconds to display feedback before moving to the next question
        yield return new WaitForSeconds(1.5f);

        NextQuestion();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    //public void OnGridButtonClicked(int x, int y)
    //{
    //    if (currentQuestion is GridMemoryQuestion)
    //    {
    //        ((GridMemoryQuestion)currentQuestion).ToggleGridCell(x, y);
    //    }
    //    else if (currentQuestion is ReCaptchaQuestion)
    //    {
    //        ((ReCaptchaQuestion)currentQuestion).ToggleGridCell(x, y);
    //    }
    //}

    //private Sprite[,] GenerateReCaptchaImages()
    //{
    //    // Implement your method to generate or load the grid images
    //    // For simplicity, let's assume we have a 3x3 grid with dummy sprites
    //    Sprite[,] images = new Sprite[3, 3];
    //    // Fill the array with sprites (load from resources or assign manually)
    //    return images;
    //}

    //private bool[,] GenerateCorrectAnswers()
    //{
    //    // Implement your method to generate the correct answers for the ReCaptcha question
    //    // For simplicity, let's assume we have a 3x3 grid with predefined answers
    //    bool[,] answers = new bool[3, 3];
    //    // Fill the array with correct answers (true for correct cells, false for incorrect cells)
    //    return answers;
    //}

    private void Update()
    {
        if (timerRunning)
        {
            timer -= Time.deltaTime;
            UpdateTimerText();

            if (timer <= 0)
            {
                timerRunning = false;
                timer = 0;
                OnTimerExpired();
            }
        }
    }
    private void ResetTimer()
    {
        timer = currentQuestion.timerDuration;
        timerRunning = true;
        UpdateTimerText();
    }
    private void UpdateTimerText()
    {
        timerText.text = timer.ToString("F0"); // Display the timer as a whole number
    }
    private void OnTimerExpired()
    {
        if (currentQuestion != null)
        {
            currentQuestion.HandleTimeExpired();
        }
    }
}
