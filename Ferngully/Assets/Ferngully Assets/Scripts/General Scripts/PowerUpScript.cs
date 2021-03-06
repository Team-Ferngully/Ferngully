﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour {

    public enum PowerUpTypes { Jump, Dash, WallJump };  //all the selectable power up types
    public PowerUpTypes powerUpType;    //the power up type selected to apply to the player

    private bool isPowerUpActive;   //will this power up apply power up to player on contact
    private GameObject player;      //the player that touches this power up

    public float animationTime;     //how long will the player be locked in "power up applying"
    private Animator anim;          //handles animations for this power up pod

    public float applyRangeX = 0.1f;
    public float spawnOffsetY = -0.1f;

    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();
        HandlePowerUpInit();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //only handle trigger if power up is active AND collision is with player AND player is in the middle of the pod
        if(isPowerUpActive == true && collision.gameObject.tag == "Player" && IsPlayerInRange(collision.transform))
        {
            //store the player gameobject for later disabling/enabling
            player = collision.gameObject;
            
            isPowerUpActive = false;
            StartCoroutine("HandlePowerUpApplying");
        }
    }

    private void ApplyCorrectPowerup()
    {
        //handle power up saving and applying...
        if (powerUpType == PowerUpTypes.Dash)
        {
            PowerUpHolderScript.instance.SetDashPowerUpOn();
        }

        if (powerUpType == PowerUpTypes.Jump)
        {
            PowerUpHolderScript.instance.SetJumpPowerUpOn();
        }

        if (powerUpType == PowerUpTypes.WallJump)
        {
            PowerUpHolderScript.instance.SetWallJumpPowerUpOn();
        }
    }

    //Tells whether the player is within power up applying range.
    private bool IsPlayerInRange(Transform playerTransform)
    {
        if(playerTransform.position.x > transform.position.x - applyRangeX 
            && playerTransform.position.x < transform.position.x + applyRangeX)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Will set this power up to be active/not active
    private void HandlePowerUpInit()
    {
        //check if power up has been used already -> make power up inactive...	
        if (powerUpType == PowerUpTypes.Dash && PowerUpHolderScript.instance.GetDashPowerUpOn() == true)
        {
            isPowerUpActive = false;
        }
        else if (powerUpType == PowerUpTypes.Jump && PowerUpHolderScript.instance.GetJumpPowerUpOn() == true)
        {
            isPowerUpActive = false;
        }
        else if (powerUpType == PowerUpTypes.WallJump && PowerUpHolderScript.instance.GetWallJumpPowerUpOn() == true)
        {
            isPowerUpActive = false;
        }
        else
        {
            isPowerUpActive = true;
        }

        //set pod animation to be on if pod is active
        if(isPowerUpActive == true)
        {
            anim.SetBool("IsPodOn", true);
        }
    }

    //Handles animating the power up applying.
    private IEnumerator HandlePowerUpApplying()
    {
        //disable player
        player.GetComponent<CharacterControllerScript>().DisableCharacterMovement();

        //close door
        anim.SetTrigger("CloseDoors");

        //store which way player is facing
        bool isPlayerFacingLeft = player.transform.localScale.x < 0;

        //wait a bit for the doors to close
        yield return new WaitForSeconds(1);

        //delete player (might not be needed, simply translate/teleport player to desired pos?)
        GameObject.Destroy(player);

        //spawn new player with powerup
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + spawnOffsetY, transform.position.z);
        player = Instantiate(GameManagerScript.instance.GetCurrentPlayerPrefab(), spawnPos, transform.rotation);

        //disable player controls until powerup anim finishes
        player.GetComponent<PlayerInputScript>().SetIsListeningForInput(false);

        //flip player if needed
        if (isPlayerFacingLeft == true)
        {
            player.GetComponent<CharacterControllerScript>().FlipCharacter(-1);
        }

        //play power up sound
        player.GetComponent<PlayerSoundEffectsScript>().PlayPowerUp();

        //apply correct anim controller to player
        ApplyCorrectPowerup();

        yield return new WaitForSeconds(animationTime);

        //set pod to be off and open door
        anim.SetBool("IsPodOn", false);
        anim.SetTrigger("OpenDoors");

        yield return new WaitForEndOfFrame();
        //add small delay before enabling player controls
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length*3);

        //enable player controls
        player.GetComponent<PlayerInputScript>().SetIsListeningForInput(true);

        yield return null;
    }
}
