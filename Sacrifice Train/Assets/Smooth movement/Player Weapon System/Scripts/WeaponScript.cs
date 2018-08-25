using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour {

    public WEAPON_TYPE currentLeftWeapon;
    public WEAPON_TYPE currentRightWeapon;
    public GameObject[] weaponProjectiles; //hard-coded indexes of each weapon. See weapon autoconfig for projectile index positions.
    public Transform[] projectileSpawnPositions; //Same as above applies here.
    //Shooting system
    public float primaryWeaponFireRate; //bullets per second
    public float secondaryWeaponFireRate;
    float primaryWeaponBulletSpread;
    float secondaryWeaponBulletSpread;
    float primaryFireT;
    float secondaryFireT;
    int primaryWeaponIndex;
    int secondaryWeaponIndex;
    GameObject primaryProjectile;
    GameObject secondaryProjectile;
    float leftShotCooldown;
    float rightShotCooldown;
    float leftShotMem;
    float rightShotMem;
    int primaryLeftProjectilesAmount;
    public int primaryRightProjectilesAmount;
    int leftShot;
    public int rightShot;

    //Weapon equipment "sensor" system, to configure weapons when equipped
    WEAPON_TYPE previousPrimaryWeapon;
    WEAPON_TYPE previousSecondaryWeapon;

    bool canFire = true;

    public bool CanFire { get; private set; } = true;

    void Update ()
    {
        //Weapon auto-configuration
        if(previousPrimaryWeapon != currentLeftWeapon)
        {
            switch(currentLeftWeapon)
            {
                case WEAPON_TYPE.RAILGUN:
                    primaryWeaponIndex = 0;
                    primaryWeaponFireRate = 9;
                    primaryWeaponBulletSpread = 0.5f;
                    leftShotCooldown = 0.5f;
                    primaryLeftProjectilesAmount = 6;
                    break;
                case WEAPON_TYPE.MISSILE:
                    primaryWeaponIndex = 1;
                    primaryWeaponFireRate = 3;
                    primaryWeaponBulletSpread = 0.33f;
                    leftShotCooldown = 1.25f;
                    primaryLeftProjectilesAmount = 3;
                    break;
                case WEAPON_TYPE.BOMBS:
                    primaryWeaponIndex = 2;
                    primaryWeaponFireRate = 2;
                    primaryWeaponBulletSpread = 0;
                    leftShotCooldown = 2f;
                    primaryLeftProjectilesAmount = 3;
                    break;
            }
            primaryProjectile = weaponProjectiles[primaryWeaponIndex];
            previousPrimaryWeapon = currentLeftWeapon;
            leftShotMem = leftShotCooldown;
        }
        if(previousSecondaryWeapon != currentRightWeapon)
        {
            switch (currentRightWeapon)
            {
                case WEAPON_TYPE.RAILGUN:
                    secondaryWeaponIndex = 0;
                    secondaryWeaponFireRate = 6;
                    secondaryWeaponBulletSpread = 0.5f;
                    rightShotCooldown = 0.5f;
                    primaryRightProjectilesAmount = 6;
                    break;
                case WEAPON_TYPE.MISSILE:
                    secondaryWeaponIndex = 1;
                    secondaryWeaponFireRate = 3;
                    secondaryWeaponBulletSpread = 0.33f;
                    rightShotCooldown = 1.25f;
                    primaryRightProjectilesAmount = 3;
                    break;
                case WEAPON_TYPE.BOMBS:
                    secondaryWeaponIndex = 2;
                    secondaryWeaponFireRate = 2;
                    secondaryWeaponBulletSpread = 0;
                    rightShotCooldown = 2f;
                    primaryRightProjectilesAmount = 3;
                    break;
            }
            secondaryProjectile = weaponProjectiles[secondaryWeaponIndex];
            previousSecondaryWeapon = currentRightWeapon;
            rightShotMem = rightShotCooldown;
        }

        //Fire handle
        if(primaryFireT  < 1)
        primaryFireT += Time.deltaTime * primaryWeaponFireRate;
        if(secondaryFireT < 1)
        secondaryFireT += Time.deltaTime * secondaryWeaponFireRate;

        //Projectile handle
        if (leftShotCooldown > 0)
            leftShotCooldown -= Time.deltaTime;
        if (rightShotCooldown > 0)
            rightShotCooldown -= Time.deltaTime;
        if (CanFire)
        {
            if (Input.GetButton("Fire1") && primaryFireT >= 1 && leftShotCooldown <= 0)
            {
                if (leftShot < primaryLeftProjectilesAmount)
                {
                    var temp = Instantiate(primaryProjectile, projectileSpawnPositions[primaryWeaponIndex].position + Vector3.up * Random.Range(-primaryWeaponBulletSpread / 2, primaryWeaponBulletSpread / 2), projectileSpawnPositions[primaryWeaponIndex].rotation);
                    SetProjectileProperties(temp.transform, currentLeftWeapon);
                    primaryFireT = 0;
                    leftShot++;
                }
                else
                {
                    leftShot = 0;
                    leftShotCooldown = leftShotMem;
                }
            }
            if (Input.GetButton("Fire2") && secondaryFireT >= 1 && rightShotCooldown <= 0)
            {
                if(rightShot < primaryRightProjectilesAmount)
                {
                    var temp = Instantiate(secondaryProjectile, projectileSpawnPositions[secondaryWeaponIndex].position + Vector3.up * Random.Range(-secondaryWeaponBulletSpread / 4, secondaryWeaponBulletSpread / 4), projectileSpawnPositions[secondaryWeaponIndex].rotation);
                    SetProjectileProperties(temp.transform, currentRightWeapon);
                    secondaryFireT = 0;
                    rightShot++;
                }
                else
                {
                    rightShot = 0;
                    rightShotCooldown = rightShotMem;
                }
            }
        }
    }

    void SetProjectileProperties(Transform trans, WEAPON_TYPE weaponType)
    {
        float scaleToUse = 0;
        switch (weaponType)
        {
            case WEAPON_TYPE.RAILGUN:
                scaleToUse = 0.85f;
                break;
            case WEAPON_TYPE.MISSILE:
                scaleToUse = 1.0f;
                break;
            case WEAPON_TYPE.BOMBS:
                scaleToUse = 0.7f;
                break;
            default:
                break;
        }
        trans.localScale = new Vector3(scaleToUse, scaleToUse, scaleToUse);
    }

    public bool SetLeftWeapon(WEAPON_TYPE weapon)
    {
        if (weapon == currentRightWeapon)
            return false;
        currentLeftWeapon = weapon;
        return true;
    }
    public bool SetRightWeapon(WEAPON_TYPE weapon)
    {
        if (weapon == currentLeftWeapon)
            return false;
        currentRightWeapon = weapon;
        return true;
    }

    public void EnableFire()
    {
        CanFire = true;
    }
    public void DisableFire()
    {
        CanFire = false;
    }
}
