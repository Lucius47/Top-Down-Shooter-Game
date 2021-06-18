using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform shell;
    public Transform shellEjection;
    public Projectile projectile;
    public Transform muzzle;
    MuzzleFlash muzzleFlash;


    public float timeBetweenShots = 100;
    public float muzzleVelocity = 35;
    float nextShotTime;

    void Start()
    {
        muzzleFlash = GetComponent<MuzzleFlash>();
        
    }
    public void Shoot ()
    {
        if (Time.time > nextShotTime)
        {
            nextShotTime = Time.time + timeBetweenShots / 1000; //to convert ms to seconds
            Projectile newProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation) as Projectile;
            newProjectile.SetSpeed(muzzleVelocity);

            Instantiate(shell, shellEjection.position, shellEjection.rotation);

            muzzleFlash.Activate();
        }
        
    }

}
