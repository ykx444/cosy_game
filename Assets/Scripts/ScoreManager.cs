using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Button closeButton;

    public int coinEarnedToday = 0;
    public int customerServedToday = 0;

    [SerializeField] private TextMeshProUGUI coinScore;
    [SerializeField] private TextMeshProUGUI customerScore;

    public GameObject resultPanel;
    private void Start()
    {
        Close();
    }

    public void Close()
    {
        resultPanel.SetActive(false);
    }


    public void ShowResult()
    {
        resultPanel.SetActive(true);
        PopulateField();
    }

    public void PopulateField()
    {
        coinScore.text = coinEarnedToday.ToString();
        customerScore.text = customerServedToday.ToString();
    }

    public void Reset()
    {
        coinEarnedToday = 0;
        customerServedToday = 0;
    }
}
