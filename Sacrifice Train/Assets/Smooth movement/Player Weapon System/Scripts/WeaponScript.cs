using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour {

    public WeaponType currentLeftWeapon;
    public WeaponType currentRightWeapon;
    public GameObject[] weaponProjectiles; //hard-coded indexes of each weapon. See weapon autoconfig for projectile index positions.
    public Transform[] projectileSpawnPositions; //Same as above applies here.
    //Shooting system
    public float primaryWeaponFireRate; //bullets per second
    public float secondaryWeaponFireRate;
    float primaryWeaponBulletSpread;
    float secondaryWeaponBulletSpread;
    float primaryFireT;
    public float secondaryFireT;
    int primaryWeaponIndex;
    int secondaryWeaponIndex;
    GameObject primaryProjectile;
    GameObject secondaryProjectile;

    //Weapon equipment "sensor" system, to configure weapons when equipped
    WeaponType previousPrimaryWeapon;
    WeaponType previousSecondaryWeapon;

    bool canFire = true;
    
    void Update ()
    {
        //Weapon auto-configuration
        if(previousPrimaryWeapon != currentLeftWeapon)
        {
            switch(currentLeftWeapon)
            {
                case WeaponType.Railgun:
                    primaryWeaponIndex = 0;
                    primaryWeaponFireRate = 6;
                    primaryWeaponBulletSpread = 0.5f;
                    break;
                case WeaponType.Straight_Line_Missile:
                    primaryWeaponIndex = 1;
                    primaryWeaponFireRate = 3;
                    primaryWeaponBulletSpread = 0.33f;
                    break;
                case WeaponType.Bombs:
                    primaryWeaponIndex = 2;
                    primaryWeaponFireRate = 2;
                    primaryWeaponBulletSpread = 0;
                    break;
            }
            //primaryProjectile = weaponProjectiles[primaryWeaponIndex];
            previousPrimaryWeapon = currentLeftWeapon;
        }
        if(previousSecondaryWeapon != currentRightWeapon)
        {
            switch (currentRightWeapon)
            {
                case WeaponType.Railgun:
                    secondaryWeaponIndex = 0;
                    secondaryWeaponFireRate = 6;
                    secondaryWeaponBulletSpread = 0.5f;
                    break;
                case WeaponType.Straight_Line_Missile:
                    secondaryWeaponIndex = 1;
                    secondaryWeaponFireRate = 3;
                    secondaryWeaponBulletSpread = 0.33f;
                    break;
                case WeaponType.Bombs:
                    secondaryWeaponIndex = 2;
                    secondaryWeaponFireRate = 2;
                    secondaryWeaponBulletSpread = 0;
                    break;
            }
            secondaryProjectile = weaponProjectiles[secondaryWeaponIndex];
            previousSecondaryWeapon = currentRightWeapon;
        }

        //Fire handle
        if(primaryFireT  < 1)
        primaryFireT += Time.deltaTime * primaryWeaponFireRate;
        if(secondaryFireT < 1)
        secondaryFireT += Time.deltaTime * secondaryWeaponFireRate;

        //Projectile handle
        if(canFire)
        {
            if (Input.GetButton("Fire1") && primaryFireT >= 1)
            {
                Instantiate(primaryProjectile, projectileSpawnPositions[primaryWeaponIndex].position + Vector3.up * Random.Range(-primaryWeaponBulletSpread / 2, primaryWeaponBulletSpread / 2), projectileSpawnPositions[primaryWeaponIndex].rotation);
                primaryFireT = 0;
            }
            if (Input.GetButton("Fire2") && secondaryFireT >= 1)
            {
                Instantiate(secondaryProjectile, projectileSpawnPositions[secondaryWeaponIndex].position + Vector3.up * Random.Range(-secondaryWeaponBulletSpread / 2, secondaryWeaponBulletSpread / 2), projectileSpawnPositions[secondaryWeaponIndex].rotation);
                secondaryFireT = 0;
            }
        }
    }

    public bool SetLeftWeapon(WeaponType weapon)
    {
        if (weapon == currentRightWeapon)
            return false;
        currentLeftWeapon = weapon;
        return true;
    }
    public bool SetRightWeapon(WeaponType weapon)
    {
        if (weapon == currentLeftWeapon)
            return false;
        currentRightWeapon = weapon;
        return true;
    }

    public void ToggleFire()
    {
        canFire = !canFire;
    }
}
