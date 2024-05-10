using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Author: Fouche', Els
 * Updated: 04/25/2024
 * Notes:   This script handles the shooting behavior of the towers.
 *          It is tightly coupled to TowerController. It handles 
 *          instantiation of bullets based on parameters set in the
 *          TowerController which are set there for editor-friendliness. 
 */

public class TowerShoot : MonoBehaviour
{
    public Transform shootFromThis;
    private float bulletSpeed;
    private float bulletDistance;
    private float bulletArea;
    private int bulletDamage;
    private float fireDelay;
    private float leadTarget;
    private int bulletDurability;
    private bool waitingToShoot = false;
    private GameObject bullet;
    private GameObject target;
    private TowerController controller;
    private CustomLookAt targeting;

    /// <summary>
    /// Initializes all the values we need for bullet creation. 
    /// </summary>
    void Start()
    {
        controller = gameObject.GetComponentInParent<TowerController>();

        if (controller)
        {
            fireDelay = 10.1f - controller.fireRate;
            bullet = controller.bullet;
            bulletSpeed = controller.bulletSpeed;
            bulletDistance = controller.bulletDistance;
            bulletDamage = controller.bulletDamage;
            bulletArea = controller.bulletArea;
            leadTarget = controller.leadTarget;
            bulletDurability = controller.bulletDurability;
        } else
        {
            Debug.Log("Unable to access Tower Controller script in parent. \n" +
                      "is this script attached to a child?");
        }

        targeting = GetComponent<CustomLookAt>();
        targeting.SetTarget();
    }

    /// <summary>
    /// Finds our target using the TowerController's FindTarget function and
    /// updates our facing direction towards that target based on an offset
    /// in the target's forward vector as determined by our leadTarget variable.
    /// Initializes our delay before firing again if we weren't already waiting
    /// to fire and fires a bullet. 
    /// </summary>
    void Update()
    {
        target = controller.FindTarget();

        if (target)
        {
            Vector3 targetPos = target.transform.position;
            targetPos += target.transform.forward * leadTarget;
            targeting.SetTarget(targetPos);
            if (!waitingToShoot)
            {
                StartCoroutine("ShootDelay");
                ShootBullet();
            }
        }
    }

    /// <summary>
    /// Basic wait function that is probably built into unity somewhere already?
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShootDelay()
    {
        waitingToShoot = true;
        yield return new WaitForSeconds(fireDelay);
        waitingToShoot = false;
    }

    /// <summary>
    /// Creates a bullet at the location of our 'barrel' (shootFromThis empty game object),
    /// gets the bullet script attached to the new bullet and initializes its variables. 
    /// </summary>
    private void ShootBullet()
    {
       // Debug.Log("Firing!");
        GameObject bulletFired = Instantiate(bullet, shootFromThis.position, transform.rotation);
        TowerBullet tempBullet = bulletFired.GetComponent<TowerBullet>();
        tempBullet.bulletDistance = bulletDistance;
        tempBullet.bulletSpeed = bulletSpeed;
        tempBullet.bulletDamage = bulletDamage;
        tempBullet.bulletArea = bulletArea;
        tempBullet.bulletDurability = bulletDurability;
    }
}