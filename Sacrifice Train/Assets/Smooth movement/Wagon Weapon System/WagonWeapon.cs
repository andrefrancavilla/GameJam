using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagonWeapon : MonoBehaviour {

    public WAGON_WEAPON weaponType;
    public float weaponHP;

    public int acidSprayerTickDamage;
    public float acidSprayerTickTime; //in seconds
    public float cooldownTimer;
    public int numberOfTicks;
    public Transform acidSprayerBarrel;
    public Transform aimer;
    public AcidSpray acidSensing;
    bool acidSprayerFiring;
    PlayerController player;
    private bool isInUse;
    public bool IsInUse { get { return isInUse; } set { isInUse = value; } }

    public RuntimeAnimatorController catapultAnimator;

    public GameObject cow;
    public Transform cowSpawn;

    float weaponFireT;
    float cooldownTimerTMem;
    int tickCount;

    public enum WAGON_WEAPON
    {
        COW_CATAPULT,
        ACID_SPRAYER
    }

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        cooldownTimerTMem = cooldownTimer;

        if(weaponType == WAGON_WEAPON.COW_CATAPULT)
        {
            if(!GetComponent<Animator>())
            {
                gameObject.AddComponent<Animator>();
                GetComponent<Animator>().runtimeAnimatorController = catapultAnimator;
            }
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if(!isInUse)
        {
		    if(weaponType == WAGON_WEAPON.ACID_SPRAYER)
            {
                acidSprayerBarrel.right = Vector3.Lerp(acidSprayerBarrel.right, (player.transform.position / 2) - acidSprayerBarrel.position, 0.05f);

                weaponFireT += Time.deltaTime;
                if (cooldownTimer <= 0)
                {
                    if (weaponFireT >= acidSprayerTickTime)
                    {
                        if (tickCount < numberOfTicks)
                        {
                            if (acidSensing.playerInArea)
                            {
                                player.DamagePlayer(acidSprayerTickDamage);
                            }
                            tickCount++;
                            weaponFireT = 0;
                        }
                        else
                            cooldownTimer = cooldownTimerTMem;
                        weaponFireT = 0;
                    }
                }
                else
                {
                    tickCount = 0;
                    cooldownTimer -= Time.deltaTime;
                }
            }

            if(weaponType == WAGON_WEAPON.COW_CATAPULT)
            {
                if (cooldownTimer <= 0)
                {
                    GetComponent<Animator>().SetTrigger("Launch");
                    cooldownTimer = cooldownTimerTMem;
                }
                else
                {
                    tickCount = 0;
                    cooldownTimer -= Time.deltaTime;
                }
            }
        }

        if(weaponHP <= 0)
        {
            Destroy(gameObject);
        }
	}

    public void Damage(float damage)
    {
        weaponHP -= damage;
    }

    public void LaunchCow()
    {
        GameObject clone = Instantiate(cow, cowSpawn.position, cowSpawn.rotation);
        clone.GetComponent<Rigidbody2D>().velocity = player.transform.position + (Vector3.up * 4) - transform.position;
    }
}
