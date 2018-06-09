using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPowerUpChangeListener
{
    void OnPowerUpsChanged();
}

public class PowerUpHolderScript : MonoBehaviour, ISceneSwitchListener {

    public static PowerUpHolderScript instance = null;  //static reference to this instance

    public bool isJumpPowerUpOn;        //has the jump power up been collected?
    public bool isDashPowerUpOn;        //has the dash power up been collected?
    public bool isWallJumpPowerUpOn;    //has the wall jump power up been collected?

    private IPowerUpChangeListener listener;    //a listener for power up changes (player)

    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start ()
    {
        //set as scene switch listener
        SceneLoaderScript.instance.SetSceneSwitchListener(this);
	}

    /// <summary>
    /// Sets the dash power up to be on and notifies any listeners a change in power ups has occured.
    /// </summary>
    public void SetDashPowerUpOn()
    {
        isDashPowerUpOn = true;
        NotifyPowerUpChangeListener();
    }

    /// <summary>
    /// Sets the jump power up to be on and notifies any listeners a change in power ups has occured.
    /// </summary>
    public void SetJumpPowerUpOn()
    {
        isJumpPowerUpOn = true;
        NotifyPowerUpChangeListener();
    }

    /// <summary>
    /// Sets the wall jump power up to be on and notifies any listeners a change in power ups has occured.
    /// </summary>
    public void SetWallJumpPowerUpOn()
    {
        isWallJumpPowerUpOn = true;
        NotifyPowerUpChangeListener();
    }

    //getters for power up statuses
    public bool GetDashPowerUpOn() { return isDashPowerUpOn; }
    public bool GetJumpPowerUpOn() { return isJumpPowerUpOn; }
    public bool GetWallJumpPowerUpOn() { return isWallJumpPowerUpOn; }

    public void ResetPowerUps()
    {
        isDashPowerUpOn = false;
        isJumpPowerUpOn = false;
        isWallJumpPowerUpOn = false;
    }

    /// <summary>
    /// Sets the power up change listener.
    /// </summary>
    /// <param name="listener"></param>
    public void SetPowerUpChangeListener(IPowerUpChangeListener listener)
    {
        this.listener = listener;
    }

    /// <summary>
    /// Clears current power up change listener.
    /// </summary>
    public void ClearPowerUpChangeListener()
    {
        this.listener = null;
    }

    /// <summary>
    /// Handles notifying the power up change listener a change has occurred.
    /// </summary>
    private void NotifyPowerUpChangeListener()
    {
        if(listener != null)
        {
            listener.OnPowerUpsChanged();
        }
    }

    public void OnSceneSwitch()
    {
        ClearPowerUpChangeListener();
    }
}
