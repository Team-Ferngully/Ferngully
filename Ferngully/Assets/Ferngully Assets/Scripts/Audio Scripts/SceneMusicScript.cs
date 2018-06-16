using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMusicScript : MonoBehaviour {

    public AudioClip sceneMusicTrack;   //the music track that plays on current scene. Set in editor

	// Use this for initialization
	void Start ()
    {
        AudioManagerScript.instance.PlayMusicTrack(sceneMusicTrack);	
	}
}
