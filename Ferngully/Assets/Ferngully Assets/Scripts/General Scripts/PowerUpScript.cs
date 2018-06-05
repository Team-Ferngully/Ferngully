using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour {

    //the power ups that can be applied when player triggers this power up
    public bool applyDashPowerUp;
    public bool applyJumpPowerUp;
    public bool applyWallJumpPowerUp;

    private bool isPowerUpActive;   //will this power up apply power up to player on contact
    private GameObject player;      //the player that touches this power up

    public float animationTime;     //how long will the player be locked in "power up applying"

    // Use this for initialization
    void Start ()
    {
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
            if (applyDashPowerUp == true)
            {
                PowerUpHolderScript.instance.SetDashPowerUpOn();
            }

            if (applyJumpPowerUp == true)
            {
                PowerUpHolderScript.instance.SetJumpPowerUpOn();
            }

            if (applyWallJumpPowerUp == true)
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
        if (applyDashPowerUp == true && PowerUpHolderScript.instance.GetDashPowerUpOn() == true)
        {
            isPowerUpActive = false;
        }
        else if (applyJumpPowerUp == true && PowerUpHolderScript.instance.GetJumpPowerUpOn() == true)
        {
            isPowerUpActive = false;
        }
        else if (applyWallJumpPowerUp == true && PowerUpHolderScript.instance.GetWallJumpPowerUpOn() == true)
        {
            isPowerUpActive = false;
        }
        else
        {
            isPowerUpActive = true;
        }
    }

    //Handles animating the power up applying.
    private IEnumerator HandlePowerUpApplying()
    {
        //close door

        //get player position (and which way sprite is facing)
        Vector3 playerPosition = player.transform.position;
        bool isPlayerFacingLeft = player.GetComponent<SpriteRenderer>().flipX;

        //delete player
        GameObject.Destroy(player);

        //animate "power up installing"?
        yield return new WaitForSeconds(animationTime);
        //open door

        //spawn player
        player = Instantiate(GameManagerScript.instance.GetCurrentPlayerPrefab(), playerPosition, transform.rotation);
        player.GetComponent<SpriteRenderer>().flipX = isPlayerFacingLeft;

        yield return null;
    }
}
