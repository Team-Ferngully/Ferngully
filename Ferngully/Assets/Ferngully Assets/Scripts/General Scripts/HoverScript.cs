using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverScript : MonoBehaviour {

    public bool allowHovering = true;   //toggles hovering on/off
    public float hoverDistance = 0.5f;  //how far from the starting position will the object move to
    public float hoverSpeed = 1;    //how long it takes to move from point to point

    private Vector3 topPos, bottomPos;  //the end points the object will move between

	// Use this for initialization
	void Start ()
    {
        //set top and bottom position which are the end points the transform will hover between
        topPos = new Vector3(transform.position.x, transform.position.y + hoverDistance, transform.position.z);
        bottomPos = new Vector3(transform.position.x, transform.position.y - hoverDistance, transform.position.z);
    }

    private void FixedUpdate()
    {
        if(allowHovering == true)
        {
            transform.position = Vector3.Lerp(topPos, bottomPos, Mathf.PingPong(Time.time * hoverSpeed, 1.0f));
        }
    }
}
