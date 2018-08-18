using System.Collections.Generic;
using UnityEngine;

public class WagonHealthManager : MonoBehaviour
{
    //public Brain brain;
    
    [Range(0, 10)]
    public int wagonNumber; // set in inspector for marking purposes

    #region CONSTANTS
    const float MAX_HEALTH = 1000;
    const float MIN_HEALTH = 0;

    const float SECONDS_PER_TICK = 5;
    const float HEALTH_PER_TICK = 10;
    #endregion

    [HideInInspector]
    public float Health { get; private set; } = MAX_HEALTH;

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

        if (Health + (sumAddedHealth) > MAX_HEALTH)
            Health = MAX_HEALTH;
        else
            Health += sumAddedHealth;
    }

    void TakeDmg(float dmg)
    {
        if (Health - dmg < MIN_HEALTH)
        {
            Health = MIN_HEALTH;
            DisableWagon();
        }
        else
            Health -= dmg;
    }

    void DisableWagon()
    {
        //brain.DisableAllHenchmen(wagonNumber);

        var wagonPrisonerManager = GetComponent<WagonPrisonerManager>();
        if (wagonPrisonerManager != null) wagonPrisonerManager.DisablePrisoners();

        enabled = false;
    }

    public void OnCollisionEnter2D(Collision2D c)
    {
        switch (c.gameObject.tag)
        {
            case "Railgun":
                TakeDmg(WEAPON_DMG.RAILGUN_DMG);
                break;
            case "Missile":
                TakeDmg(WEAPON_DMG.MISSILE_DMG);
                break;
            case "Bomb":
                TakeDmg(WEAPON_DMG.BOMB_DMG);
                break;
            case "Snowball":
                TakeDmg(WEAPON_DMG.SNOWBALL_DMG);
                break;
            default:
                if (c.gameObject.tag != null && c.gameObject.tag.Length > 0)
                    Debug.Log("Tag is incorrectly spelled!");
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
}