using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Author:  Fouche', Els
 * Updated: 05/09/2024
 * Notes:   This script handles player health and death scenario.
 *          It communicates with the GameController to allow for
 *          display updating.
 */

public class BakeryController : MonoBehaviour
{
    private int health;
    private int maxHealth;

    /// <summary>
    /// If an enemy hits our bakery at the end of the path, we take damage
    /// based on the damage stat of the enemy. We notify the GameController
    /// that we've taken damage. If we're out of health, we go to the Game
    /// Over scene. Enemies that touch us are destroyed. 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<TagManager>())
        {
            if (other.gameObject.GetComponent<TagManager>().tagType == TagManager.BaseTag.Enemy)
            {
                health -= other.gameObject.GetComponent<EnemyData>().GetEnemyDamage();
                if (health <= 0)
                {
                    GameController.Instance.SwitchScene(3);
                } else
                {
                    GameController.Instance.SetHealth(health);
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
