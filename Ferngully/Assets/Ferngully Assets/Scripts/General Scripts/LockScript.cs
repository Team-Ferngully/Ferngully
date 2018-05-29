using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles opening a door/wall/obstacle.
/// </summary>
public class LockScript : MonoBehaviour, IKeyTakenListener {

    public int lockId;  //identifies this lock and it should match a lock id

	// Use this for initialization
	void Start ()
    {
        //check in start if lock should be open or not
        OpenLock(false);

        //add this as a key taken listener
        KeyHolderScript.instance.AddKeyTakenListener(this);
	}

    /// <summary>
    /// Handles opening this lock with or without animations.
    /// </summary>
    /// <param name="useAnimations"></param>
    private void OpenLock(bool useAnimations)
    {
        //if key with same id as this lock exists in key holder / player just got a key
        if(KeyHolderScript.instance.IsKeyInKeyHolder(lockId) == true)
        {
            //open lock

            //could do some animation if player got the key in the same scene
            //if useAnimations == true, do anim

            //remove/disable this gameobject
            gameObject.SetActive(false);
        }

    }

    public void HandleKeyTaken()
    {
        //because this gets called when player is in the same scene,
        //animations can be done.
        OpenLock(true);
    }

    private void OnDestroy()
    {
        //remove this from key taken listeners
        KeyHolderScript.instance.RemoveKeyTakenListener(this);
    }
}
