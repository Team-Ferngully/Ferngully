using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderScript : MonoBehaviour {

    //Static instance of SceneLoader which allows it to be accessed by any other script.
    public static SceneLoaderScript instance = null;

    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Reloads the currently active scene which basically acts as a reset.
    /// </summary>
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Loads the main menu scene which should be the first scene loaded when the game starts.
    /// </summary>
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Loads the desired scene based on its name.
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadSceneByName(string sceneName)
    {
        //Debug.Log("target scene name: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}
