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

    public float playerMove = 2;
    public GameObject barriorPointL;
    public GameObject barriorPointR;
    public SelectedTower selectedTower;
    public List<GameObject> towers = new List<GameObject>();

    private List<CardMovement> towerCards = new List<CardMovement>();
    private Vector3 playerPos = new Vector3(0,0,0);

    private void Start()
    {
        towerCards.Add(null);
        for (int i = 0; i < towers.Count; i++)
        {
            towerCards.Add(towers[i].GetComponentInChildren<CardMovement>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = gameObject.transform.position; // determies the players postion

        if (Input.GetKeyDown(KeyCode.D) && playerPos.x < barriorPointR.transform.position.x)// moves player right
        {
            playerPos.x += playerMove;

            gameObject.transform.position = playerPos;
        }
        if (Input.GetKeyDown(KeyCode.A) && playerPos.x > barriorPointL.transform.position.x)// moves player left
        {
            playerPos.x -= playerMove;

            gameObject.transform.position = playerPos;
        }
        if (Input.GetKeyDown(KeyCode.W) && playerPos.z < barriorPointR.transform.position.z)//moves player up
        {
            playerPos.z += playerMove;

            gameObject.transform.position = playerPos;
        }
        if (Input.GetKeyDown(KeyCode.S) && playerPos.z > barriorPointL.transform.position.z)// moves player down
        {
            playerPos.z -= playerMove;

            gameObject.transform.position = playerPos;
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
}
