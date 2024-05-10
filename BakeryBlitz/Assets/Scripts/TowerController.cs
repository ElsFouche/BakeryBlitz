using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

/* Author: Fouche', Els
 * Updated: 04/29/2024
 * Notes:   This is the primary logic handler for tower behavior. 
 *          It creates lists of enemies that are within range and can
 *          prioritize which enemies it targets based on several inspector-set
 *          parameters. TODO: Make it so players can adjust those parameters 
 *          for each tower. 
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

    /// <summary>
    /// Initialize our coroutine that updates our target every findTargetDelay
    /// seconds. 
    /// </summary>
    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0.0f, findTargetDelay);
    }

    /// <summary>
    /// When something enters our range, if it has an EnemyData script add
    /// it to our list of enemies. Normally this would have been handled by
    /// checking the tag manager but in this case proved unnecessary. 
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
    /// When an enemy leaves our range as determined by its tag manager (I don't 
    /// remember why, I'm terrible I know, this should definitely be the same
    /// format as above but I'm scared I'll break it if I change it now) we 
    /// remove it from its spot on the list. 
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
    /// This sets our target based on several modifiable parameters. It searches through
    /// our list of enemies that are within our range, sorting them based on the priority
    /// parameter using a simple highest-value search pattern. 
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

    /// <summary>
    /// Accessor method for determining our current target. 
    /// </summary>
    /// <returns></returns>
    public GameObject FindTarget() { return target; }
}