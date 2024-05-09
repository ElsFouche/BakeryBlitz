using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomLookAt : MonoBehaviour
{
    public Camera mainCamera;
    public Transform target;
    public Vector3 offset;
    public bool x = true;
    public bool y = true;
    public bool z = true;

    private void Start()
    {
        if (!mainCamera && !target)
        {
            mainCamera = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (mainCamera && !target)
        {
            LookAt(mainCamera.transform, offset, x, y, z);
        }
        else if (target && !mainCamera)
        {
            LookAt(target, offset, x, y, z);
        }
    }
    public void SetTarget(Transform newTarget) { target = newTarget; mainCamera = null; }

    public void LookAt(Transform target, Vector3 offset, bool x, bool y, bool z)
    {
        float xPos;
        float yPos;
        float zPos;

        if (x)
        {
            xPos = target.position.x + offset.x;
        }
        else
        {
            xPos = transform.position.x + offset.x;
        }
        if (y)
        {
            yPos = target.position.y + offset.y;
        }
        else
        {
            yPos = transform.position.y + offset.y;
        }
        if (z)
        {
            zPos = target.position.z + offset.z;
        }
        else
        {
            zPos = transform.position.z + offset.z;
        }

        Vector3 tempLook = new Vector3(xPos, yPos, zPos);
        transform.LookAt(tempLook);
    }
}