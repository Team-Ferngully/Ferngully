using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles adding a key(id) to the keyholder and disabling itself when taken.
/// </summary>
public class KeyScript : MonoBehaviour {

    public int keyId;   //used to tell which key is which and which lock it interacts with

	// Use this for initialization
	void Start () {
        //if key has already been taken, it shouldn't be spawned in scenes anymore..
        DisableUsedKey();
	}

    /// <summary>
    /// Disables this key if it already has been taken / exists in keyholder.
    /// </summary>
    private void DisableUsedKey()
    {
        //if this key has already been taken, disable it
        if(KeyHolderScript.instance.IsKeyInKeyHolder(keyId) == true)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Adds this key(id) to the keyholder.
    /// </summary>
    private void TakeKey()
    {
        //put the key in key holder..
        KeyHolderScript.instance.AddKeyToKeyHolder(keyId);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //only check for player collision
        if(collision.gameObject.tag == "Player")
        {
            //take key
            TakeKey();

            //handle some animations? screenshake, shine/bling anim/sound.
            collision.gameObject.GetComponent<PlayerSoundEffectsScript>().PlayItemGet();

            //remove/disable this gameobject
            gameObject.SetActive(false);
        }
    }
}
