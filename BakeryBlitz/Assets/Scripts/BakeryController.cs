using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Author:  Fouche', Els
 * Updated: 04/30/2024
 * Notes:   This script handles the player life counter
 */

public class BakeryController : MonoBehaviour
{
    private int health;
    private int maxHealth;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<TagManager>())
        {
            if (other.gameObject.GetComponent<TagManager>().tagType == TagManager.BaseTag.Enemy)
            {
                health--;
                if (health <= 0)
                {
                    GameController.Instance.SwitchScene(SceneManager.GetSceneByName("Screen_GameOver"));
                }
                Destroy(other.gameObject);
            }
        }    
    }

    public void SetHealth(int health) { this.health = health; }
    public void SetMaxHealth(int health) { this.maxHealth = health; }
    public int GetHealth() { return this.health; }
    public int GetMaxHealth() { return this.maxHealth; }
}
