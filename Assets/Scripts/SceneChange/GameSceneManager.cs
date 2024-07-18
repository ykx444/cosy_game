using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager instance;
    private void Awake()
    {
        instance = this;
    }

    string currentScene;
    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
    }

    public void SwitchScene(string to, Vector3 startPosition)
    {
        SceneManager.LoadScene(to, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(currentScene);
        currentScene = to;

        //RESET LIGHT
        GameManager.instance.timeController.RefreshLightReference();

        //assign player position
        GameManager.instance.player.transform.position = new Vector3(startPosition.x, startPosition.y, startPosition.z);
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (SceneManager.GetActiveScene().name == "Title")
            {
                SwitchScene("Overworld", new Vector3(0, 0, 0));

            }
        }

    }
}
