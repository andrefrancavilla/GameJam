using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagonWeapon : MonoBehaviour {

    public WAGON_WEAPON weaponType;

    public float acidSprayerTickDamage;
    public float acidSprayerTickTime; //in seconds
    public Transform acidSprayerBarrel;
    public Transform aimer;
    bool acidSprayerFiring;
    PlayerController player;
    

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
            /*Vector3 dir = player.transform.position - acidSprayerBarrel.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            acidSprayerBarrel.rotation = Quaternion.AngleAxis(angle, Vector3.forward);*/
            acidSprayerBarrel.right = (player.transform.position / 2) - acidSprayerBarrel.position;

            if (acidSprayerFiring)
            {
                //
            }
        }
	}
}
