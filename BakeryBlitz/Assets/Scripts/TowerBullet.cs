using System.Collections;
using UnityEngine;

/* Author:  Fouche', Els
 * Updated: 04/09/2024
 * Notes:   This script handles bullet behavior. It accepts
 *          various parameters when it is instantiated by the
 *          TowerShoot script. 
 */
public class TowerBullet : MonoBehaviour
{
    public float bulletSpeed;
    public float bulletDistance;
    public float bulletArea;
    public int bulletDamage;
    public int bulletDurability = 5;

    /// <summary>
    /// Modifies the size of the bullet based on the bulletArea value to
    /// create larger or smaller projectiles. 
    /// </summary>
    void Start()
    {
        this.transform.localScale *= bulletArea;
        StartCoroutine("DespawnBullet");
    }

    /// <summary>
    /// Simple movement that utilizes our forward vector at time of instantiation
    /// to determine where we should move. Notably does NOT 'home in' on enemies. 
    /// </summary>
    void Update()
    {
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;
    }

    /// <summary>
    /// If we impact an enemy, deal our damage to the enemy and decrease our durability.
    /// If our durability is at or below 0, we are destroyed. 
    /// </summary>
    /// <param name="other"></param>
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

    /// <summary>
    /// Destroys the bullet if we've been alive too long. 
    /// </summary>
    /// <returns></returns>
    private IEnumerator DespawnBullet()
    {
        yield return new WaitForSeconds(bulletDistance);
        Destroy(gameObject);
    }
}
