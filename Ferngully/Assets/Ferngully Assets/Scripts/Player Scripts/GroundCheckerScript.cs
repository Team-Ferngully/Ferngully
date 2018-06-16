using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles checking whether this gameobject is grounded or not.
/// </summary>
public class GroundCheckerScript : MonoBehaviour {

    private bool isGrounded = false;
    private bool wasGroundedLastCheck = false;

    /// <summary>
    /// Tells whether the gameobject is currently grounded or not.
    /// </summary>
    /// <returns></returns>
    public bool IsCharacterGrounded()
    {
        return isGrounded;
    }

    /// <summary>
    /// Tells whether the gameobject was grounded the last time ground check was made.
    /// </summary>
    /// <returns></returns>
    public bool WasGroundedLastCheck()
    {
        return wasGroundedLastCheck;
    }

    //if the character's trigger collider is touching ground,
    //the character is considered to be grounded
    private void OnTriggerStay2D(Collider2D collision)
    {
        //only colliders with Ground tag will set the character to be grounded.
        if (collision.gameObject.tag == "Ground")
        {
            //if is grounded = false -> was grounded last frame = false
            if(isGrounded == false)
            {
                wasGroundedLastCheck = false;
            }
            else
            {
                wasGroundedLastCheck = true;
            }

            isGrounded = true;
        }
    }

    //if the character's trigger collider stops colliding,
    //the character is no longer grounded
    private void OnTriggerExit2D(Collider2D collision)
    {
        //may or may not need a check what kind of collision exit happened..
        isGrounded = false;
    }
}
