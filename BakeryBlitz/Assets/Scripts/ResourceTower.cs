using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Symon Belcher
 * 5/2/2024
 * handles rescource collecting tower script
 */
public class ResourceTower : MonoBehaviour
{
    public GameObject Tower;
    public int resource;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<TagManager>())
            if (other.gameObject.GetComponent<TagManager>().resourceType == TagManager.ResourceType.Butterfalls)
            {

            }
    }
}
