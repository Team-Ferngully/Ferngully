using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour {

    public enum PowerUpTypes { Jump, Dash, WallJump };  //all the selectable power up types
    public PowerUpTypes powerUpType;    //the power up type selected to apply to the player

    private bool isPowerUpActive;   //will this power up apply power up to player on contact
    private GameObject player;      //the player that touches this power up

    public float animationTime;     //how long will the player be locked in "power up applying"
    private Animator anim;          //handles animations for this power up pod

    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();
        HandlePowerUpInit();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //only handle trigger if power up is active and collision is with player
        if(isPowerUpActive == true && collision.gameObject.tag == "Player")
        {
            //store the player gameobject for later disabling/enabling
            player = collision.gameObject;
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

            isPowerUpActive = false;
            StartCoroutine("HandlePowerUpApplying");
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
        //close door
        anim.SetTrigger("CloseDoors");

        //get player position (and which way sprite is facing)
        Vector3 playerPosition = player.transform.position;
        bool isPlayerFacingLeft = player.transform.localScale.x < 0;

        //delete player
        GameObject.Destroy(player);

        //animate "power up installing"?
        yield return new WaitForSeconds(animationTime);

        //set pod to be off and open door
        anim.SetBool("IsPodOn", false);
        anim.SetTrigger("OpenDoors");

        //spawn player
        player = Instantiate(GameManagerScript.instance.GetCurrentPlayerPrefab(), transform.position, transform.rotation);
        //flip player if needed
        if (isPlayerFacingLeft == true)
        {
            player.GetComponent<CharacterControllerScript>().FlipCharacter(-1);
        }

        yield return null;
    }
}
