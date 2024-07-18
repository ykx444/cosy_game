using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class DayTimeController : MonoBehaviour
{
    //the class controls in game time and returns hours and seconds

    float time;
    const float secondsInDay = 86400f;
    private int day = 1;

    const float phaseLength = 900; //fire timeagent at interval

    [SerializeField] Color nightLightColor;
    [SerializeField] Color dayLightColor = Color.white;
    [SerializeField] AnimationCurve nightTimeCurve;
    [SerializeField] Light2D globalLight;
    [SerializeField] TextMeshProUGUI text;

    [SerializeField] List<Light2D> lights; //update: it only control user's light

    //display
    [SerializeField] float timeScale = 600f; //60s  = 1 min


    List<TimeAgent> agents;

    
    public float Hours
    {
        get { return (time / 3600f) % 24f; } //millisec into seconds, then into hour
    }
    public float Minutes
    {
        get { return time % 3600f / 60f; }
    }

    private void Awake()
    {
        agents = new List<TimeAgent>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        RefreshLightReference();
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    //classes can subscribe to get game time, such as growing, or npc activity6
    public void Subsribe(TimeAgent agent)
    {
        agents.Add(agent);
    }
    public void Unsubscribe(TimeAgent agent)
    {
        agents.Remove(agent);
    }
    private void Start()
    {
        RefreshLightReference();
        time = 7 * 3600f; //initialise to 7am
    }

    private void Update()
    {
        time += Time.deltaTime * timeScale;

        TimeCalculation();
        DayNightCycle();

        if (time > secondsInDay)
        {
            NextDay();
        }

        InvokeTimeAgents();
    }

    int currentPhase = 0;
    private void InvokeTimeAgents()
    {
        //when phase reached, call agent
        //determine when the shift in phase happens, and call time agent
        //->invoke every set minutes in game, call time agent
        int phase = (int)(time / phaseLength);

        if (currentPhase != phase)
        {
            currentPhase = phase;
            for (int i = 0; i < agents.Count; i++)
            {
                agents[i].Invoke();
            }
        }

    }

    private void DayNightCycle()
    {
        float v = nightTimeCurve.Evaluate(Hours);
        Color c = Color.Lerp(dayLightColor, nightLightColor, v);
        //adjust the daynight brightness based on hour

        globalLight.color = c;

        // Control lights based on the time of day
        if (Hours >= 5f && Hours < 18f)
        {
            SetLightsActive(false); // Turn off lights during the day (5 AM to 6 PM)
        }
        else
        {
            SetLightsActive(true); // Turn on lights during the night 
        }
    }

    private void TimeCalculation()
    {
        int hh = (int)Hours;
        int mm = (int)Minutes;
        text.text = "Day " + day + " " + hh.ToString("00") + ":" + mm.ToString("00");
    }

    public void NextDay()
    {
        time = 0;
        day += 1;
    }

    public void SkipToMorning()
    {
        time = time = 7 * 3600f;
        day += 1;
    }

    private void SetLightsActive(bool isActive)
    {
        foreach (Light2D item in lights)
        {
            if (item != null)
                item.enabled = isActive;
        }

    }

    public void RefreshLightReference()
    {
        lights.Clear();
        foreach (GameObject lightObj in GameObject.FindGameObjectsWithTag("Light2D"))
        {
            Light2D light = lightObj.GetComponent<Light2D>();
            if (light != null)
            {
                lights.Add(light);
            }
            else lights.Remove(light);

        }
    }

    //make sure to get lights in the scenes when new scene loaded
}
