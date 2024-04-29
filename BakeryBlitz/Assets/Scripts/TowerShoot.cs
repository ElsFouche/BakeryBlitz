using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class TowerShoot : MonoBehaviour
{
    private float fireDelay;
    private bool waitingToShoot = false;
    private GameObject bullet;
    private GameObject target;
    private TowerController controller;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            controller = gameObject.transform.parent.GetComponent<TowerController>();
        }
        catch
        {
            Debug.Log("Unable to access Tower Controller script in parent. \n" +
                      "is this script attached to a child?");
        }

        if (controller)
        {
            fireDelay = controller.fireRate;
            bullet = controller.bullet;
        }
    }

    // Update is called once per frame
    void Update()
    {
        target = controller.FindTarget();

        if (target)
        {
            transform.LookAt(target.transform.position);
        }

        if (target && !waitingToShoot)
        {
            StartCoroutine("ShootDelay");
            ShootBullet();
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
        Debug.Log("Firing!");
    }
}