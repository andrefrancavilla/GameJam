﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour {

    public float projectileSpeed;
    public float tBeforeDestruction;
    public float damage;
    public bool isExplosive;
    public GameObject explosion;
    public bool isEnemyProjectile;
    Rigidbody2D rb;


    // Use this for initialization
    private void Awake()
    {
        if(isEnemyProjectile)
            gameObject.layer = 0;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * projectileSpeed;
        Destroy(gameObject, tBeforeDestruction);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*
         * if(collision.gameObject.tag == STRINGS.ENEMY)
         *  collision.gameObject.GetComponent<EnemyAI>().Damage(damage);
        */
        if (!isExplosive)
        {
            if (!isEnemyProjectile)
            {
                if (collision.gameObject.tag == STRINGS.WAGON_WEAPON)
                    collision.gameObject.GetComponent<WagonWeapon>().weaponHP -= damage;
                if (collision.gameObject.tag == STRINGS.ENEMY)
                    collision.gameObject.GetComponent<HenchmanCharacter>().Damage(damage);
                Destroy(gameObject);

            }
            else
            {
                if(collision.gameObject.tag == STRINGS.PLAYER)
                    collision.gameObject.GetComponent<PlayerController>().DamagePlayer(damage);
                Destroy(gameObject);

            }
        }
        else
            Instantiate(explosion, transform.position, transform.rotation);
    }
}
