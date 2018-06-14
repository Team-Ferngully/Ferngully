using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//handles checking whether the character is touching a wall or not
public class WallCheckerScript : MonoBehaviour {

    public float wallCheckDistance = 0.33f; //how far are we checking for a wall from the center of this transform
    public LayerMask wallLayer;             //which layer is considered to be a wall
    public float raycastOffsetY;    //how much above or below the center of transform are we performing the check
    private Vector3 rayOrigin;      //the point where the raycast originates

    public bool isDebugOn = false;  //do we draw gizmo ray to visualize wall raycasting

    /// <summary>
    /// Let's us know whether the character is touching a wall in given direction
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public bool IsInContactWithWall(Vector2 direction)
    {
        //raycast to the given direction to see if wall is found

        rayOrigin = new Vector3(transform.position.x, transform.position.y + raycastOffsetY, transform.position.z);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, wallCheckDistance, wallLayer);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "Ground")
            {
                return true;
            }
        }

        return false;
    }

    void OnDrawGizmosSelected()
    {
        //draw a line which visualizes how far wall contant effects
        if(isDebugOn == true)
        {
            Gizmos.color = Color.red;
            Vector3 destination;
            if(transform.localScale.x < 0)
            {
                destination = new Vector3(rayOrigin.x + (wallCheckDistance * -1), rayOrigin.y, rayOrigin.z);
            }
            else
            {
                destination = new Vector3(rayOrigin.x + (wallCheckDistance * 1), rayOrigin.y, rayOrigin.z);
            }
            Gizmos.DrawLine(rayOrigin, destination);
        }
    }
}
