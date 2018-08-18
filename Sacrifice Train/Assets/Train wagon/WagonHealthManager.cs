using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagonHealthManager : MonoBehaviour
{
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
    public int health = MAX_HEALTH;

    [HideInInspector]
    public int repairWorkers = 0;

    void Update()
    {
        if (repairWorkers > 0 && Time.time % SECONDS_PER_TICK == 0)
            AddHealth(repairWorkers);
    }

    void AddHealth(int numOfRepairWorkers)
    {
        int sumAddedHealth = numOfRepairWorkers * HEALTH_PER_TICK;

        if (health + (sumAddedHealth) > MAX_HEALTH)
            health = MAX_HEALTH;
        else
            health += sumAddedHealth;
    }

    void TakeDmg(int dmg)
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
        // code here
    }

    public void InteractWithWagon(WAGON_INTERACTION interaction)
    {
        switch (interaction)
        {
            case WAGON_INTERACTION.ATTACH_REPAIR_WORKER:
                repairWorkers++;
                break;
            case WAGON_INTERACTION.DETACH_REPAIR_WORKER:
                repairWorkers--;
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