using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour {

    public WeaponList currentPrimaryWeapon;
    public WeaponList currentSecondaryWeapon;
    public GameObject[] weaponProjectiles; //hard-coded indexes of each weapon. See weapon autoconfig for projectile index positions.
    public Transform[] projectileSpawnPositions; //Same as above applies here.
    //Shooting system
    public float primaryWeaponFireRate; //bullets per second
    public float secondaryWeaponFireRate;
    float primaryFireT;
    public float secondaryFireT;
    int primaryWeaponIndex;
    int secondaryWeaponIndex;
    GameObject primaryProjectile;
    GameObject secondaryProjectile;

    //Weapon equipment "sensor" system, to configure weapons when equipped
    WeaponList previousPrimaryWeapon;
    WeaponList previousSecondaryWeapon;

    public enum WeaponList
    {
        None = 0,
        Railgun = 1,
        Straight_Line_Missile = 2,
        Bombs = 3
    }
    
    void Update ()
    {
        //Weapon auto-configuration
        if(previousPrimaryWeapon != currentPrimaryWeapon)
        {
            switch(currentPrimaryWeapon)
            {
                case WeaponList.Railgun:
                    primaryWeaponIndex = 0;
                    primaryWeaponFireRate = 10;
                    break;
                case WeaponList.Straight_Line_Missile:
                    primaryWeaponIndex = 1;
                    primaryWeaponFireRate = 3;
                    break;
                case WeaponList.Bombs:
                    primaryWeaponIndex = 2;
                    primaryWeaponFireRate = 2;
                    break;
            }
            primaryProjectile = weaponProjectiles[primaryWeaponIndex];
            previousPrimaryWeapon = currentPrimaryWeapon;
        }
        if(previousSecondaryWeapon != currentSecondaryWeapon)
        {
            switch (currentSecondaryWeapon)
            {
                case WeaponList.Railgun:
                    secondaryWeaponIndex = 0;
                    secondaryWeaponFireRate = 10;
                    break;
                case WeaponList.Straight_Line_Missile:
                    secondaryWeaponIndex = 1;
                    secondaryWeaponFireRate = 3;
                    break;
                case WeaponList.Bombs:
                    secondaryWeaponIndex = 2;
                    secondaryWeaponFireRate = 2;
                    break;
            }
            secondaryProjectile = weaponProjectiles[secondaryWeaponIndex];
            previousSecondaryWeapon = currentSecondaryWeapon;
        }

        //Fire handle
        if (Input.GetButton("Fire1"))
            primaryFireT += Time.deltaTime * primaryWeaponFireRate;
        else
            primaryFireT = 0;

        if (Input.GetButton("Fire2"))
            secondaryFireT += Time.deltaTime * secondaryWeaponFireRate;
        else
            secondaryFireT = 0;

        //Projectile handle
        if(primaryFireT >= 1)
        {
            Instantiate(primaryProjectile, projectileSpawnPositions[primaryWeaponIndex].position, projectileSpawnPositions[primaryWeaponIndex].rotation);
            primaryFireT = 0;
        }
        if(secondaryFireT >= 1)
        {
            Instantiate(secondaryProjectile, projectileSpawnPositions[secondaryWeaponIndex].position, projectileSpawnPositions[secondaryWeaponIndex].rotation);
            secondaryFireT = 0;
        }
    }
}
