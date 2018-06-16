using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardHandlerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if player collides with a hazard..
        if (collision.gameObject.tag == "Hazard")
        {
            //kill the player (one hit = death?)
            //freeze player
            //animate death (-> player anim handler)
            //play death sound
            GetComponent<PlayerSoundEffectsScript>().PlayDeath();
            //setup and animate scene reload (quick fade-in)
            GameManagerScript.instance.SetEnteringFromDirection(false);
            SceneLoaderScript.instance.ReloadScene();
        }
    }
}
