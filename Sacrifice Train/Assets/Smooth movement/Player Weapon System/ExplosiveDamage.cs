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
                if (col.gameObject.tag == "Enemy")
                {
                    col.GetComponent<HenchmanCharacter>().Damage(damage);
                }
                if (col.gameObject.tag == "Wagon_Weapon")
                    col.gameObject.GetComponent<WagonWeapon>().weaponHP -= damage;
            }
            else
            {
                if (col.gameObject.tag == "Player")
                {
                    col.GetComponent<PlayerController>().DamagePlayer(damage);
                }
            }
        }
    }
}
