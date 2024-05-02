using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Belcher, Symon
 * Fouche, Els 
 * Machus, Ryan
 * Last Updated: 5/2/24
 * This script will control the enemy movement and the path they take on the  map
 */

public class EnemyMovement : MonoBehaviour
{
    //Creating pathpoint array of the Points script
    Points[] pathPoint;
    public float moveSpeed;
    public float distToTarget = 1.0f;
    public GameObject pathHolder;
    private Vector3 currentPosition;
    private Vector3 startPosition;
    private int currentPoint;

    // Start is called before the first frame update
    void Start()
    {
        //Fills the array with the children (points) of the path
        pathPoint = pathHolder.GetComponentsInChildren<Points>();
        //Sets the enemy's starting position and the first point
        CheckPoint();
    }

    // Update is called once per frame
    void Update()
    {
        //If the current enemy position distance is more than distToTarget
        if ((transform.position - currentPosition).sqrMagnitude > distToTarget)
        {
            //Look at the currentPosition point
            transform.LookAt(currentPosition);
            //Move towards point looking at
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
        //else if at currentPoint
        else 
        {
            if (currentPoint < pathPoint.Length - 1)
            {
                //Increment to next point in path
                currentPoint++;
                //Update start and currentPosition
                CheckPoint();
            }
        }
        
    }

    /// <summary>
    /// Checks the current enemy and next point positions and saves the vector3's to variables
    /// </summary>
    private void CheckPoint()
    {
        //Sets startPosition as enemy's current position
        startPosition = transform.position;
        //Sets currentPosition as the position of the next point the enemy is headed to
        currentPosition = pathPoint[currentPoint].transform.position;
    }
}
