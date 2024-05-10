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
    // public int resourcesPerTower = 50;
    public float speed = 3.0f;
    public int gatheringTime = 10;

    private int towerAmount = 0;
    private bool isGathering = false;
    private float minDist = 5;
    private int resourceCount;
    private List<Resource> resourceList = new List<Resource>();
    private bool targetReached = false;
    private int targetIndex;


    // Start is called before the first frame update
    void Start()
    {
        //find resouces by scrpt
        Resource[] tempResourceList = FindObjectsOfType<Resource>();
        for (int i = 0; i < tempResourceList.Length; i++)
        {
            resourceList.Add(tempResourceList[i]);
        }
        resourceCount = resourceList.Count;

        targetIndex = (int)Random.Range(0, resourceList.Count - 1 + 0.99f);


/*        //check if within range of any, if so inc toweramt
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
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (resourceList.Count > 0 && (transform.position - resourceList[targetIndex].transform.position).sqrMagnitude > 0.01f)
        {
            transform.LookAt(resourceList[targetIndex].transform.position);
            transform.position += transform.forward * speed * Time.deltaTime;
        } else if (!targetReached)
        {
            targetReached = true;
            CheckForNearbyResources();
        } else if (towerAmount > 0 && !isGathering)
        {
            //Gather
            StartCoroutine(GatherResources());
        }
    }

    private void CheckForNearbyResources()
    {
        //check if within range of any, if so inc toweramt
        for (int i = 0; i < resourceCount; i++)
        {
            if ((transform.position - resourceList[i].transform.position).sqrMagnitude <= minDist)
            {
                towerAmount++;
                Debug.Log(towerAmount);
            }
/*            else
            {
                resourceList.RemoveAt(i);
            }*/
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
            if (r)
            {
                temp += r.GetResources();
            }
        }
        GameController.Instance.AddResources(temp);
        //Set isSpawningWave to false when done
        isGathering = false;
    }
}
//Can make resourceTower travel to nearest resource instead of checking to see if close enough