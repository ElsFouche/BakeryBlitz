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

    private Vector3 targetPos;
    private bool manualTargetSet = false;

    private void Start()
    {
        if (!mainCamera && !target)
        {
            mainCamera = Camera.main;
        } else if (target)
        {
            targetPos = target.transform.position;
        }
    }

    void Update()
    {
        if (mainCamera && !target)
        {
            LookAt(mainCamera.transform.position, offset, x, y, z);
        }
        else if (target && !mainCamera)
        {
            LookAt(targetPos, offset, x, y, z);
        } else if (manualTargetSet)
        {
            LookAt(targetPos, offset, x, y, z);
        }
    }

    public void SetTarget()
    {
        manualTargetSet = false;
        target = null;
        mainCamera = null;
    }

    public void SetTarget(Transform newTarget) 
    {
        manualTargetSet = false;
        target = newTarget;
        targetPos = target.transform.position;
        mainCamera = null; 
    }

    public void SetTarget(Vector3 newTarget)
    {
        manualTargetSet = true;
        targetPos = newTarget;
        mainCamera = null;
    }

    public void SetTarget(Camera newCamera)
    {
        manualTargetSet = false;
        mainCamera = newCamera;
        target = null;
    }

    public void LookAt(Vector3 target)
    {
        transform.LookAt(target);
    }

    public void LookAt(Vector3 target, Vector3 offset, bool x, bool y, bool z)
    {
        float xPos;
        float yPos;
        float zPos;

        if (x)
        {
            xPos = target.x + offset.x;
        }
        else
        {
            xPos = transform.position.x + offset.x;
        }
        if (y)
        {
            yPos = target.y + offset.y;
        }
        else
        {
            yPos = transform.position.y + offset.y;
        }
        if (z)
        {
            zPos = target.z + offset.z;
        }
        else
        {
            zPos = transform.position.z + offset.z;
        }

        Vector3 tempLook = new Vector3(xPos, yPos, zPos);
        transform.LookAt(tempLook);
    }
}