using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
/*
 * Symon Belcher
 * 4/30/2024
 * handles player movment throuout the map
 * 
 * Author:  Fouche', Els
 * Updated: 05/09/2024
 * Notes:   This is the primary logic for the player allowing for movement,
 *          purchasing and placing of towers, valid location checking to 
 *          prevent tower placement on enemy paths and other towers, and 
 *          selected card switching to show which tower type is currently
 *          selected and ready for purchase. 
 */


public class PlayerController : MonoBehaviour
{
    public enum SelectedTower
    {
        None,
        Cookie,
        Cake,
        Gatherer
    }

    [Header("Player Control")]
    [Tooltip("Player move should be adjusted to sync with game grid.")]
    public int playerMove = 2;
    [Tooltip("Lower left boundary point of the map.")]
    public GameObject barriorPointL;
    [Tooltip("Upper right boundary point of the map.")]
    public GameObject barriorPointR;
    
    [Header("Tower Information")]
    public SelectedTower selectedTower;
    [Header("Tower Cost")]
    public int cookieTowerCost = 10;
    public int cakeTowerCost = 20;
    public int gathererTowerCost = 30;
    public float costMult = 1.1f;
    [Header("Tower Prefabs")]
    [Tooltip("Add all tower types here.")]
    public List<GameObject> towerPrefabs = new List<GameObject>();
    [Tooltip("Add all tower cards here")]
    public List<GameObject> towerCardPrefabs = new List<GameObject>();

    [Header("Enemy Route")]
    [Tooltip("This is required for determining valid tower locations.")]
    public List<GameObject> pathHolder = new List<GameObject>();
    public Material validLocationColor;
    public Material invalidLocationColor;

    private List<Transform> enemyPath = new List<Transform>();
    private List<CardMovement> towerCards = new List<CardMovement>();
    private Vector3 playerPos = new Vector3(0,0,0);
    private bool validPos = false;
    private Renderer playerRenderer;

    /// <summary>
    /// Initializes two lists, one of the cards we're using for UI elements
    /// and one of the set of points that make up the enemy paths. We offset
    /// the list of cards so that it matches up with our enumerator above due
    /// to 0 being 'none' tower. We also initialize our renderer variable and 
    /// set our default tower to the cookie tower. 
    /// </summary>
    private void Start()
    {
        towerCards.Add(null); // Spacer to allow for easier manipulation later on. 
        for (int i = 0; i < towerCardPrefabs.Count; i++)
        {
            towerCards.Add(towerCardPrefabs[i].GetComponentInChildren<CardMovement>());
        }

        // Null-spaced list of all points that make up enemy routes. 
        for (int i = 0; i < pathHolder.Count; i++)
        {
            foreach (Transform child in pathHolder[i].transform)
            {
                enemyPath.Add(child);
            }
            enemyPath.Add(null);
        }

        playerRenderer = GetComponentInChildren<Renderer>();
        selectedTower = SelectedTower.Cookie;
    }

