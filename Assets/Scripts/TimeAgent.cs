using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeAgent : MonoBehaviour
{
    public Action onTimeRun;
    private void Start()
    {
        Init();
    }
    // methods will call init before subscriber class call it
    // so they can be properly subscribe
    public void Init()
    {
        GameManager.instance.timeController.Subsribe(this);
    }
    private void OnDestroy()
    {
        GameManager.instance.timeController.Unsubscribe(this);
    }
    public void Invoke()
    {
        //how much time has passed
        //action
        onTimeRun?.Invoke();
    }
}
