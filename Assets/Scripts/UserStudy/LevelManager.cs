using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //this class obtain level number from BOFS
    public static LevelManager instance;
    [SerializeField] private int levelNum; //0 for cosy, 1 for non-cosy
    public bool isMainGameActive = true;
    public bool isMainGameReached = false; //when it reaches 'title' scene, set to true

    public Stopwatch timer;
    int defaultLoadingInCaseOfError = 0;

    float timeElapsed;
    public int LevelNum
    {
        get { return levelNum; }
    }

    //BOFS
    private string apiURL = "http://127.0.0.1:5001/get-condition";
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // StartCoroutine(GetLevelFromServer());
        timer = new Stopwatch();
    }

    private void Update()
    {
        if (isMainGameActive && isMainGameReached)
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= 300f) // 300 seconds = 5 minutes
            {
                SceneManager.LoadScene("End");
                isMainGameActive = false;
                Destroy(GameManager.instance.gameObject);
            }
        }

    }


    private IEnumerator RequestConditionOfExperiment()
    {
        UnityEngine.Debug.Log("REQUEST");
        yield return new WaitForSeconds(0.001f);
        int selectedCondition = defaultLoadingInCaseOfError; // Go to Basic mode
                                                             // We don't need to wait for this request, since we use only one condition in the experiment
        UnityWebRequest request = new UnityWebRequest(String.Concat(apiURL, "/fetch_condition"));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SendWebRequest();
        while (!request.isDone)
        {
            yield return null;
        }
        UnityEngine.Debug.Log(request.downloadHandler.text);
        if (!request.isNetworkError)
        {
            try
            {
                string result = request.downloadHandler.text;
                //FindObjectOfType<PrintConditionID>().PrintCondition(result);
                string output = string.Concat(result.Where(Char.IsDigit));
                int id = Int32.Parse(output);
                selectedCondition = id - 1;
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log(e.ToString());
                selectedCondition = defaultLoadingInCaseOfError; // Go into Basic mode
            }
        }
    }

    private void ProcessLevelResponse(string jsonResponse)
    {
        int levelNum = JsonUtility.FromJson<LevelResponse>(jsonResponse).levelNum;
        this.levelNum = levelNum;
    }
}






[System.Serializable]
public class LevelResponse
{
    public int levelNum;
}
