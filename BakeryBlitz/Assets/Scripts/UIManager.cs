using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

/*
 * 5/2/2024
 * Symon Belcher
 * Handles the UI, PLayer Health, Resource amount 
 */
public class UIManager : MonoBehaviour
{
    public TMP_Text healthText;
    public TMP_Text resourceText;
    public int nextSceneIndex;

    private CardMovement buttonMove;

    private void Start()
    {
        buttonMove = GetComponentInChildren<CardMovement>();
        buttonMove.DeselectCard();
    }

    public void UpdateHealth(int health, string symbol = "♥")
    {
        healthText.text = "";
        if (health <= 10)
        {
            for (int i = 0; i < health; i++)
            {
                healthText.text += symbol;
            }
        } else
        {
            healthText.text = symbol + ": " + health;
        }
    }

    public void UpdateResource(int changedResource)
    {
        resourceText.text = "" + changedResource;
    }

    public void NextLevelButton()
    {
        buttonMove.SelectCard();
    }

    public void ClickToNextLevel()
    {
        GameController.Instance.SwitchScene(SceneManager.GetSceneByBuildIndex(nextSceneIndex));
    }
}