using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class TowerBullet : MonoBehaviour
{
    public float bulletSpeed;
    public float bulletDistance;
    public float bulletArea;
    public int bulletDamage;

    void Start()
    {
        StartCoroutine("DespawnBullet");
    }

    void Update()
    {
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<TagManager>())
        {
            if (other.gameObject.GetComponent<TagManager>().tagType == TagManager.BaseTag.Enemy)
            {
                other.gameObject.GetComponent<EnemyData>().SetEnemyHealth(bulletDamage);
            }
        }
    }

    private IEnumerator DespawnBullet()
    {
        yield return new WaitForSeconds(bulletDistance);
        Destroy(gameObject);
    }
}
