using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {

    public GameObject optionsPanel; //options panel in main menu. Set in editor
    public string firstRoomName;    //first playable level player enters when hitting start game
    public Button continueButton;   //the button which loads player to last played scene and with correct data

	// Use this for initialization
	void Start ()
    {
        //if save data doesn't exist, disable continue button
		if(SaveManagerScript.instance.SaveDataExists() == false)
        {
            //continueButton.enabled = false;
            continueButton.interactable = false;
        }
	}
	
    public void HandleStartButton()
    {
        //clear save data to start a new game
        SaveManagerScript.instance.ClearSaveData();

        //load "empty" data
        //SaveManagerScript.instance.LoadGameData();

        //set next scene to start without a directional transition anim
        GameManagerScript.instance.SetEnteringFromDirection(false);

        //start game from the first playable scene
        SceneLoaderScript.instance.LoadSceneByName(firstRoomName);
    }

    public void HandleContinueButton()
    {
        //set next scene to start without a directional transition anim
        GameManagerScript.instance.SetEnteringFromDirection(false);

        //load the correct scene
        SaveManagerScript.instance.LoadGameData();
        SaveManagerScript.instance.LoadLastPlayedScene();
    }

    public void HandleOptionsButton()
    {
        //open options menu panel and close main panel
        optionsPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void HandleExitButton()
    {
        //closes the application
        Application.Quit();
    }

    private void OnEnable()
    {
        //set first main panel element to be active for navigation
    }
}
