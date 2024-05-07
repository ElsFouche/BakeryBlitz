using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
 * 5/2/2024
 * Symon Belcher
 * Handles the UI, PLayer Health, Resource amount 
 */
public class UIManager : MonoBehaviour
{
    public TMP_Text healthText;
    public TMP_Text resourceText;

    public void UpdateHealth(int changedHealth)
    {
        healthText.text = "health" + changedHealth;
    }

    public void UpdateResource(int changedResource)
    {
        resourceText.text = "resource" + changedResource;
    }
}