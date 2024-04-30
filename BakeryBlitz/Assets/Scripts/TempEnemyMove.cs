using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEnemyMove : MonoBehaviour
{
    private float speed;
    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3(0.0f, 0.0f, 1.0f * speed * Time.deltaTime);       
    }

    public void SetSpeed(float setSpeed)
    {
        speed = setSpeed;
    }
}
