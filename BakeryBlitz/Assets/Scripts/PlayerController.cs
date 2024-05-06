using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
/*
 * Symon Belcher
 * 4/30/2024
 * handles player movment throuout the map
 * 
 * Author:  Fouche', Els
 * Updated: 04/30/2024
 * Notes:   Player ability to purchase & place towers
 *          
 */


public class PlayerController : MonoBehaviour
{
    public enum SelectedTower
    {
        None,
        Cookie,
        Cake
    }

    [Tooltip("Player move should be adjusted to sync with game grid.")]
    public float playerMove = 2;
    [Tooltip("Lower left boundary point of the map.")]
    public GameObject barriorPointL;
    [Tooltip("Upper right boundary point of the map.")]
    public GameObject barriorPointR;
    [Header("Tower Information")]
    public SelectedTower selectedTower;
    [Tooltip("Add all towers here")]
    public List<GameObject> towers = new List<GameObject>();
    [Header("Enemy Route")]
    [Tooltip("This is required for determining valid tower locations.")]
    public GameObject pathHolder;
    public Material validLocationColor;
    public Material invalidLocationColor;

    private List<Transform> enemyPath = new List<Transform>();
    private List<CardMovement> towerCards = new List<CardMovement>();
    private Vector3 playerPos = new Vector3(0,0,0);
    private bool validPos = false;
    private Renderer playerRenderer;

    private void Start()
    {
        towerCards.Add(null);
        for (int i = 0; i < towers.Count; i++)
        {
            towerCards.Add(towers[i].GetComponentInChildren<CardMovement>());
        }

        foreach (Transform child in pathHolder.transform)
        {
            enemyPath.Add(child);
        }

        playerRenderer = GetComponentInChildren<Renderer>();
    }

    void Update()
    {
        playerPos = gameObject.transform.position; // Determines the player's postion

        if (Input.GetKeyDown(KeyCode.D) && playerPos.x < barriorPointR.transform.position.x)// moves player right
        {
            playerPos.x += playerMove;

            gameObject.transform.position = playerPos;
            CheckIfValidPos();
        }
        if (Input.GetKeyDown(KeyCode.A) && playerPos.x > barriorPointL.transform.position.x)// moves player left
        {
            playerPos.x -= playerMove;

            gameObject.transform.position = playerPos;
            CheckIfValidPos();
        }
        if (Input.GetKeyDown(KeyCode.W) && playerPos.z < barriorPointR.transform.position.z)//moves player up
        {
            playerPos.z += playerMove;

            gameObject.transform.position = playerPos;
            CheckIfValidPos();
        }
        if (Input.GetKeyDown(KeyCode.S) && playerPos.z > barriorPointL.transform.position.z)// moves player down
        {
            playerPos.z -= playerMove;

            gameObject.transform.position = playerPos;
            CheckIfValidPos();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedTower = SelectedTower.Cookie;
            CardSelector(selectedTower);
            PurchaseTower();
        } else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedTower = SelectedTower.Cake;
            CardSelector(selectedTower);
            PurchaseTower();
        }
    }

    private void PurchaseTower()
    { 
        if (validPos)
        {
            switch (selectedTower)
            {
                case SelectedTower.None:
                    break;
                case SelectedTower.Cookie:
                    break;
                case SelectedTower.Cake:
                    break;
                default:
                    break;
            }
        }
    }

    private void CardSelector(SelectedTower towerSelect)
    {
        foreach(CardMovement card in towerCards)
        {
            if (card)
            {
                card.DeselectCard();
            }
        }
        if (towerCards[(int)towerSelect])
        {
            towerCards[(int)towerSelect].SelectCard();
        }
    }

    private void CheckIfValidPos()
    {
        Vector3 tempBound1;
        Vector3 tempBound2;

        for (int i = 0; i < enemyPath.Count - 1; i++)
        {
            if (enemyPath[i].position.sqrMagnitude < enemyPath[i + 1].position.sqrMagnitude) 
            {
                tempBound1 = enemyPath[i].position;
                tempBound2 = enemyPath[i + 1].position;
            } else
            {
                tempBound2 = enemyPath[i].position;
                tempBound1 = enemyPath[i + 1].position;
            }
            
            if ( (transform.position.x >= tempBound1.x && transform.position.x <= tempBound2.x) && 
                 (transform.position.z >= tempBound1.z && transform.position.z <= tempBound2.z) )
            {
                validPos = false;
                playerRenderer.material = invalidLocationColor;
                return;
            }
        }
        validPos = true;
        playerRenderer.material = validLocationColor;
    }
}

/*          Attempt 1:
 *          
            tempBound1 = enemyPath[i].position;
            Debug.Log("point 1: " + tempBound1);
            tempBound2 = enemyPath[i + 1].position;
            Debug.Log("point 2: " + tempBound2);
            Debug.Log("Player position: " + transform.position);
            topTerm =    Mathf.Pow((tempBound2.x - tempBound1.x) * (transform.position.z - tempBound1.z)
                                - (transform.position.x - tempBound1.x) * (tempBound2.z - tempBound1.z), 2);
            
            bottomTerm = Mathf.Pow((tempBound2.x - tempBound1.x), 2) + Mathf.Pow((tempBound2.z - tempBound1.z), 2);
            
            tempDistance = (topTerm / bottomTerm);
            Debug.Log("tempDistance: " + tempDistance);
            if (tempDistance < 5.0f)
            {
                validPos = false;
                playerRenderer.material.SetColor("_Color", invalidLocationColor);
                Debug.Log("Exiting");
                break;
            }
            
            Attempt 2:
    private void CheckIfValidPos()
    {
        Vector3 tempBound1;
        Vector3 tempBound2;
        Vector3 playerPos;

        for (int i = 0; i < enemyPath.Count - 1; i++)
        {
            tempBound1 = enemyPath[i].position;
            tempBound2 = enemyPath[i + 1].position;
            playerPos = transform.position - (tempBound2 - tempBound1);
            Debug.Log("Distance: " + Vector3.Project(playerPos, (tempBound2 - tempBound1)).sqrMagnitude);
            if (Vector3.Project(playerPos, (tempBound2 - tempBound1)).sqrMagnitude <= 2.0f)
            {
                validPos = false;
                playerRenderer.material.SetColor("_Color", invalidLocationColor);
                Debug.Log("Exiting");
                return;
            }
        }
        validPos = true;
        playerRenderer.material.SetColor("_Color", validLocationColor);
    }
 */