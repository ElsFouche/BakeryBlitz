using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.Experimental.GraphView;
/*
 * 4/25/2024
 * Symon Belcher
 * Handles the UI, PLayer Health, Resource amount 
 */
public class UIManager : MonoBehaviour
{

    public TMP_Text healthText;
    public TMP_Text resourceText;

    public GameObject upPoint;
    public GameObject downPoint;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            
           gameObject.transform.position = upPoint.transform.position;
          
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {

           gameObject.transform.position = downPoint.transform.position;

        }
    }
}