    /// <summary>
    /// Primary input checker. Determines if the player is within the bounds of the map before 
    /// allowing them to move. Quits the game with escape, allows for tower selection with
    /// the alphanumeric 1 and 2 keys, and allows for tower purchase with many different keys.
    /// </summary>
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameController.Instance.QuitGame();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedTower = SelectedTower.Cookie;
            CardSelector(selectedTower);
        } else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedTower = SelectedTower.Cake;
            CardSelector(selectedTower);
        } else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedTower = SelectedTower.Gatherer;
            CardSelector(selectedTower);
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.T))
        {
            PurchaseTower();
        }
    }

    /// <summary>
    /// Primary logic for purchasing and placing towers.
    /// Only enters if we're currently in a valid position then switches based
    /// on currently selected tower. If the GameController tells us we have 
    /// enough resources, increases the cost of that tower and creates the tower at
    /// our location. The tower cost should be handled in the tower controller script
    /// rather than here (todo). We then call CreateInvalidPoint that adds a set of 
    /// points to prevent the player placing towers on top of each other. 
    /// </summary>
    private void PurchaseTower()
    { 
        if (validPos)
        {
            switch (selectedTower)
            {
                case SelectedTower.None:
                    break;
                case SelectedTower.Cookie:
                    if (GameController.Instance.PayForTower(cookieTowerCost))
                    {
                        cookieTowerCost += Mathf.Min(1, (int)(cookieTowerCost * costMult));
                        Instantiate(towerPrefabs[(int)selectedTower - 1], transform.position, Quaternion.identity);
                        CreateInvalidPoint();
                    }
                    break;
                case SelectedTower.Cake:
                    if (GameController.Instance.PayForTower(cakeTowerCost))
                    {
                        cakeTowerCost += Mathf.Min(1, (int)(cakeTowerCost * costMult));
                        Instantiate(towerPrefabs[(int)selectedTower - 1], transform.position, Quaternion.identity);
                        CreateInvalidPoint();
                    }
                    break;
                case SelectedTower.Gatherer:
                    if (GameController.Instance.PayForTower(gathererTowerCost))
                    {
                        gathererTowerCost += Mathf.Min(1, (int)(cakeTowerCost * costMult));
                        Instantiate(towerPrefabs[(int)selectedTower - 1], transform.position, Quaternion.identity);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// Creates two points and adds them to our null-spaced list that determines
    /// the set of invalid points on the map. Two points are created due to how
    /// the CheckIfValidPos handles validity determination. 
    /// </summary>
    private void CreateInvalidPoint()
    {
        GameObject invalid1 = new GameObject();
        invalid1.transform.position = transform.position;
        GameObject invalid2 = new GameObject();
        invalid2.transform.position = transform.position;
        enemyPath.Add(invalid1.transform);
        enemyPath.Add(invalid2.transform);
        enemyPath.Add(null);
    }

    /// <summary>
    /// When called, sets all cards in our list of cards to their 'down' state
    /// then moves the selected tower to its 'up' state. 
    /// </summary>
    /// <param name="towerSelect"></param>
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

    /// <summary>
    /// This script checks if the player's current position is valid or not by comparing our 
    /// current location to two known points and seeing if we're in between them. This is terrible,
    /// slow, awful implementation. What this script SHOULD do is run once at the start and again
    /// when we place a tower to load a list with a set of points that are invalid by determining 
    /// the points in-between enemy navigation loctions. INSTEAD what this does is it compares the
    /// player's location in real time to each of all sets of two known points (skipping over null
    /// values and beginning at the next set of points) to determine if we're between them or not.
    /// Sets our position to invalid if we're on top of a point or in-between two points.
    /// This should be decomposed and re-written for clarity but it's super critical to the game 
    /// and I don't have time. 
    /// Minor alterations could make it check if we're within a bounding box as well. 
    /// </summary>
    private void CheckIfValidPos()
    {
        Vector3 tempBound1;
        Vector3 tempBound2;

        for (int i = 0; i < enemyPath.Count - 1; i++)
        {
            if (enemyPath[i + 1] == null && i < enemyPath.Count - 2)
            {
                i += 1;
            } else if (i >= enemyPath.Count - 2) 
            {
                break;
            } 
            else
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
            
                if ( ((int)(Mathf.Round(transform.position.x)) >= (int)(Mathf.Round(tempBound1.x)) && (int)(Mathf.Round(transform.position.x)) <= (int)(Mathf.Round(tempBound2.x))) && 
                     ((int)(Mathf.Round(transform.position.z)) >= (int)(Mathf.Round(tempBound1.z)) && (int)(Mathf.Round(transform.position.z)) <= (int)(Mathf.Round(tempBound2.z))))
                {
                    validPos = false;
                    playerRenderer.material = invalidLocationColor;
                    return;
                }
            }
        }
        validPos = true;
        playerRenderer.material = validLocationColor;
    }
}

/*          
        Previous attempts at the above valid position check code. Left in for posterity and future learning. 

            Attempt 1:
         
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