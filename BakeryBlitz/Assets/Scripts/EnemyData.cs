using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    public float timeStep = 0.5f;

    private int IDNum;
    private int speed;
    private float distanceTraveled;

    private void Start()
    {
        //speed = gameObject.GetComponent<EnemyMovement>().moveSpeed;
        speed = 1; // TODO: Remove this! This is for testing. 
        IDNum = gameObject.GetInstanceID();
        distanceTraveled = 0.0f;

        InvokeRepeating("DistanceTraveled", timeStep, timeStep);
        InvokeRepeating("UpdateSpeed", timeStep, timeStep);
    }

    public int GetEnemyNum() { return IDNum;}
    public float EnemyPriority() { return distanceTraveled; }

    private void UpdateSpeed()
    {
        // speed = gameObject.GetComponent<EnemyMovement>().moveSpeed;
        speed = 1; // TODO: Remove this and uncomment the above. Testing only!
    }

    private void DistanceTraveled()
    {
        // D = r(dt) where D is distance, r is speed, and d of t is the time step since last check
        distanceTraveled += (speed * timeStep);
    }
}
