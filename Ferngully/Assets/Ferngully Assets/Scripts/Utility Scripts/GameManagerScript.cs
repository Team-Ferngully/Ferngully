using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPauseListener
{
    void OnGamePause();
    void OnGameUnpause();
}

public class GameManagerScript : MonoBehaviour {

    //Static instance of GameManager which allows it to be accessed by any other script.
    public static GameManagerScript instance = null;

    public GameObject currentPlayerPrefab;  //the player model/version we want to use/spawn
    public int targetSceneLinkId;   //used to store which scene link is used for spawning when entering scene
    public bool enterSceneWithoutDirection; //if scene is reloaded and not entered from another scene
    private GameObject playerInstance;  //the player gameobject which is in use currently

    private IPauseListener pauseListener;   //listens for pausing/unpausing and acts on it

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

    // Use this for initialization
    void Start () {
		
	}
	
    public GameObject GetCurrentPlayerPrefab()
    {
        return currentPlayerPrefab;
    }

    public void SetCurrentPlayerPrefab(GameObject playerPrefab)
    {
        currentPlayerPrefab = playerPrefab;
    }

    public int GetTargetSceneLinkId()
    {
        return targetSceneLinkId;
    }

    public void SetTargetSceneLinkId(int id)
    {
        targetSceneLinkId = id;
    }

    public GameObject GetPlayerInstance()
    {
        return playerInstance;
    }

    public void SetPlayerInstance(GameObject player)
    {
        playerInstance = player;
    }

    /// <summary>
    /// Sets whether a directional transition is wanted when a scene is loaded.
    /// For example scene1 to scene 2 transition should be directional enter.
    /// Directionless transition could be used in reloading the same scene.
    /// </summary>
    /// <param name="isEnteringFromDirection"></param>
    public void SetEnteringFromDirection(bool isEnteringFromDirection)
    {
        enterSceneWithoutDirection = isEnteringFromDirection;
    }

    /// <summary>
    /// Gets whether a directional transition should be used in scene load.
    /// </summary>
    /// <returns></returns>
    public bool GetEnteringFromDirection() { return enterSceneWithoutDirection; }

    /// <summary>
    /// Pauses the game by freezing time.
    /// </summary>
    public void PauseGame()
    {
        Time.timeScale = 0;

        //stop listening for player input
        if (pauseListener != null)
        {
            pauseListener.OnGamePause();
        }
    }

    /// <summary>
    /// Unpauses the game by unfreezing time.
    /// </summary>
    public void UnpauseGame()
    {
        Time.timeScale = 1;

        //start listening for player input
        if (pauseListener != null)
        {
            pauseListener.OnGameUnpause();
        }
    }

    //sets a listener for game pausing events
    public void SetPauseListener(IPauseListener listener)   {   pauseListener = listener;   }

    /// <summary>
    /// Resets game manager's data (like scene entering and target scene link id) to default state.
    /// </summary>
    public void ResetGameManagerData()
    {
        //reset scene entering and target scene link id
        SetEnteringFromDirection(false);
        SetTargetSceneLinkId(0);
        //player data doesn't need reseting?
    }
}
