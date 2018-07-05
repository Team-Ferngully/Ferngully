using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles passing information to animator and choosing which animation controller to use.
/// </summary>
public class PlayerAnimHandler : MonoBehaviour {

    private Animator anim;  //animator that handles player's animations

    //Power ups: J = Jump, D = Dash, W = Wall jump
    public RuntimeAnimatorController defaultAnimator;                       //no powerups
    public RuntimeAnimatorController animatorJ, animatorD, animatorW;       //single powerups
    public RuntimeAnimatorController animatorJD, animatorJW, animatorDW;    //double powerups
    public RuntimeAnimatorController animatorJDW;                           //triple / all powerups

    private void Awake()
    {
        //get reference to the animator
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// Tells the animator whether player is running or not.
    /// </summary>
    /// <param name="isRunning"></param>
    public void SetIsRunning(bool isRunning)
    {
        //sets animator's parameter "isRunning" to be true or false.
        anim.SetBool("isRunning", isRunning);
    }
    public void SetIsJumping(bool isJumping)
    {
        anim.SetBool("isJumping", isJumping);
    }
    public void SetIsDashing(bool isDashing)
    {
        anim.SetBool("isDashing", isDashing);
    }
    public void SetIsWallSliding(bool isWallSliding)
    {
        anim.SetBool("isWallSliding", isWallSliding);
    }

    /// <summary>
    /// Sets player's animation controller based on which power ups are on.
    /// </summary>
    /// <param name="isJumpPowerupOn"></param>
    /// <param name="isDashPowerupOn"></param>
    /// <param name="isWallJumpPowerupOn"></param>
    public void PickCorrectAnimator(bool isJumpPowerupOn, bool isDashPowerupOn, bool isWallJumpPowerupOn)
    {
        if(isJumpPowerupOn == true && isDashPowerupOn == true && isWallJumpPowerupOn == true)
        {
            Debug.Log("using player animator jump+dash+walljump");
        }
        else if(isJumpPowerupOn == true && isDashPowerupOn == true)
        {
            Debug.Log("using player animator jump+dash");
        }
        else if(isJumpPowerupOn == true && isWallJumpPowerupOn == true)
        {
            Debug.Log("using player animator jump+walljump");
        }
        else if(isDashPowerupOn == true && isWallJumpPowerupOn == true)
        {
            Debug.Log("using player animator dash+walljump");
        }
        else if(isJumpPowerupOn == true)
        {
            anim.runtimeAnimatorController = animatorJ;
        }
        else if(isDashPowerupOn == true)
        {
            Debug.Log("using player animator dash");
            anim.runtimeAnimatorController = animatorD;
        }
        else if(isWallJumpPowerupOn == true)
        {
            Debug.Log("using player animator walljump");
            anim.runtimeAnimatorController = animatorW;
        }
        else
        {
            anim.runtimeAnimatorController = defaultAnimator;
        }
    }
}
