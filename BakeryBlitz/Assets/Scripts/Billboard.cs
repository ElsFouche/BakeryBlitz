using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Author:  Fouche', Els
 * Updated: 04/29/2024
 * Notes:   This is a barebones script for billboarding an object.
 */

public class Billboard : MonoBehaviour
{
    public Camera mainCamera;

    /// <summary>
    /// If a camera hasn't been manually set, get the main camera.
    /// </summary>
    private void Start()
    {
        if (!mainCamera)
        {
            mainCamera = Camera.main;
        }
    }

    /// <summary>
    /// Looks at the specified camera. 
    /// </summary>
    void Update()
    {
        transform.LookAt(mainCamera.transform.position);
    }
}
