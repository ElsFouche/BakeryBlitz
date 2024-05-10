using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Author:  Fouche', Els
 * Updated: 04/25/24
 * Notes:   This script holds enemy data. It includes methods for accessing 
 *          different values of each enemy that enables the TowerController to
 *          prioritize different targets. 
 */
public class EnemyData : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int enemyHealth;
    public int enemyDamage;
    public int resourceAdded = 3;
    [Tooltip("Enemy blinks once when hit, this \ndetermines how long the enemy is invisible (blinking)")]
    public float blinkSpeed = 0.2f;
    [Tooltip("This value determines how often \ndistance traveled checks are performed")]
    public float timeStep = 0.5f;
    public enum Prioritize
    {
        Furthest,
        HighestHealth,
        LowestHealth,
        Fastest,
        Slowest
    }

    private float speed;
    private float distanceTraveled;
    // Used for determining inverse health & speed relationships:
    private float arbitrariyLargeValue = 10000.0f;  
    // Holds instance ID value:
    private int IDNum;

    /// <summary>
    /// Value initialization and coroutine start. 
    /// </summary>
    private void Start()
    {
        speed = gameObject.GetComponent<EnemyMovement>().moveSpeed;
        IDNum = gameObject.GetInstanceID();
        distanceTraveled = 0.0f;

        InvokeRepeating("DistanceTraveled", timeStep, timeStep);
        InvokeRepeating("UpdateSpeed", timeStep, timeStep);
    }

    /// <summary>
    /// Accessor method for the enemy's unique id number.
    /// </summary>
    /// <returns></returns>
    public int GetEnemyNum() { return IDNum;}
    /// <summary>
    /// Accessor method for the enemy's health. 
    /// </summary>
    /// <returns></returns>
    public int GetEnemyHealth() {  return enemyHealth;}
    /// <summary>
    /// Accessor method for the damage the enemy deals.
    /// </summary>
    /// <returns></returns>
    public int GetEnemyDamage() { return enemyDamage; }
    /// <summary>
    /// Default enemy priority assignment is based on how far the 
    /// enemy has traveled during its lifespan. 
    /// </summary>
    /// <returns></returns>
    public float EnemyPriority() { return distanceTraveled; }
    /// <summary>
    /// Overload method for enemy priority that accepts an int value for 
    /// determining enemy priority. 
    /// 0: distance | 1: highest health | 2: lowest health
    /// 3: highest speed | 4: lowest speed
    /// TODO: Make it so that this accepts
    /// an enum to make it more clear what is being selected for. 
    /// </summary>
    /// <param name="enemyPriority"></param>
    /// <returns></returns>
    public float EnemyPriority(int enemyPriority)
    {
        switch (enemyPriority)
        {
            case 0:
               // Debug.Log("Returning distanceTraveled");
                return distanceTraveled;
            case 1:
                return enemyHealth;
            case 2:
                return arbitrariyLargeValue - enemyHealth;
            case 3:
                return speed;
            case 4:
                return arbitrariyLargeValue - speed;
            default:
               // Debug.Log("Returning default (distanceTraveled)");
                return distanceTraveled;
        }
    }

    /// <summary>
    /// Mutator method for modifying the enemy's health. Additionally
    /// causes the enemy to blink when damaged. 
    /// </summary>
    /// <param name="damage"></param>
    public void SetEnemyHealth(int damage)
    {
        enemyHealth -= damage;

        if (enemyHealth <= 0)
        {
            GameController.Instance.AddResources(resourceAdded);
            Destroy(gameObject);
        }

        foreach(MeshRenderer mesh in gameObject.GetComponentsInChildren<MeshRenderer>())
        {
            if (mesh.enabled) 
            {
                StartCoroutine(DamageBlink(mesh));
            }
        }
    }

    /// <summary>
    /// Causese the enemy to blink once by disabling its mesh and re-
    /// enabling it after blinkSpeed seconds. 
    /// </summary>
    /// <param name="mesh"></param>
    /// <returns></returns>
    private IEnumerator DamageBlink(MeshRenderer mesh)
    {
        mesh.enabled = false;
        yield return new WaitForSeconds(blinkSpeed);
        mesh.enabled = true;
    }

    /// <summary>
    /// Updates this.speed to reflect the enemy's actual movement speed
    /// as set in the EnemyMovement script. 
    /// </summary>
    private void UpdateSpeed()
    {
        speed = gameObject.GetComponent<EnemyMovement>().moveSpeed;
    }

    /// <summary>
    /// Determines distance traveled by the enemy and stores it in 
    /// distanceTraveled as a float using D = r(dt) where D is 
    /// distance, r is speed, and d of t is the time step since last
    /// check. 
    /// </summary>
    private void DistanceTraveled()
    {
        distanceTraveled += (speed * timeStep);
    }
}
