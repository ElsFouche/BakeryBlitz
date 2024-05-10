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
    public int bulletDurability = 5;

    void Start()
    {
        this.transform.localScale *= bulletArea;
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
                bulletDurability--;
                if (bulletDurability <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private IEnumerator DespawnBullet()
    {
        yield return new WaitForSeconds(bulletDistance);
        Destroy(gameObject);
    }
}
