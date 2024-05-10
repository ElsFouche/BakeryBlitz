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

    private void Start()
    {
        towerCards.Add(null); // Spacer to allow for easier manipulation later on. 
        for (int i = 0; i < towerCardPrefabs.Count; i++)
        {
            towerCards.Add(towerCardPrefabs[i].GetComponentInChildren<CardMovement>());
        }

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
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.T))
        {
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
/*                case SelectedTower.Gatherer:
                    if (GameController.Instance.PayForTower(gathererTowerCost))
                    {
                        gathererTowerCost += Mathf.Min(1, (int)(cakeTowerCost * costMult));
                        Instantiate(towerPrefabs[(int)selectedTower - 1], transform.position, Quaternion.identity);
                        CreateInvalidPoint();
                    }
                    break;
                    */
                default:
                    break;
            }
        }
    }

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


    // This should be decomposed.
    // Minor alterations can make it check if we're within a bounding box as well.
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