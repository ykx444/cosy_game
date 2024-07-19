using Content.Source.DataInformation;
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

    [SerializeField] TrialRecord currentTrial = new TrialRecord();

    public TrialRecord CurrentTrialRecord
    {
        get
        {
            return currentTrial;

        }
        set { }
    }

    public int LevelNum
    {
        get { return levelNum; }
    }

    //BOFS
    private string apiURL = "http://127.0.0.1:5000";
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartCoroutine(RequestInformation("/ fetch_condition"));
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


    public IEnumerator RequestInformation(string requestString)
    {
        UnityEngine.Debug.Log("REQUEST");
        yield return new WaitForSeconds(0.001f);
        int selectedCondition = defaultLoadingInCaseOfError; // Go to Basic mode
                                                             // We don't need to wait for this request, since we use only one condition in the experiment
        UnityWebRequest request = new UnityWebRequest(String.Concat(apiURL, requestString));
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
                // Parse the JSON response
                var responseData = JsonUtility.FromJson<ConditionResponse>(result);
                if (responseData != null)
                {
                    UnityEngine.Debug.Log("Participant ID: " + responseData.participantID);
                    UnityEngine.Debug.Log("Condition: " + responseData.condition);

                    selectedCondition = responseData.condition - 1;
                    levelNum = selectedCondition;
                    currentTrial.UserId = responseData.participantID.ToString();
                    currentTrial.Condition = levelNum.ToString();
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log(e.ToString());
                selectedCondition = defaultLoadingInCaseOfError; // Go into Basic mode
            }
        }
    }


    [Serializable]
    public class ConditionResponse
    {
        public int participantID;
        public int condition;
    }
}


