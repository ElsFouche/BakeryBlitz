using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Author:  Fouche', Els
 * Updated: 05/02/2024
 * Notes:   Basic script for moving an object between two
 *          specified locations based on accessible functions.
 */

public class CardMovement : MonoBehaviour
{
    public GameObject upPoint;
    public GameObject downPoint;

    /// <summary>
    /// Move object to upPoint location.
    /// </summary>
    public void SelectCard()
    {
        gameObject.transform.position = upPoint.transform.position;
    }

    /// <summary>
    /// Move object to downPoint location.
    /// </summary>
    public void DeselectCard()
    {
        gameObject.transform.position = downPoint.transform.position;
    }
}
