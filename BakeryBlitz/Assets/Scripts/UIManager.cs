using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    private GameObject nextLevelButton;

    private void Start()
    {
        nextLevelButton = GetComponentInChildren<Button>().gameObject;
        nextLevelButton.SetActive(false);
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
        nextLevelButton.SetActive(true);
    }

    public void ClickToNextLevel()
    {
        Debug.Log("Next level clicked");
        GameController.Instance.SwitchScene(nextSceneIndex);
    }
}