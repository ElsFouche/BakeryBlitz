using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

/* Author: Fouche', Els
 * Updated: 04/29/2024
 * Notes:   
 */
public class TowerController : MonoBehaviour
{
    public enum Prioritize
    {
        Furthest,
        HighestHealth,
        LowestHealth,
        Fastest,
        Slowest
    }

    [Header("Enemy Priority")]
    public Prioritize prioritize;
    [Header("Bullet Properties")]
    public float bulletSpeed;
    public float bulletDistance;
    public float bulletArea;
    public int bulletDamage;
    public int bulletDurability;
    [Range(0.1f, 10.0f)]
    public float fireRate = 1.0f;
    public GameObject bullet;
    [Header("Targeting Properties")]
    [Range(-5.0f, 5.0f)]
    public float leadTarget;
    public float findTargetDelay = 1.0f;

    private List<EnemyData> enemiesInRange = new List<EnemyData>();
    private EnemyData enemyData;
    private GameObject target;

    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0.0f, findTargetDelay);
    }

    /// <summary>
    /// Adds enemies to the list
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<EnemyData>())
        {
           // Debug.Log(other.gameObject.name + " has an Enemy Data script.");
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
    /// TODO: Add switch statement to search for highest health
    /// enemy, lowest health enemy 
    /// Use an Enum to allow the targeting to change in game. 
    /// </summary>
    private void UpdateTarget()
    {
        if (enemiesInRange.Count() > 0)
        {
            float highestPriority = 0.0f;
            foreach (EnemyData enemyData in enemiesInRange)
            {
                if (enemyData.EnemyPriority((int)prioritize) > highestPriority && enemyData)
                {
                    highestPriority = enemyData.EnemyPriority((int)prioritize);
                    // Debug.Log("Highest Priority: " + highestPriority);
                    target = enemyData.gameObject;
                    // Debug.Log("My target is: " +  target);
                }
            }
        } else
        {
            target = null;
           // Debug.Log("I have no target.");
        }
    }

    public GameObject FindTarget() { return target; }
}