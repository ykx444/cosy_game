using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum TransitionType
{
    Teleport,
    SceneTransit
}
public class Transition : MonoBehaviour
{
    [SerializeField] TransitionType type;
    [SerializeField] string sceneNameToTransition;
    [SerializeField] Vector3 targetStartPosition;
    //where you want to pos after change scene
    Transform destination;

    private void Start()
    {
        destination = transform.GetChild(1);
    }

    internal void InitialiseTransition(Transform point)
    {
        switch (type)
        {
            case TransitionType.Teleport:
                point.position = new Vector3(destination.position.x, destination.position.y, point.position.z);
                break;
            case TransitionType.SceneTransit:
                GameSceneManager.instance.SwitchScene(sceneNameToTransition, targetStartPosition);
                break;
            default:
                break;
        }
      
    }
}
