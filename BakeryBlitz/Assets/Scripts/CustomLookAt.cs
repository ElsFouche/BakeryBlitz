using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Author:  Fouche', Els
 * Updated: 05/08/2024
 * Notes:   This is a more robust lookat script.
 *          It provides additional functionality that allows for
 *          the object to look at a specified target while ignoring
 *          or including the target's x, y, and z position. It accepts
 *          a Vector3 offset that can be used to look ahead or behind
 *          of a target's true location. 
 */
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

    /// <summary>
    /// Initialize our target. Defaults to the main
    /// camera if no targets have been specified. 
    /// </summary>
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

    /// <summary>
    /// We call our modified LookAt function based on what target
    /// we have set. The bool manualTargetSet is used to override 
    /// the script defaulting to the main camera if a target is lost.
    /// </summary>
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

    /// <summary>
    /// Overloaded function set. When called with no arguments,
    /// sets the target to null and notifies the script that we
    /// want to override the default behavior of using the main
    /// camera when we don't have a different target. 
    /// </summary>
    public void SetTarget()
    {
        manualTargetSet = false;
        target = null;
        mainCamera = null;
    }

    /// <summary>
    /// Overload method for setting the target to 
    /// an incoming transform.
    /// </summary>
    /// <param name="newTarget"></param>
    public void SetTarget(Transform newTarget) 
    {
        manualTargetSet = false;
        target = newTarget;
        targetPos = target.transform.position;
        mainCamera = null; 
    }

    /// <summary>
    /// Overload method for setting the target to
    /// an incoming Vector3.
    /// </summary>
    /// <param name="newTarget"></param>
    public void SetTarget(Vector3 newTarget)
    {
        manualTargetSet = true;
        targetPos = newTarget;
        mainCamera = null;
    }

    /// <summary>
    /// Overload method for setting the target to
    /// an incoming Camera object. 
    /// </summary>
    /// <param name="newCamera"></param>
    public void SetTarget(Camera newCamera)
    {
        manualTargetSet = false;
        mainCamera = newCamera;
        target = null;
    }

    /// <summary>
    /// Overload method for basic use of this custom look at.
    /// </summary>
    /// <param name="target"></param>
    public void LookAt(Vector3 target)
    {
        transform.LookAt(target);
    }

    /// <summary>
    /// Primary method for this script. Accepts a Vector3 target value,
    /// a Vector3 offset, and three bools to determine which position values
    /// we want to use from our target. This needs additional debugging.
    /// Current functionality when ignoring any of x, y, or z sets the
    /// relevant position to this game object's position which can result
    /// in unexpected behavior. More robust implementation will involve 
    /// rewriting the main behavior of the default LookAt function (todo). 
    /// </summary>
    /// <param name="target"></param>
    /// <param name="offset"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
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