using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEnemyMove : MonoBehaviour
{
    public GameObject target;
    private float speed;
    // Update is called once per frame
    void Update()
    {
        //transform.position -= new Vector3(0.0f, 0.0f, 1.0f * speed * Time.deltaTime);
        transform.LookAt(target.transform.position);
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public void SetSpeed(float setSpeed)
    {
        speed = setSpeed;
    }
}
