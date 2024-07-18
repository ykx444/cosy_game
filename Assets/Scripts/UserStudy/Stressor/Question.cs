using TMPro;
using UnityEngine;

public abstract class Question
{
    public float timerDuration;
    public abstract void Display();
    public abstract bool CheckAnswer(string answer);
    public bool answered = false;

    public GameObject answerButtonPrefab;
    public GameObject buttonContainer;
    public GameObject gridButtnPrefab;

    public abstract void HandleTimeExpired();

    public abstract void OnAnswerButtonClicked(string v);

    public void ClearButtons()
    {
        // Clear any existing buttons
        foreach (Transform child in buttonContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}

