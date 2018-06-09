using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour {

    public GameObject pausePanel;   //the pause menu in game
    public GameObject pauseTintImage;   //image which makes the game look darker while paused
    public string pauseButtonName;  //the name of button to use when pausing/unpausing
    private bool isGamePaused = false;  //is the game in pause state at given movement
    public Button buttonToHighlight;    //the first button to highlight when menu is opened

    private void Update()
    {
        //listen for pause button press
        if (Input.GetButtonDown(pauseButtonName))
        {
            if(isGamePaused == false)
            {
                //if game is not currently paused, pause and open pause menu
                OpenPauseMenu();
            }
            else
            {
                //if game is currently paused, close pause menu and unpause the game
                HidePauseMenu();
            }
        }
    }

    /// <summary>
    /// Makes the game pause and opens the pause menu.
    /// </summary>
    public void OpenPauseMenu()
    {
        //pause game
        isGamePaused = true;
        GameManagerScript.instance.PauseGame();

        //open pause menu (and pause tint image)
        pauseTintImage.SetActive(true);
        pausePanel.SetActive(true);

        //set continue button active for navigation
        buttonToHighlight.Select();
        
    }

    /// <summary>
    /// Makes the game unpause and closes the pause menu.
    /// </summary>
    public void HidePauseMenu()
    {
        //unpause game
        isGamePaused = false;
        GameManagerScript.instance.UnpauseGame();

        //deselect the active button to prevent issues with reopening the pause menu
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);

        //close pause menu (and pause tint image)
        pauseTintImage.SetActive(false);
        pausePanel.SetActive(false);
    }

    public void HandleContinueButton()
    {
        //acts the same way as pressing pause button
        HidePauseMenu();
    }

    public void HandleMainMenuButton()
    {
        //unpause/unfreeze game
        GameManagerScript.instance.UnpauseGame();
        //load main menu
        SceneLoaderScript.instance.LoadMainMenu();
    }
}
