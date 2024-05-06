using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int enemyHealth;
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

    private void Start()
    {
        speed = gameObject.GetComponent<EnemyMovement>().moveSpeed;
        IDNum = gameObject.GetInstanceID();
        distanceTraveled = 0.0f;

        InvokeRepeating("DistanceTraveled", timeStep, timeStep);
        InvokeRepeating("UpdateSpeed", timeStep, timeStep);
    }

    public int GetEnemyNum() { return IDNum;}
    public float EnemyPriority() { return distanceTraveled; }
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
    public int GetEnemyHealth() {  return enemyHealth;}
    public void SetEnemyHealth(int damage)
    {
        enemyHealth -= damage;

        if (enemyHealth <= 0)
        {
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

    private IEnumerator DamageBlink(MeshRenderer mesh)
    {
        mesh.enabled = false;
        yield return new WaitForSeconds(blinkSpeed);
        mesh.enabled = true;
    }

    private void UpdateSpeed()
    {
        speed = gameObject.GetComponent<EnemyMovement>().moveSpeed;
    }

    private void DistanceTraveled()
    {
        // D = r(dt) where D is distance, r is speed, and d of t is the time step since last check
        distanceTraveled += (speed * timeStep);
    }
}
