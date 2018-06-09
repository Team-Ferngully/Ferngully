using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour {

    public GameObject optionsPanel; //options panel in main menu. Set in editor
    public string firstRoomName;    //first playable level player enters when hitting start game

	// Use this for initialization
	void Start () {
		
	}
	
    public void HandleStartButton()
    {
        //start game from the first playable scene which should be at index 1 (main menu at 0)
        SceneLoaderScript.instance.LoadSceneByName(firstRoomName);
    }

    public void HandleContinueButton()
    {
        //load the correct scene
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
