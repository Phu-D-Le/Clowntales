using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public bool isFiring;

    public GameObject bullet;
    public float bulletSpeed;
    public float bulletDespawnTimer;

    public float firingDelay;
    private float shotTimer;

    public Transform gunBarrel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isFiring)
        {
            shotTimer -= Time.deltaTime;
            if(shotTimer <= 0)
            {
                shotTimer = firingDelay;
                var newBullet = (GameObject)Instantiate(bullet, gunBarrel.position, gunBarrel.rotation);
                newBullet.GetComponent<BulletController>().speed = bulletSpeed;
                Destroy(newBullet, bulletDespawnTimer);
            }
        }

        else
        {
            shotTimer = 0;
        }
    }
}
