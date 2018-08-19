using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveDamage : MonoBehaviour {

    public float explosionRange;
    public float damage;
    public bool hurtPlayer;

    private void Awake()
    {
        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, explosionRange);
        foreach(Collider2D col in collisions)
        {
            if (!hurtPlayer)
            {
                if (col.gameObject.tag == STRINGS.ENEMY)
                {
                    col.GetComponent<HenchmanCharacter>().Damage(damage);
                }
                if (col.gameObject.tag == STRINGS.WAGON_WEAPON)
                    col.gameObject.GetComponent<WagonWeapon>().weaponHP -= damage;
            }
            else
            {
                if (col.gameObject.tag == STRINGS.PLAYER)
                {
                    col.GetComponent<PlayerController>().DamagePlayer(damage);
                }
            }
        }
    }
}
