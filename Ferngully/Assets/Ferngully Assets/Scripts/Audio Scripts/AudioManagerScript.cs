using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour {

    public static AudioManagerScript instance;

    public AudioSource effectSource;   //audio source that plays sound effects like player jump
    public AudioSource walkSource;  //audio source that playes player walk sounds
    public AudioSource musicSource; //audio source that plays music

    //sound volume controls both effect and walk sounds
    public float SoundVolume {
        get { return effectSource.volume; }

        set
        {
            if(value > 1)
            {
                effectSource.volume = 1;
                walkSource.volume = 1;
            }
            else if(value < 0)
            {
                effectSource.volume = 0;
                walkSource.volume = 0;
            }
            else
            {
                effectSource.volume = value;
                walkSource.volume = value;
            }
        }
    }

    public float MusicVolume
    {
        get { return musicSource.volume; }

        set
        {
            if (value > 1)
            {
                musicSource.volume = 1;
            }
            else if (value < 0)
            {
                musicSource.volume = 0;
            }
            else
            {
                musicSource.volume = value;
            }
        }
    }

    void Awake()
    {
        //Check if there is already an instance of SoundManager
        if (instance == null)
            //if not, set it to this.
            instance = this;
        //If instance already exists:
        else if (instance != this)
            //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
            Destroy(gameObject);

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Used to play single sound clips.
    /// </summary>
    /// <param name="clip"></param>
    public void PlaySingleSound(AudioClip clip)
    {
        //if clip is the same as the one playing already, dont do anything
        if(effectSource.clip != null)
        {
            if(clip == effectSource.clip && effectSource.isPlaying == true)
            {
                return;
            }
        }

        //Set the clip of our effectSource audio source to the clip passed in as a parameter.
        effectSource.clip = clip;

        //Play the clip.
        effectSource.Play();
    }

    public void PlayWalkSound(AudioClip clip)
    {
        //if clip is the same as the one playing already, dont do anything
        if (walkSource.clip != null)
        {
            if (walkSource.isPlaying == true)
            {
                return;
            }
        }

        //Set the clip of our effectSource audio source to the clip passed in as a parameter.
        walkSource.clip = clip;

        //Play the clip.
        walkSource.Play();
    }

    /// <summary>
    /// Used to play and change current music track.
    /// </summary>
    /// <param name="clip">music track to play</param>
    public void PlayMusicTrack(AudioClip clip)
    {
        //if clip is the same as the one playing already, dont do anything
        if(clip != musicSource.clip)
        {
            musicSource.clip = clip;

            musicSource.Play();
        }
    }


}
