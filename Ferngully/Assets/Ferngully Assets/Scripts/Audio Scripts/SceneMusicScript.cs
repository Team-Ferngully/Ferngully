using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMusicScript : MonoBehaviour {

    public AudioClip sceneMusicTrack;   //the music track that plays on current scene. Set in editor
    public bool useSceneMusic;

	// Use this for initialization
	void Start ()
    {
        if(useSceneMusic == true)
            AudioManagerScript.instance.PlayMusicTrack(sceneMusicTrack);	
	}
}
