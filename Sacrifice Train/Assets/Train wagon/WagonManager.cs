using System.Collections.Generic;
using UnityEngine;

public class WagonManager : MonoBehaviour
{
    public HenchmenAI henchmenAI;
    
    [Range(0, 10)]
    public int wagonNumber; // set in inspector for marking purposes
    public List<WagonWeapon> wagonWeapons;  // weapons on the wagon roof

    #region CONSTANTS
    const float MAX_HEALTH = 1000;
    const float MIN_HEALTH = 0;

    const float SECONDS_PER_TICK = 5;
    const float HEALTH_PER_TICK = 10;
    #endregion

    public float health  = MAX_HEALTH;

    [HideInInspector]
    public int RepairWorkers { get; private set; } = 0;

    void Update()
    {
        if (RepairWorkers > 0 && Time.time % SECONDS_PER_TICK == 0)
            AddHealth();
    }

    void AddHealth()
    {
        float sumAddedHealth = RepairWorkers * HEALTH_PER_TICK;

        if (health + (sumAddedHealth) > MAX_HEALTH)
            health = MAX_HEALTH;
        else
            health += sumAddedHealth;
    }

    void TakeDmg(float dmg)
    {
        if (health - dmg < MIN_HEALTH)
        {
            health = MIN_HEALTH;
            DisableWagon();
        }
        else
            health -= dmg;
    }

    void DisableWagon()
    {
        henchmenAI.DestroyHenchmenByWagon(wagonNumber);

        var wagonPrisonerManager = GetComponent<WagonPrisonerManager>();
        if (wagonPrisonerManager != null) wagonPrisonerManager.DisablePrisoners();

        enabled = false;
    }

    public void OnCollisionEnter2D(Collision2D c)
    {
        switch (c.gameObject.tag)
        {
            case STRINGS.TAG_RAILGUN:
                TakeDmg(WEAPON_DMG.RAILGUN_DMG);
                break;
            case STRINGS.TAG_MISSILE:
                TakeDmg(WEAPON_DMG.MISSILE_DMG);
                break;
            case STRINGS.TAG_BOMB:
                TakeDmg(WEAPON_DMG.BOMB_DMG);
                break;
            case STRINGS.TAG_SNOWBALL:
                TakeDmg(WEAPON_DMG.SNOWBALL_DMG);
                break;
        }
    }

    public void InteractWithWagon(WAGON_INTERACTION interaction)
    {
        switch (interaction)
        {
            case WAGON_INTERACTION.ATTACH_REPAIR_WORKER:
                RepairWorkers++;
                break;
            case WAGON_INTERACTION.DETACH_REPAIR_WORKER:
                RepairWorkers--;
                break;
            default:
                break;
        }
    }

    public WagonWeapon GetClosestAvailableWeapon(Transform henchman)
    {
        float currentClosestDistance = 0.0f;
        WagonWeapon currentClosestHenchman = null;
        for (int i = 0; i < wagonWeapons.Count; i++)
        {
            if (!wagonWeapons[i].IsInUse)
            {
                var distance = Vector2.Distance(henchman.position, wagonWeapons[i].transform.position);
                if (currentClosestDistance < distance)
                {
                    currentClosestDistance = distance;
                    currentClosestHenchman = wagonWeapons[i];
                }
            }
        }
        return currentClosestHenchman;
    }
}