using UnityEngine;

public class WagonRoofManager : MonoBehaviour
{
    public SpriteRenderer sprRen;   // the sprite renderer of the roof object

    #region CONSTANTS
    const float MAX_HEALTH = 1000.0f;
    const float MIN_HEALTH = 0.0f;

    const float SECONDS_PER_TICK = 5.0f;
    const float HEALTH_PER_TICK = 10.0f;
    #endregion

    #region Fading data
    bool isFading = false;
    const float FADE_SECONDS = 3.0f;
    #endregion

    
    public float Health = MAX_HEALTH;

    [HideInInspector]
    public int RepairWorkers { get; private set; } = 0;

    void Update()
    {
        if (RepairWorkers > 0 && Time.time % SECONDS_PER_TICK == 0)
            AddHealth();
        if (isFading)
            FadeRoofOut();
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
            isFading = true;
        }
        else
            Health -= dmg;
    }

    void FadeRoofOut()
    {
        sprRen.color = new Color(
                sprRen.color.r,
                sprRen.color.g,
                sprRen.color.b,
                sprRen.color.a - (Time.deltaTime / FADE_SECONDS));

        if (sprRen.color.a <= 0.0f)
            Destroy(gameObject);
    }

    public void OnCollisionEnter2D(Collision2D c)
    {
        if(!isFading)
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
                        Debug.Log("Tag is incorrectly spelled!" + c.gameObject.tag);
                    break;
            }
        }
    }

    public void InteractWithWagonRoof(WAGON_INTERACTION interaction)
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
