using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

/* What do I want this script to do?
 * This script is supposed to target enemies that are within range of the tower.
 * How does it determine if they're in range?
 * How does it determine which target to shoot at?
 * How does it keep track of the enemies it wants to shoot?
 * 
 */
public class TowerController : MonoBehaviour
{
    private List<EnemyData> enemiesInRange;
    private EnemyData enemyData;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<TagManager>() != null)
        {
            if (other.gameObject.GetComponent<TagManager>().tagType == TagManager.BaseTag.Enemy)
            {
                enemyData = other.gameObject.GetComponent<EnemyData>();
                enemiesInRange.Add(enemyData);
                int enemyNum = enemiesInRange.LastIndexOf(enemyData);
                enemyData.SetEnemyNum(enemyNum);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<TagManager>() != null)
        {
            if (other.gameObject.GetComponent<TagManager>().tagType == TagManager.BaseTag.Enemy)
            {
                enemyData = other.gameObject.GetComponent<EnemyData>();
                enemiesInRange.RemoveAt(enemyData.GetEnemyNum());
            }
        }
    }
}
