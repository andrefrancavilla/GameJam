using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WagonWeapon : MonoBehaviour {

    public WAGON_WEAPON weaponType;
    public float weaponHP;
    float previousWeaponHP;
    float initWeaponHP;

    public int acidSprayerTickDamage;
    public float acidSprayerTickTime; //in seconds
    public float cooldownTimer;
    public int numberOfTicks;
    public Transform acidSprayerBarrel;
    public Transform aimer;
    public AcidSpray acidSensing;
    bool acidSprayerFiring;
    PlayerController player;
    public bool IsInUse { get; set; }

    public RuntimeAnimatorController catapultAnimator;

    public GameObject cow;
    public Transform cowSpawn;
    public GameObject healthBar;
    public Transform barPosition;
    public float tHpBarIsVisible;

    float weaponFireT;
    float cooldownTimerTMem;
    float tHpBarIsVisibleMem;
    int tickCount;
    GameObject hpBar;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        cooldownTimerTMem = cooldownTimer;
        previousWeaponHP = weaponHP;
        tHpBarIsVisibleMem = tHpBarIsVisible;
        //tHpBarIsVisible = 0;
        initWeaponHP = weaponHP;

        if (weaponType == WAGON_WEAPON.COW_CATAPULT)
        {
            if(!GetComponent<Animator>())
            {
                gameObject.AddComponent<Animator>();
                GetComponent<Animator>().runtimeAnimatorController = catapultAnimator;
            }
        }
        if (healthBar)
        {
            hpBar = Instantiate(healthBar, barPosition.position, barPosition.rotation);
            hpBar.transform.parent = FindObjectOfType<HeadsUpDisplay>().transform;
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if(IsInUse)
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
                    GetComponent<Animator>().SetTrigger(STRINGS.TRIGGER_LAUNCH);
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
            if (hpBar)
                Destroy(hpBar);
        }

        //UI stuff
        if(hpBar)
        {
            hpBar.transform.position = barPosition.position;
            if (previousWeaponHP != weaponHP)
            {
                hpBar.SetActive(true);
                hpBar.transform.GetChild(0).GetComponent<Image>().fillAmount = weaponHP / initWeaponHP;
                tHpBarIsVisible = tHpBarIsVisibleMem;
                previousWeaponHP = weaponHP;
            }

            if (tHpBarIsVisible > 0)
                tHpBarIsVisible -= Time.deltaTime;
            else
                hpBar.SetActive(false);
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
