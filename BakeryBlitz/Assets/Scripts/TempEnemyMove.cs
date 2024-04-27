using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEnemyMove : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3(0.0f, 0.0f, 1.0f * Time.deltaTime);       
    }
}
