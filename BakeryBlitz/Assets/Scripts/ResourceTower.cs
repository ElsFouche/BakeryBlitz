using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
/*
 * Belcher, Symon
 * Fouche, Els 
 * Machus, Ryan
 * 5/9/2024
 * Handles placing a resource tower and collecting resources by a certain amount over time
 */
public class ResourceTower : MonoBehaviour
{
    public float minDistToTarget = 5.0f;
    public int resourcesPerTower = 50;

    private int towerAmount = 0;
    private bool isGathering = false;
    private int gatheringTime = 10;
    private float minDist = 5;
    private int resourceCount;
    private List<Resource> resourceList = new List<Resource>();


    // Start is called before the first frame update
    void Start()
    {
        Resource[] tempResourceList = FindObjectsOfType<Resource>();
        for (int i = 0; i < tempResourceList.Length; i++)
        {
            resourceList.Add(tempResourceList[i]);
        }
        //find resouces by scrpt
        resourceCount = resourceList.Count;

        //check if within range of any, if so inc toweramt
        for (int i =0; i<resourceCount; i++)
        {
            if ((transform.position - resourceList[i].transform.position).sqrMagnitude <= minDist)
            {
                towerAmount++;
            }
            else
            {
                resourceList.RemoveAt(i);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if the amount of towers is more than 0 and is not currently gathering
        if (towerAmount != 0 && isGathering == false)
        {
            //Gather
            GatherResources();
        }
    }

    /// <summary>
    /// Coroutine to gather resources based on the amount of towers and resources gained per tower
    /// over a set amount of seconds
    /// </summary>
    /// <returns></returns>
    private IEnumerator GatherResources()
    {
        //Set isGathering to true
        isGathering = true;
        //Wait a frame to return the wait time between gathering
        yield return new WaitForSeconds(gatheringTime);
        int temp = 0;
        foreach(Resource r in resourceList) 
        {
            temp += r.GetResources();
        }
        GameController.Instance.AddResources(temp);
        //Set isSpawningWave to false when done
        isGathering = false;
    }
}
//Can make resourceTower travel to nearest resource instead of checking to see if close enough