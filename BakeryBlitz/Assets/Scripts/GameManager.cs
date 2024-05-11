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
    public List<GameObject> enemyList = new List<GameObject>();
    public GameObject pathHolder;
    public GameObject spawnPoint;
    public int enemiesPerWave = 1;
    public float minSpawnTime = 0;
    public float maxSpawnTime = 5;
    public float waveSpawnTime;
    public int maxWaves = 3;
    [Range(0.0f, 1.99f)]
    public float enemyWeight = 1.0f;
    public float waveDifficultyIncrease = 0.1f;
    public int waveEnemyIncrease = 0;

    //private variables
    private Vector3 spawnLocation;
    private bool isSpawningWave;
    private int enemyCount;
    private int numWaves = 0;
    private bool endlessMode = false;


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
        if (enemyCount <= 0 && !isSpawningWave && (numWaves < maxWaves || endlessMode))
        {
            //Start the coroutine again
            StartCoroutine(SpawnEnemyWave(enemiesPerWave));
            numWaves++;
            enemyWeight = Mathf.Min(enemyWeight + waveDifficultyIncrease, 1.99f);
            enemiesPerWave += waveEnemyIncrease;
        } else if (enemyCount <= 0 && numWaves >= maxWaves && !endlessMode)
        {
            GameController.Instance.EndLevel();
            gameObject.SetActive(false);
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
            int enemySelected = (int)Random.Range(0, enemyWeight) + (int)Random.Range(0, enemyWeight);
            //Instantiate an enemy at the spawnLocation
            GameObject enemy = Instantiate(enemyList[enemySelected], spawnLocation, enemyList[enemySelected].transform.rotation);
            enemy.GetComponent<EnemyMovement>().pathHolder = this.pathHolder;
            enemy.GetComponent<EnemyData>().enemyHealth += (int)(numWaves + numWaves * waveDifficultyIncrease);
            //Random range time to wait between each enemy spawn
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime)); //Time between each enemy spawn
        }
        //Set isSpawningWave to false when done
        isSpawningWave = false;
    }

    public void SetEndlessMode(bool isEndless) 
    {
        endlessMode = isEndless;
        gameObject.SetActive(isEndless);
        this.enabled = isEndless;
        isSpawningWave = false;
        StartCoroutine(SpawnEnemyWave(enemiesPerWave));
    }
}