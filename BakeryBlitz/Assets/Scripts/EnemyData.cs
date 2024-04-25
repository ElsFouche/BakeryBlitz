using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    private int num;

    public void SetTransform(Transform setPos)
    {
        transform.position = setPos.position;
    }

    public void SetEnemyNum(int setNum)
    {
        num = setNum;
    }

    public Transform GetTransform() { return transform; }
    public int GetEnemyNum() { return num;}
}
