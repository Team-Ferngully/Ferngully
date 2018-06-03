using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimFinishedListener
{
    void FinishExitAnimation();     //use this to let listener know when an exit animation has finished.
    void FinishEnterAnimation();    //may or may not be needed..
}

public class TransitionAnimScript : MonoBehaviour {

    public static TransitionAnimScript instance;    //static reference to this script

    public Transform transitionSprite;  //the "black box" used in transition animation
    public float transitionSpeed = 5f;  //how fast the black box moves to its target destination
    public Transform upPoint, downPoint, leftPoint, rightPoint, centerPoint;    //all the from and to points
    private Transform destinationPoint; //used for setting the current destination for black box
    private bool isMoving = false;       //is the black box moving / allowed to move

    public bool useFadeInAnimation;     //are fade-in animations on
    public float fadeInAnimTime = 1f;   //how long it takes for fade in to complete

    private SpriteRenderer sr;  //the sprite renderer of transition sprite
    private IAnimFinishedListener animListener; //the listener to tell when an animation is finished

    private void Awake()
    {
        //setting static reference to be this instance
        instance = this;
        //get reference to transition sprite's renderer
        sr = transitionSprite.GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //only move the "black box" when we set it to move..
	    if(isMoving == true)
        {
            //move the black box closer to its destination each frame
            float step = transitionSpeed * Time.deltaTime;
            transitionSprite.position = Vector2.MoveTowards(transitionSprite.position, destinationPoint.position, step);

            //if the black box is almost at its destination, stop moving and let listener know anim is finished
            if(Vector2.Distance(transitionSprite.position, destinationPoint.position) <= 0.1f)
            {
                isMoving = false;
                if(animListener != null)
                {
                    animListener.FinishExitAnimation();
                }
            }
        }	
	}

    /// <summary>
    /// Starts the slide animation from given start point and sets the animation to stop at end point.
    /// </summary>
    /// <param name="startPoint"></param>
    /// <param name="endPoint"></param>
    private void SlideTransitionSprite(Transform startPoint, Transform endPoint)
    {
        transitionSprite.position = startPoint.position;
        destinationPoint = endPoint;
        isMoving = true;
    }

    /// <summary>
    /// Makes the slide animation start from right and end at center.
    /// </summary>
    public void SlideFromRightToCenter()
    {
        SlideTransitionSprite(rightPoint, centerPoint);
    }

    public void SlideFromLeftToCenter()
    {
        SlideTransitionSprite(leftPoint, centerPoint);
    }

    public void SlideFromUpToCenter()
    {
        SlideTransitionSprite(upPoint, centerPoint);
    }

    public void SlideFromDownToCenter()
    {
        SlideTransitionSprite(downPoint, centerPoint);
    }

    public void SlideFromCenterToRight()
    {
        SlideTransitionSprite(centerPoint, rightPoint);
    }

    public void SlideFromCenterToLeft()
    {
        SlideTransitionSprite(centerPoint, leftPoint);
    }

    public void SlideFromCenterToUp()
    {
        SlideTransitionSprite(centerPoint, upPoint);
    }

    public void SlideFromCenterToDown()
    {
        SlideTransitionSprite(centerPoint, downPoint);
    }

    /// <summary>
    /// Starts a fade in animation ie starting from a black screen transition into
    /// a clear screen.
    /// </summary>
    public void FadeIn()
    {
        //if fade in animation is set to be used, start the animation
        if(useFadeInAnimation == true)
        {
            StartCoroutine("HandleFadeIn");
        }
    }

    /// <summary>
    /// Makes the transition sprite appear fully visible in the center of the screen
    /// and then slowly starts to decrease the sprite's alpha.
    /// </summary>
    /// <returns></returns>
    private IEnumerator HandleFadeIn()
    {        
        //sprite to center
        transitionSprite.position = centerPoint.position;

        //lerp sprite alpha from max to zero in fadeInTime
        Color spriteColor = sr.color;
        float alpha;
        float currentTime = 0f;

        while (currentTime < fadeInAnimTime)
        {
            //if another anim (slide) starts, break
            if(isMoving == true)
            {
                break;
            }

            alpha = Mathf.Lerp(1f, 0f, currentTime / fadeInAnimTime);
            sr.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }

        //put sprite away from the middle and set alpha to normal
        transitionSprite.position = rightPoint.position;
        sr.color = spriteColor;
        yield return null;
    }

    /// <summary>
    /// Sets the animation finish listener for callback.
    /// </summary>
    /// <param name="listener"></param>
    public void SetAnimFinishListener(IAnimFinishedListener listener)
    {
        animListener = listener;
    }
}
