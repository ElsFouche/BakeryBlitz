using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
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

    private IEnumerator ShootDelay()
    {
        waitingToShoot = true;
        yield return new WaitForSeconds(fireDelay);
        waitingToShoot = false;
    }

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