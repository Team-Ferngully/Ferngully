using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLinkScript : MonoBehaviour, IAnimFinishedListener {

    public string targetSceneName;  //the name of the scene to transition into
    public int targetLinkId;        //the id of scene link we want to use for spawning in the next scene
    public int myLinkId;            //this scene link's id
    public PlayerSpawnerScript playerSpawner;   //the spawn-point used by this scene link. Set in editor

    public bool useExitAnimation;           //is an animation used for scene exiting?
    public enum ExitAnimations { fromRight, fromLeft, fromUp, fromDown};
    public ExitAnimations ExitAnimation;    //the selected exit anim (select in editor)

    public bool useEnterAnimation;
    public enum EnterAnimations { toRight, toLeft, toUp, toDown };
    public EnterAnimations EnterAnimation;

    // Use this for initialization
    void Start ()
    {
        //if this link is used for scene entering, handle player spawning etc through here.
        if(IsThisLinkUsedForEntering() == true)
        {
            EnterScene();
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if player enters the scene link trigger, handle the scene exiting
        if(collision.gameObject.tag == "Player")
        {
            //start scene exit -> transition anim -> load target scene..
            ExitScene();
        }

    }

    /// <summary>
    /// Handles transitioning from current scene to the defined target scene.
    /// </summary>
    public void ExitScene()
    {
        //remove/disable player?
        //gamemanager playerinstance get component input script -> disable

        //set target link id in gamemanager and set/save current player prefab
        GameManagerScript.instance.SetTargetSceneLinkId(targetLinkId);
        //gamemanagerscript instance set current player prefab...

        //set entering to be from a direction (eg scene1 -> scene2 would happen from left to right)
        GameManagerScript.instance.SetEnteringFromDirection(true);

        if(useExitAnimation == false)
        {
            //simply load target scene if no animation is used
            SceneLoaderScript.instance.LoadSceneByName(targetSceneName);
        }
        else
        {
            //if animation is used, set this script as anim finish listener
            TransitionAnimScript.instance.SetAnimFinishListener(this);

            //start anim
            if(ExitAnimation == ExitAnimations.fromRight)
            {
                TransitionAnimScript.instance.SlideFromRightToCenter();
            }
            else if(ExitAnimation == ExitAnimations.fromLeft)
            {
                TransitionAnimScript.instance.SlideFromLeftToCenter();
            }
            else if(ExitAnimation == ExitAnimations.fromUp)
            {
                TransitionAnimScript.instance.SlideFromUpToCenter();
            }
            else if (ExitAnimation == ExitAnimations.fromDown)
            {
                TransitionAnimScript.instance.SlideFromDownToCenter();
            }
        }
    }

    /// <summary>
    /// Handles transitioning from previous scene to current scene.
    /// </summary>
    public void EnterScene()
    {
        //Save game
        SaveManagerScript.instance.SaveGameData();

        //spawn player
        playerSpawner.SpawnPlayer(GameManagerScript.instance.GetCurrentPlayerPrefab());
	
        //if scene enter animation is used, start it
        if(useEnterAnimation == true )
        {
            if( GameManagerScript.instance.GetEnteringFromDirection() == true )
            {
                if (EnterAnimation == EnterAnimations.toRight)
                {
                    TransitionAnimScript.instance.SlideFromCenterToRight();
                }
                else if (EnterAnimation == EnterAnimations.toLeft)
                {
                    TransitionAnimScript.instance.SlideFromCenterToLeft();
                }
                else if (EnterAnimation == EnterAnimations.toUp)
                {
                    TransitionAnimScript.instance.SlideFromCenterToUp();
                }
                else if (EnterAnimation == EnterAnimations.toDown)
                {
                    TransitionAnimScript.instance.SlideFromCenterToDown();
                }
            }
            else
            {
                //fade in anim
                TransitionAnimScript.instance.FadeIn();
            }
            
        }
    }

    /// <summary>
    /// Handles checking whether this scene link is used for entering the current scene.
    /// </summary>
    /// <returns>true if is used for entering</returns>
    private bool IsThisLinkUsedForEntering()
    {
        if(GameManagerScript.instance.GetTargetSceneLinkId() == myLinkId)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void FinishExitAnimation()
    {
        //if exit animation is used and this method is called, switch scene
        if(useExitAnimation == true)
        {
            SceneLoaderScript.instance.LoadSceneByName(targetSceneName);
        }
    }

    public void FinishEnterAnimation()
    {
        //could enable player input or something like that..
        //may not be needed...
    }
}
