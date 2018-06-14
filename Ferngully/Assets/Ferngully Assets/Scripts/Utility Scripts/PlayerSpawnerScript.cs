using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnerScript : MonoBehaviour {

    public bool playerIsFacingLeft; //is the player facing left when entering the scene

    /// <summary>
    /// Spawns the player with given player prefab.
    /// </summary>
    /// <param name="playerPrefab"></param>
    public void SpawnPlayer(GameObject playerPrefab)
    {
        GameObject player = Instantiate(playerPrefab, transform.position, transform.rotation);
        //player.GetComponent<SpriteRenderer>().flipX = playerIsFacingLeft;
        if(playerIsFacingLeft == true)
        {
            player.GetComponent<CharacterControllerScript>().FlipCharacter(-1);
        }
    }
}
