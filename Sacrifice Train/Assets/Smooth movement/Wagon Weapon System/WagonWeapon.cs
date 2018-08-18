using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagonWeapon : MonoBehaviour {

    public WAGON_WEAPON weaponType;

    public float acidSprayerTickDamage;
    public float acidSprayerTickTime; //in seconds
    public float cooldownTimer;
    public Transform acidSprayerBarrel;
    public Transform aimer;
    bool acidSprayerFiring;
    PlayerController player;
    public bool IsInUse { get { return IsInUse; } set { IsInUse = value; } }

    float weaponFireT;

    public enum WAGON_WEAPON
    {
        COW_CATAPULT,
        ACID_SPRAYER
    }

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update ()
    {
		if(weaponType == WAGON_WEAPON.ACID_SPRAYER)
        {
            acidSprayerBarrel.right = (player.transform.position / 2) - acidSprayerBarrel.position;

            weaponFireT += Time.deltaTime;
            if(weaponFireT >= acidSprayerTickTime)
            {

                weaponFireT = 0;
            }
        }
	}

    float GetClosestAvailableWeapon(GameObject henchman)
    {
        float distance = Vector2.Distance(transform.position, henchman.transform.position);
        return distance;
    }
}
