using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedSceneSwitcher : MonoBehaviour {

    public string targetSceneName;  //name of the scene we want to switch to
    public float timeTillSwitch = 7;    //how many seconds till we switch to the target scene
    public bool resetSavesOnSwitch = false; //should saves be reset when switching to target scene

	// Use this for initialization
	void Start ()
    {
        //call switch scene at scene start
        StartCoroutine(SwitchScene(timeTillSwitch));	
	}

    private IEnumerator SwitchScene(float delay)
    {
        //wait until switch delay is spent
        yield return new WaitForSeconds(delay);

        //reset save data if it's wanted
        if(resetSavesOnSwitch == true)
        {
            SaveManagerScript.instance.ClearSaveData();
        }

        //load target scene
        SceneLoaderScript.instance.LoadSceneByName(targetSceneName);

        yield return null;
    }
}
