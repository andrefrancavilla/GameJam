using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagonHealthManager : MonoBehaviour
{
    public List<GameObject> henchmen;

    #region CONSTANTS
    const int MAX_HEALTH = 1000;
    const int MIN_HEALTH = 0;

    const int SECONDS_PER_TICK = 5;
    const int HEALTH_PER_TICK = 10;

    // example dmg values
    const int RAILGUN_DMG = 3;
    const int MISSILE_DMG = 30;
    const int BOMB_DMG = 300;
    const int SNOWBALL_DMG = 3;
    #endregion

    [HideInInspector]
    public int Health { get; private set; }

    [HideInInspector]
    public int RepairWorkers { get; private set; }

    void Start()
    {
        Health = MAX_HEALTH;
        RepairWorkers = 0;
    }

    void Update()
    {
        if (RepairWorkers > 0 && Time.time % SECONDS_PER_TICK == 0)
            AddHealth();
    }

    void AddHealth()
    {
        int sumAddedHealth = RepairWorkers * HEALTH_PER_TICK;

        if (Health + (sumAddedHealth) > MAX_HEALTH)
            Health = MAX_HEALTH;
        else
            Health += sumAddedHealth;
    }

    void TakeDmg(int dmg)
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
        for (int i = 0; i < henchmen.Count; i++)
        {
            //henchmen[i].GetComponent<AIScript>().DisableHenchman(DISABLE_TYPE.PERMADEATH);
        }

        var wagonPrisonerManager = GetComponent<WagonPrisonerManager>();
        if (wagonPrisonerManager != null) wagonPrisonerManager.DisablePrisoners();

        enabled = false;
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

    public void OnCollisionEnter(Collision c)
    {
        switch (c.gameObject.tag)
        {
            case "Railgun":
                TakeDmg(RAILGUN_DMG);
                break;
            case "Missile":
                TakeDmg(MISSILE_DMG);
                break;
            case "Bomb":
                TakeDmg(BOMB_DMG);
                break;
            case "Snowball":
                TakeDmg(SNOWBALL_DMG);
                break;
            default:
                if (c.gameObject.tag != null && c.gameObject.tag.Length > 0)
                    Debug.Log("Tag is incorrectly spelled!");
                break;
        }
    }
}