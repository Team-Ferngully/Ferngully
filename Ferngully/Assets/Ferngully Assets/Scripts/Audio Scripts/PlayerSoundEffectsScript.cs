using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffectsScript : MonoBehaviour {

    //running dirt/metal
    public bool useRunningSound;
    public AudioClip running;

    public bool useJumpingSound;
    public AudioClip jumping;

    //public AudioClip landing;   // dirt / metal

    public bool useWallSlidingSound;
    public AudioClip wallSliding;

    public bool useDashingSound;
    public AudioClip dashing;

    public bool usePowerUpSound;
    public AudioClip powerUpApplying;

    public bool useItemGetSound;
    public AudioClip itemGet;

    public bool useDeathSound;
    public AudioClip death;

    private void PlaySoundEffect(AudioClip soundEffect)
    {
        AudioManagerScript.instance.PlaySingleSound(soundEffect);
    }

    public void PlayRunning()
    {
        if(useRunningSound == true)
            AudioManagerScript.instance.PlayWalkSound(running);
    }

    public void PlayJumping()
    {
        PlaySoundEffect(jumping);
    }

    /*
    public void PlayLanding()
    {
        PlaySoundEffect(landing);
    }
    */
    public void PlayWallSliding()
    {
        if(useWallSlidingSound == true)
            PlaySoundEffect(wallSliding);
    }

    public void PlayDashing()
    {
        if(useDashingSound == true)
            PlaySoundEffect(dashing);
    }

    public void PlayPowerUp()
    {
        if(usePowerUpSound == true)
            PlaySoundEffect(powerUpApplying);
    }

    public void PlayItemGet()
    {
        if(useItemGetSound == true)
            PlaySoundEffect(itemGet);
    }

    public void PlayDeath()
    {
        if(useDeathSound == true)
            PlaySoundEffect(death);
    }
}
