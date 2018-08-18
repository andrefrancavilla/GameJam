using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour {

    public WeaponType currentLeftWeapon;
    public WeaponType currentRightWeapon;
    public GameObject[] weaponProjectiles; //hard-coded indexes of each weapon. See weapon autoconfig for projectile index positions.
    public Transform[] projectileSpawnPositions; //Same as above applies here.
    //Shooting system
    float primaryWeaponFireRate; //bullets per second
    float secondaryWeaponFireRate;
    float primaryFireT;
    float secondaryFireT;
    int primaryWeaponIndex;
    int secondaryWeaponIndex;
    GameObject primaryProjectile;
    GameObject secondaryProjectile;

    //Weapon equipment "sensor" system, to configure weapons when equipped
    WeaponType previousPrimaryWeapon;
    WeaponType previousSecondaryWeapon;

    public enum WeaponType
    {
        None = 0,
        Railgun = 1,
        Straight_Line_Missile = 2,
        Bombs = 3
    }
    
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
                    break;
                case WeaponType.Straight_Line_Missile:
                    primaryWeaponIndex = 1;
                    primaryWeaponFireRate = 3;
                    break;
                case WeaponType.Bombs:
                    primaryWeaponIndex = 2;
                    primaryWeaponFireRate = 2;
                    break;
            }
            primaryProjectile = weaponProjectiles[primaryWeaponIndex];
            previousPrimaryWeapon = currentLeftWeapon;
        }
        if(previousSecondaryWeapon != currentRightWeapon)
        {
            switch (currentRightWeapon)
            {
                case WeaponType.Railgun:
                    secondaryWeaponIndex = 0;
                    secondaryWeaponFireRate = 6;
                    break;
                case WeaponType.Straight_Line_Missile:
                    secondaryWeaponIndex = 1;
                    secondaryWeaponFireRate = 3;
                    break;
                case WeaponType.Bombs:
                    secondaryWeaponIndex = 2;
                    secondaryWeaponFireRate = 2;
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
        if (Input.GetButton("Fire1") && primaryFireT >= 1)
        {
            Instantiate(primaryProjectile, projectileSpawnPositions[primaryWeaponIndex].position, projectileSpawnPositions[primaryWeaponIndex].rotation);
            primaryFireT = 0;
        }
        if (Input.GetButton("Fire2") && secondaryFireT >= 1)
        {
            Instantiate(secondaryProjectile, projectileSpawnPositions[secondaryWeaponIndex].position, projectileSpawnPositions[secondaryWeaponIndex].rotation);
            secondaryFireT = 0;
        }
    }
}
