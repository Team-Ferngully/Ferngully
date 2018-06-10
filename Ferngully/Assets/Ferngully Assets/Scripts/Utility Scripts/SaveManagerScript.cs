using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles saving and loading game data.
/// </summary>
public class SaveManagerScript : MonoBehaviour {

    //Static instance of save manager which allows it to be accessed by any other script.
    public static SaveManagerScript instance = null;

    public bool enableSaving;   //do we allow saving
    public bool clearSaveDataOnStart;   //do we want save data to be cleared when this script starts

    //player pref keys for power ups
    private string dashKey = "dashPowerUp", jumpKey = "jumpPowerUp", wallJumpKey = "wallJumpPowerUp";

    //player pref key for collected keys
    private string collectedKeysKey = "collectedKeys";

    //player pref key for last played scene index
    private string sceneKey = "lastPlayedSceneIndex";

    //player pref key for target scene link id
    private string sceneLinkKey = "targetSceneLinkId";

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
    void Start ()
    {
	    if(clearSaveDataOnStart == true)
        {
            ClearSaveData();
        }	
	}

    /// <summary>
    /// Saves player's current progress in game.
    /// </summary>
    public void SaveGameData()
    {
        if(enableSaving == true)
        {
            //save all saveable data to player prefs
            SavePowerUps();
            SaveKeysCollected();
            SaveLastPlayedScene();
            SaveTargetSceneLink();

            PlayerPrefs.Save();
        }   
    }

    /// <summary>
    /// Loads player's progress in game.
    /// </summary>
    public void LoadGameData()
    {
        //load all saved data (except last played scene which would trigger scene switch)
        LoadPowerUps();
        LoadKeysCollected();
        LoadTargetSceneLink();
    }

    /// <summary>
    /// Deletes all player's save data and resets currently active data.
    /// </summary>
    public void ClearSaveData()
    {
        //use when starting a new game to clear previously saved data (if any exists)

        //power ups
        PlayerPrefs.DeleteKey(dashKey);
        PlayerPrefs.DeleteKey(jumpKey);
        PlayerPrefs.DeleteKey(wallJumpKey);

        //collected keys, last played scene, target scene link
        PlayerPrefs.DeleteKey(collectedKeysKey);
        PlayerPrefs.DeleteKey(sceneKey);
        PlayerPrefs.DeleteKey(sceneLinkKey);

        //reset data which is already loaded
        ResetCurrentData();
    }

    /// <summary>
    /// Tells whether previous player's progress exists. 
    /// </summary>
    /// <returns></returns>
    public bool SaveDataExists()
    {
        //test if one of the save data exists and return result (shouldnt matter which one)
        return PlayerPrefs.HasKey(sceneKey);
    }
	
    //saves dash, jump and wall jump power up data
    private void SavePowerUps()
    {
        //if player has dash power up, save dash power up to 1 (acts as bool true), else 0
        if(PowerUpHolderScript.instance.GetDashPowerUpOn() == true)
        {
            PlayerPrefs.SetInt(dashKey, 1);
        }
        else
        {
            PlayerPrefs.SetInt(dashKey, 0);
        }

        if(PowerUpHolderScript.instance.GetJumpPowerUpOn() == true)
        {
            PlayerPrefs.SetInt(jumpKey, 1);
        }
        else
        {
            PlayerPrefs.SetInt(jumpKey, 0);
        }

        if(PowerUpHolderScript.instance.GetWallJumpPowerUpOn() == true)
        {
            PlayerPrefs.SetInt(wallJumpKey, 1);
        }
        else
        {
            PlayerPrefs.SetInt(wallJumpKey, 0);
        }
    }

    //loads dash, jump and wall jump power up data
    private void LoadPowerUps()
    {
        //if dash power up has been collected previously, set it on (1 acts as true)
        if(PlayerPrefs.GetInt(dashKey) == 1)
        {
            PowerUpHolderScript.instance.SetDashPowerUpOn();
        }

        if(PlayerPrefs.GetInt(jumpKey) == 1)
        {
            PowerUpHolderScript.instance.SetJumpPowerUpOn();
        }

        if(PlayerPrefs.GetInt(wallJumpKey) == 1)
        {
            PowerUpHolderScript.instance.SetWallJumpPowerUpOn();
        }
    }

    //saves collected keys into a string
    private void SaveKeysCollected()
    {
        //convert collected key id list to an string (for example "1,2")
        string keysCollectedString = "";

        int[] keyIdArray = KeyHolderScript.instance.GetKeyIdList().ToArray();

        for (int i = 0; i < keyIdArray.Length; i++)
        {
            if(i < keyIdArray.Length-1)
                keysCollectedString += keyIdArray[i] + ",";
            else
                keysCollectedString += keyIdArray[i] + "";
        }

        PlayerPrefs.SetString(collectedKeysKey, keysCollectedString);
    }

    //loads collected keys from a string
    private void LoadKeysCollected()
    {
        //convert collected keys string into int array and add each key id to key holder
        string keysString = PlayerPrefs.GetString(collectedKeysKey);

        //if no keys have been saved, do nothing
        if(keysString == "")
        {
            return;
        }
        
        int[] keyIdArray = Array.ConvertAll(keysString.Split(','), int.Parse);

        for (int i = 0; i < keyIdArray.Length; i++)
        {
            KeyHolderScript.instance.AddKeyToKeyHolder(keyIdArray[i]);
        }
             
    }

    //saves the scene where player played last
    private void SaveLastPlayedScene()
    {
        PlayerPrefs.SetString(sceneKey, SceneLoaderScript.instance.GetCurrentSceneName());
    }

    /// <summary>
    /// Loads and switches to the scene where player was last in.
    /// </summary>
    public void LoadLastPlayedScene()
    {
        SceneLoaderScript.instance.LoadSceneByName(PlayerPrefs.GetString(sceneKey));
    }

    //saves the scene link id that was used the last.
    private void SaveTargetSceneLink()
    {
        PlayerPrefs.SetInt(sceneLinkKey, GameManagerScript.instance.GetTargetSceneLinkId());
    }

    private void LoadTargetSceneLink()
    {
        GameManagerScript.instance.SetTargetSceneLinkId(PlayerPrefs.GetInt(sceneLinkKey));
    }

    //resets keyholder and power up holder data to be empty
    private void ResetCurrentData()
    {
        KeyHolderScript.instance.ClearKeyHolderKeys();
        PowerUpHolderScript.instance.ResetPowerUps();
        GameManagerScript.instance.ResetGameManagerData();
    }
}
