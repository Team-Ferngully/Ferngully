using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Controls the attached gameobject's movement.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterControllerScript : MonoBehaviour, IPowerUpChangeListener {

    private SpriteRenderer spriteRenderer;  //this character's sprite renderer
    private Rigidbody2D rigidbody2d;        //this character's rigidbody
    private Vector3 movement;               //movement vector which we pass to the rigidbody as velocity
    private bool isGrounded;                //is the character currently on ground

    private bool areControlsDisabled = false;   //do we handle control input right now

    private GroundCheckerScript groundChecker;  //handles this character's ground checking
    private WallCheckerScript wallChecker;      //handles checking whether this character is touching a wall

    [Header("Basic Movement Settings")]
    public float movementSpeed = 10f;       //how fast the character can move horizontally
    public float jumpSpeed = 10f;           //defines how high the character can jump

    public float longJumpTime = 0.5f;       //how long can the character keep jumping up
    private float jumpTimer = 0;            //keeps time how long character has been jumping
    private bool isLongJumpSpent = true;    //has character already completed a long jump?

    private bool isTouchingWall = false;    //is the character touching a wall
    public float wallSlideSpeed = 2f;       //how fast the character will slide against a wall

    [Header("Wall Jump Settings")]
    public int maxWallJumps = 1;        //how many wall jumps can character do without reset
    private int currentWallJumps = 0;   //how many wall jumps are left at given moment
    public float wallJumpSpeedX = 10f;  //how much horizontal speed for wall jumping
    public float wallJumpSpeedY = 2f;   //how high does wall jumping get the character
    public float wallJumpTime = 0.1f;   //how long does wall jumping take (/disable controls)

    [Header("Dash Settings")]
    public int maxDashCount = 1;        //how many dashes can the character do without reset
    private int currentDashCount = 0;   //how many dashes are left at given moment
    public float dashSpeed = 12f;       //the horizontal speed of the dash
    public float dashTime = 0.2f;       //how long does dashing take (/disable controls)
    public float dashCooldown = 0.02f;  //the time before a second dash can be done
    private bool isDashAllowed = true;  //is the character allowed to dash at given moment

    [Header("Dash Power Up")]
    public bool isDashBonusOn;          //is the dash power up in effect currently
    public int bonusDashes;             //how many dashes are added to the max dash count
    [Header("Jump Power Up")]
    public bool isJumpBonusOn;          //is the jump power up in effect currently
    public float jumpSpeedBonus;        //how much speed is added to the default jump speed
    [Header("Wall Jump Power Up")]
    public bool isWallJumpBonusOn;      //is the wall jump power up in effect currently
    public float jumpSpeedXBonus;       //how much horizontal speed is added to default wall jump
    public float jumpSpeedYBonus;       //how much vertical speed is added to default wall jump
    public bool slowSlide;              //is wall sliding very slow (overrides dafault behaviour)

	// Use this for initialization
	void Start ()
    {
        //get references for used components
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        groundChecker = GetComponent<GroundCheckerScript>();
        wallChecker = GetComponent<WallCheckerScript>();

        PowerUpHolderScript.instance.SetPowerUpChangeListener(this);
        OnPowerUpsChanged();
        //make an animation controller decider to handle which controller to use
            //run it in onpowerUpsChanged...
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void FixedUpdate()
    {
        //reset skills like dash (counters).
        HandleGroundReseting();

        //apply wall sliding if it's relevant at this frame
        HandleWallSliding();

        //apply movement vector to rigidbody's velocity
        rigidbody2d.velocity = movement;
    }

    /// <summary>
    /// Makes the character move horizontally based on given input.
    /// </summary>
    /// <param name="horizontalInput"></param>
    public void MoveHorizontally(float horizontalInput)
    {
        //dont allow movement while controls are disabled
        if (areControlsDisabled == true)
        {
            return;
        }

        //set movement vector's horizontal x axis
        movement = rigidbody2d.velocity;
        movement.x = horizontalInput * movementSpeed;
        
        //make sure character is facing the correct direction based on input
        FlipCharacter(horizontalInput);
    }

    /// <summary>
    /// Changes the direction the character is facing based on direction input.
    /// </summary>
    /// <param name="direction"></param>
    private void FlipCharacter(float direction)
    {
        //flip character to face left(?) if direction is negative and not already facing that way
        //and the opposite for positive direction input

        //flipping done with spriterenderer. Other option is to use transform x scale (-1 and 1)
        if (direction < 0 && spriteRenderer.flipX == false)
        {
            spriteRenderer.flipX = true;
        }
        else if (direction > 0 && spriteRenderer.flipX == true)
        {
            spriteRenderer.flipX = false;
        }
    }

    /// <summary>
    /// Makes the character jump normally or from a wall.
    /// </summary>
    public void Jump()
    {
        //dont allow movement while controls are disabled
        if (areControlsDisabled == true)
        {
            return;
        }

        //is the character grounded when attempting a jump

        //do a wall jump or normal jump  
        if (isTouchingWall == true  && currentWallJumps > 0 && isGrounded == false)    //&& isGrounded == false
        {
            StartCoroutine("HandleWallJump");
        }
        else if(isGrounded == true)
        {
            if(isJumpBonusOn == true)
            {
                movement.y = jumpSpeed+jumpSpeedBonus;
            }
            else
            {
                movement.y = jumpSpeed;
            }          
            //reset long jumping
            isLongJumpSpent = false;
            jumpTimer = 0;
        }
    }

    /// <summary>
    /// Extends the jump length for a short time.
    /// </summary>
    /// <param name="stopJumping"> Singals that current jump should end </param>
    public void LongJump(bool stopJumping)
    {
        //dont allow movement while controls are disabled
        if (areControlsDisabled == true)
        {
            return;
        }
        
        //if we are telling jump to stop or jump timer is up, stop jumping
        //else keep jumping
        if(stopJumping == true)
        {
            isLongJumpSpent = true;
        }
        else if(jumpTimer < longJumpTime && isLongJumpSpent == false)
        {
            if(isJumpBonusOn == true)
            {
                movement.y = jumpSpeed+jumpSpeedBonus;
            }
            else
            {
                movement.y = jumpSpeed;
            }
            
            jumpTimer += Time.deltaTime;
            if(jumpTimer > longJumpTime)
            {
                isLongJumpSpent = true;
            }
        }

    }

    /// <summary>
    /// Makes the character dash in the direction the character is facing.
    /// </summary>
    public void Dash()
    {
        //dont allow movement while controls are disabled
        if (areControlsDisabled == true)
        {
            return;
        }
        //dash if more than 0 dashes left, character isn't touching a wall and cooldown isn't active
        if(currentDashCount > 0 && isTouchingWall == false && isDashAllowed == true)
        {
            StartCoroutine("HandleDash");
        }
    }

    /// <summary>
    /// Makes the character slide against walls while moving towards them.
    /// </summary>
    private void HandleWallSliding()
    {
        //if not grounded and moving against a wall horizontally -> do a constant slide down
        //possible problem: one way effectors might not work..
        //problem: jumping from the ground while moving towards the wall, cancels the jump
        // -> if grounded, dont slide? 
        if (wallChecker.IsInContactWithWall(new Vector2(movement.x, 0)) == true && isGrounded == false)   //&& isGrounded == false
        {
            isTouchingWall = true;
            
            if (rigidbody2d.velocity.y <= 0)
            {
                //wall jump power up makes sliding very slow
                if(isWallJumpBonusOn == true && slowSlide == true)
                {
                    movement.y = 0;
                }
                else
                {
                    movement.y = -wallSlideSpeed;
                }              
            }  
        }
        else
        {
            //is there a way to delay this for a very small time..? Maybe not needed
            isTouchingWall = false;
        }
    }

    //Handles wall-jumping
    private IEnumerator HandleWallJump()
    {
        //set controls to be disabled so that player input doesnt cancel the jump
        areControlsDisabled = true;
        //spend one wall jump
        currentWallJumps--;

        //if walljump bonus is on, add bonuses to jump speeds
        float usedWallJumpSpeedX = wallJumpSpeedX;
        float usedWallJumpSpeedY = wallJumpSpeedY;

        if (isWallJumpBonusOn == true)
        {
            usedWallJumpSpeedX += jumpSpeedXBonus;
            usedWallJumpSpeedY += jumpSpeedYBonus;
        }
        

        //see which way to jump horizontally based on character movement
        if(movement.x > 0)
        {
            movement.x = -usedWallJumpSpeedX;
        }
        else
        {
            movement.x = usedWallJumpSpeedX;
        }
        //flip character based on new horizontal movement
        FlipCharacter(movement.x);
        //add updwards speed
        movement.y = usedWallJumpSpeedY;

        //wait until jump time is done before enabling controls again
        yield return new WaitForSeconds(wallJumpTime);
        movement.y = 0; //resetting vertical movement like this might look better..
        areControlsDisabled = false;
        yield return null;
    }

    //Handles making the character dash forward for a short time.
    private IEnumerator HandleDash()
    {

        //set controls to be disabled so that player input doesnt cancel the dash
        areControlsDisabled = true;
        //spend one dash
        currentDashCount--;

        //dash in the direction the character is facing
        //some other direction check might be better..
        if (spriteRenderer.flipX)
        {
            movement.x = -dashSpeed;
        }
        else
        {
            movement.x = dashSpeed;
        }

        //dont allow vertical movement while dashing
        movement.y = 0; 
        //disable gravity
        float originalGravityScale = rigidbody2d.gravityScale;
        rigidbody2d.gravityScale = 0;

        //yield return new WaitForSeconds(dashTime); //old way without wall stop
        //the wait time could be split into few parts with wall checks for breaking.
        //waitForSeconds isn't accurate thus these numbers are a mess and need tweaking..
        float dashFraction = dashTime / 6;  
        for (float counter = 0; counter <= dashTime; counter += dashFraction)
        {
            if(isTouchingWall == true)
            {
                break;
            }
            else
            {
                yield return new WaitForSeconds(dashFraction);
            }
        }

        //enable gravity and controls
        rigidbody2d.gravityScale = originalGravityScale;
        areControlsDisabled = false;

        //IF grounded add cooldown to dashing / dont allow dashing yet
        if(isGrounded == true)
        {
            isDashAllowed = false;
            yield return new WaitForSeconds(dashCooldown);

            //allow dashing again
            isDashAllowed = true;
        }
        
        yield return null;
    }

    /// <summary>
    /// Handles resetting skills like dash and jumps if character is grounded.
    /// </summary>
    private void HandleGroundReseting()
    {
        if(groundChecker.IsCharacterGrounded() == true)
        {
            isGrounded = true;
            
            currentDashCount = maxDashCount;
            //add dash power up dashes to current count if power up is on
            if (isDashBonusOn == true)
            {
                currentDashCount += bonusDashes;
            }
            //jump reset?
            currentWallJumps = maxWallJumps;
            //long jumping
            //isLongJumpSpent = false;
            //jumpTimer = 0;
        }
        else
        {
            isGrounded = false;
        }
    }

    public void OnPowerUpsChanged()
    {
        //read which power ups are on and apply them

        isJumpBonusOn = PowerUpHolderScript.instance.GetJumpPowerUpOn();
        isDashBonusOn = PowerUpHolderScript.instance.GetDashPowerUpOn();
        isWallJumpBonusOn = PowerUpHolderScript.instance.GetWallJumpPowerUpOn();
        
        //figure out which animator to use?
    }
}
