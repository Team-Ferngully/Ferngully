using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuScript : MonoBehaviour {

    public GameObject mainPanel;    //the main menu. Set in editor
    public Selectable firstElement; //first element to be selected when this panel opens

    public Slider musicSlider;
    public Slider soundSlider;

	// Use this for initialization
	void Start () {
        //load and set audio settings to sliders
        musicSlider.value = AudioManagerScript.instance.MusicVolume;
        soundSlider.value = AudioManagerScript.instance.SoundVolume;
	}
	
    public void HandleBackButton()
    {
        //save audio settings
        SaveManagerScript.instance.SaveMusicVolume(musicSlider.value);
        SaveManagerScript.instance.SaveSoundVolume(soundSlider.value);

        //open main menu panel and close options panel
        mainPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        //set first options panel element to be active for navigation
        firstElement.Select();
    }

    //handle music slider
    public void SetMusicVolume(float volume)
    {
        AudioManagerScript.instance.MusicVolume = volume;
    }

    //handle sound slider
    public void SetSoundVolume(float volume)
    {
        AudioManagerScript.instance.SoundVolume = volume;
    }
}
