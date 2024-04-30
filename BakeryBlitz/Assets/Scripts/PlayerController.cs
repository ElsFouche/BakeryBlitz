using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Symon Belcher
 * 4/30/2024
 * handles player movment throuout the map
 */


public class PlayerController : MonoBehaviour
{
    private Vector3 playerPos = new Vector3(0,0,0);

    public float playerMove = 2;

    public GameObject barriorPointL;
    public GameObject barriorPointR;

    // Update is called once per frame
    void Update()
    {
        playerPos = gameObject.transform.position; // determies the players postion

        if (Input.GetKeyDown(KeyCode.D) && playerPos.x < barriorPointR.transform.position.x)//moves player right
        {
            playerPos.x += playerMove;

            gameObject.transform.position = playerPos;

        }
        if (Input.GetKeyDown(KeyCode.A) && playerPos.x > barriorPointR.transform.position.x)// moves player left
        {
            playerPos.x -= playerMove;

            gameObject.transform.position = playerPos;

        }
        if (Input.GetKeyDown(KeyCode.W) && playerPos.x < barriorPointR.transform.position.z)//moves player up
        {
            playerPos.z += playerMove;

            gameObject.transform.position = playerPos;

        }
        if (Input.GetKeyDown(KeyCode.S) && playerPos.x > barriorPointR.transform.position.z)// moves player down
        {
            playerPos.z -= playerMove;

            gameObject.transform.position = playerPos;

        }
    }

}
