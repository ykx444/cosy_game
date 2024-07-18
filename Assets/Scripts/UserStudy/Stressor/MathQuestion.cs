using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MathQuestion : Question
{
    private string questionText;
    private int correctAnswer;
    private TextMeshProUGUI uiQuestionText;

    private int correctAnswerIndex;
    private int questionNumber;
    private List<string> operations = new List<string> { "+", "-", "*", "/" };



    public MathQuestion(TextMeshProUGUI uiQuestionText, GameObject buttonContainer, GameObject gameButtonPrefab, int questionNumber)
    {
        this.uiQuestionText = uiQuestionText;
        this.answerButtonPrefab = gameButtonPrefab;
        this.buttonContainer = buttonContainer;
        this.questionNumber = questionNumber;
        GenerateQuestion();
        timerDuration = 5f;
    }


    private void GenerateQuestion()
    {
        int num1, num2;
        questionText = "";
        string evalExpression = ""; // Separate evaluation expression
        int operationCount = Mathf.Min(questionNumber / 5 + 1, 4); // Up to 4 operations
        int operationsLength = operations.Count;

        num1 = Random.Range(1, 10);
        questionText += num1.ToString();
        evalExpression += num1.ToString();

        for (int i = 0; i < operationCount; i++)
        {
            string operation = operations[Random.Range(0, operationsLength)];
            num2 = Random.Range(1, 10);

            if (operation == "/" && num1 % num2 != 0)
            {
                // Ensure division results in a whole number
                operation = operations[Random.Range(0, operationsLength - 1)];
            }

            // Use the appropriate symbol for multiplication and division
            string displayOperation = operation;
            if (operation == "*")
            {
                displayOperation = "×"; // Use Unicode multiplication symbol
            }
            else if (operation == "/")
            {
                displayOperation = "÷"; // Use Unicode division symbol
            }

            questionText += $" {displayOperation} {num2}";
            evalExpression += $" {operation} {num2}"; // Use standard symbols for evaluation
            num1 = num2; // Continue the chain
        }

        questionText += " = ?";
        correctAnswer = EvaluateExpression(evalExpression);
        correctAnswerIndex = Random.Range(0, 4); // 4 buttons (0 to 3)
        uiQuestionText.text = questionText;

        // Clear any existing buttons
        ClearButtons();

        // Instantiate answer buttons
        for (int i = 0; i < 4; i++)
        {
            GameObject button = GameObject.Instantiate(answerButtonPrefab, buttonContainer.transform);
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            if (i == correctAnswerIndex)
            {
                buttonText.text = correctAnswer.ToString();
                button.GetComponent<Button>().onClick.AddListener(() => OnAnswerButtonClicked(correctAnswer.ToString()));
            }
            else
            {
                int incorrectAnswer;
                do
                {
                    incorrectAnswer = correctAnswer + Random.Range(-10, 10);
                } while (incorrectAnswer == correctAnswer || IncorrectAnswerExists(incorrectAnswer));

                buttonText.text = incorrectAnswer.ToString();
                button.GetComponent<Button>().onClick.AddListener(() => OnAnswerButtonClicked(incorrectAnswer.ToString()));
            }
        }
    }

    private bool IncorrectAnswerExists(int answer)
    {
        foreach (Transform child in buttonContainer.transform)
        {
            if (child.GetComponentInChildren<TextMeshProUGUI>().text == answer.ToString())
            {
                return true;
            }
        }
        return false;
    }

    private int EvaluateExpression(string expression)
    {
        string formattedExpression = expression.Replace("×", "*").Replace("÷", "/").Replace("=", "").Trim();
        DataTable table = new DataTable();
        table.Columns.Add("expression", typeof(string), formattedExpression);
        DataRow row = table.NewRow();
        table.Rows.Add(row);
        return int.Parse((string)row["expression"]);
    }

    public override void Display()
    {
        uiQuestionText.gameObject.SetActive(true);
        buttonContainer.SetActive(true);
    }

    public override bool CheckAnswer(string answer)
    {
        return answer == correctAnswer.ToString();
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