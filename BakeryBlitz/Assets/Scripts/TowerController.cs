using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

/* What do I want this script to do?
 * This script is supposed to target enemies that are within range of the tower.
 * It's working so far! See TODO at bottom
 */
public class TowerController : MonoBehaviour
{
    public GameObject bullet;
    public float fireRate = 1.0f;
    public float findNewTargetEvery = 1.0f;
    private List<EnemyData> enemiesInRange = new List<EnemyData>();
    private EnemyData enemyData;
    private GameObject target;

    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0.0f, findNewTargetEvery);
    }

    /// <summary>
    /// Adds enemies to the list
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<EnemyData>())
        {
            Debug.Log(other.gameObject.name + " has an Enemy Data script.");
            enemyData = other.gameObject.GetComponent<EnemyData>();
            enemiesInRange.Add(enemyData);
        }
    }

    /// <summary>
    /// Removes enemies from the list
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<TagManager>() != null)
        {
            if (other.gameObject.GetComponent<TagManager>().tagType == TagManager.BaseTag.Enemy)
            {
                enemyData = other.gameObject.GetComponent<EnemyData>();
                enemiesInRange.RemoveAt(enemiesInRange.IndexOf(enemyData));
            }
        }
    }

    /// <summary>
    /// Finds the target duh
    /// </summary>
    private void UpdateTarget()
    {
        if (enemiesInRange.Count() > 0)
        {
            float highestPriority = 0.0f;
            foreach (EnemyData enemyData in enemiesInRange)
            {
                if (enemyData.EnemyPriority() > highestPriority)
                {
                    highestPriority = enemyData.EnemyPriority();
                    // Debug.Log("Highest Priority: " + highestPriority);
                    target = enemyData.gameObject;
                    // Debug.Log("My target is: " +  target);
                }
            }
        } else
        {
            target = null;
            Debug.Log("I have no target.");
        }
    }

    public GameObject FindTarget() { return target; }
}

// TODO: Instantiate bullets 
// TODO: Bullets inherit rotation from turret head
// TODO: Turret head has rotation constraints? 
// TODO: Bullet movement & Logic (movement is only in forward vector because of inherited rotation)