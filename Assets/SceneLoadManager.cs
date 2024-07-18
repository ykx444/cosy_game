using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    public string essentialSceneName = "Essential";

    void Start()
    {
        // Load new scene additively
        if(SceneManager.GetActiveScene().name=="Title")
        SceneManager.LoadScene(essentialSceneName, LoadSceneMode.Additive);
        LevelManager.instance.isMainGameReached = true;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
