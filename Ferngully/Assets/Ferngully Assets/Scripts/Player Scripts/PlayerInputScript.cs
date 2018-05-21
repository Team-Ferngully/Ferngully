using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles catching player's input ie keypresses etc.
/// </summary>
public class PlayerInputScript : MonoBehaviour {

    private CharacterControllerScript characterController;  //the controller we pass our input for

    //check unity settings what these names mean/are
    public string jumpButtonName = "Jump";
    public string dashButtonName = "Fire1";

	// Use this for initialization
	void Start ()
    {
        characterController = GetComponent<CharacterControllerScript>();	
	}
	
	// Update is called once per frame
	void Update ()
    {
        HandleInputs();
	}

    /// <summary>
    /// Reads all player/user input and passes them to character controller.
    /// </summary>
    private void HandleInputs()
    {
        HandleHorizontalMovement();
        HandleJumpPress();
        HandleDashPress();
    }

    /// <summary>
    /// Reads player's horizontal movement (left/right).
    /// </summary>
    private void HandleHorizontalMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        characterController.MoveHorizontally(horizontalInput);
    }

    /// <summary>
    /// Reads player's jump button presses.
    /// </summary>
    private void HandleJumpPress()
    {
        /*
        if (Input.GetButtonDown(jumpButtonName))
        {
            characterController.Jump();
            //return
        }
        */
        //start timer
        //if timer reaches long jump time -> long jump
        if (Input.GetButtonDown(jumpButtonName))
        {
            characterController.Jump();
        }
        else if (Input.GetButton(jumpButtonName))
        {
            characterController.LongJump(false);
        }
        else if(Input.GetButtonUp(jumpButtonName))
        {
            //stop jumping
            characterController.LongJump(true);
        }
        
    }

    

    /// <summary>
    /// Reads player's dash button presses.
    /// </summary>
    private void HandleDashPress()
    {
        if (Input.GetButtonDown(dashButtonName))
        {
            characterController.Dash();
        }
    }
    
}
