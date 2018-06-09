using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenuScript : MonoBehaviour {

    public GameObject mainPanel;    //the main menu. Set in editor

	// Use this for initialization
	void Start () {
		
	}
	
    public void HandleBackButton()
    {
        //open main menu panel and close options panel
        mainPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        //set first options panel element to be active for navigation
    }
}
