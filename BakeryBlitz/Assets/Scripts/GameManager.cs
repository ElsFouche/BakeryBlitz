using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Belcher, Symon
 * Fouche, Els 
 * Machus, Ryan
 * Last Updated: 5/2/24
 * This script will control the enemy spawning and wave initiation 
 */
public class GameManager : MonoBehaviour
{
    //public variables
    public GameObject Enemy;
    public GameObject spawnPoint;
    public int enemiesPerWave = 1;
    public float minSpawnTime = 0;
    public float maxSpawnTime = 5;
    public float waveSpawnTime;

    //private variables
    private Vector3 spawnLocation;
    private bool isSpawningWave;
    private int enemyCount;


    // Start is called before the first frame update
    void Start()
    {
        //Set spawnLocation vector3 at the spawnpoint's location
        spawnLocation = spawnPoint.transform.position;
        //Start the SpawnEnemyWave coroutine, taking how many enemiesPerWave
        StartCoroutine(SpawnEnemyWave(enemiesPerWave));
    }

    // Update is called once per frame
    void Update()
    {
        //enemyCount amount will find objects of type that have the EnemyMovement script
        enemyCount = FindObjectsOfType<EnemyMovement>().Length;
        //If enemyCount hits 0 and not spawning a wave
        if (enemyCount == 0 && !isSpawningWave)
        {
            //Start the coroutine again
            StartCoroutine(SpawnEnemyWave(enemiesPerWave));
        }
    }

    /// <summary>
    /// Coroutine that takes the enemy number to spawn and will start a spawn wave
    /// </summary>
    /// <param name="enemyNumberToSpawn"></param>
    /// <returns></returns>
    private IEnumerator SpawnEnemyWave(int enemyNumberToSpawn)
    {
        //Set isSpawningWave to true
        isSpawningWave = true;
        //Wait a frame to return the wait time between waves
        yield return new WaitForSeconds(waveSpawnTime); //Waiting between waves
        //For starting at 0, increase until it reaches the amount of enemies to spawn
        for (int i = 0; i < enemyNumberToSpawn; i++)
        {
            //Instantiate an enemy at the spawnLocation
            Instantiate(Enemy, spawnLocation, Enemy.transform.rotation);
            //Random range time to wait between each enemy spawn
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime)); //Time between each enemy spawn
        }
        //Set isSpawningWave to false when done
        isSpawningWave = false;
    }
}
