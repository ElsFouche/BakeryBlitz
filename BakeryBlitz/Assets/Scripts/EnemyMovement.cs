using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Belcher, Symon
 * Fouche, Els 
 * Machus, Ryan
 * Last Updated: 4/25/24
 * This script will control the enemy movement and the path they take on the  map
 */

public class EnemyMovement : MonoBehaviour
{
    //Creating pathpoint from the Point script
    Points[] pathPoint;
    public GameObject Enemy;
    public float moveSpeed;
    private float timer;
    static Vector3 currentPosition;
    static Vector3 startPosition;
    int currentPoint;

    // Start is called before the first frame update
    void Start()
    {
        pathPoint = GetComponentsInChildren<Points>();
        CheckPoint();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime * moveSpeed;
        if (Enemy.transform.position != currentPosition)
        {
            Enemy.transform.position = Vector3.Lerp(startPosition, currentPosition, timer);
        }
        else
        {
            if (currentPoint < pathPoint.Length - 1)
            {
                currentPoint++;
                CheckPoint();
            }
        }
    }

    /// <summary>
    /// Checks the current point and saves point position to currentPosition
    /// </summary>
    private void CheckPoint()
    {
        startPosition = Enemy.transform.position;
        timer = 0;
        currentPosition = pathPoint[currentPoint].transform.position;
    }
}
