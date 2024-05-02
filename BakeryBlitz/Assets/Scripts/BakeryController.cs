using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Author:  Fouche', Els
 * Updated: 04/30/2024
 * Notes:   This script handles the player life counter
 */

public class BakeryController : MonoBehaviour
{
    public int health;
    public int maxHealth;

    private TitleScreens sceneManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<TagManager>())
        {
            if (other.gameObject.GetComponent<TagManager>().tagType == TagManager.BaseTag.Enemy)
            {
                health--;
                if (health <= 0)
                {
                    sceneManager.SwitchScene(3);
                }
                Destroy(other.gameObject);
            }
        }    
    }
}
