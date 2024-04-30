using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;

/* Author:  Fouche', Els
 * Updated: 04/29/2024
 * Notes:   This is a barebones script for billboarding an object.
 */

public class Billboard : MonoBehaviour
{
    private Camera mainCamera;
    private void Start()
    {
        mainCamera = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(mainCamera.transform.position);
    }
}
