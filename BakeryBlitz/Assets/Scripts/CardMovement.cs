using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMovement : MonoBehaviour
{
    public GameObject upPoint;
    public GameObject downPoint;

    public void SelectCard()
    {
        gameObject.transform.position = upPoint.transform.position;
    }

    public void DeselectCard()
    {
        gameObject.transform.position = downPoint.transform.position;
    }
}
