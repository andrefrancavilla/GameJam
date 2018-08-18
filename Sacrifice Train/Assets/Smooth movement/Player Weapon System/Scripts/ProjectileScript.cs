using System.Collections;
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
         * if(collision.gameObject.tag == "Enemy")
         *  collision.gameObject.GetComponent<EnemyAI>().Damage(damage);
        */
        if (!isExplosive)
        {
            if (!isEnemyProjectile)
            {
                if (collision.gameObject.tag == "Wagon_Weapon")
                    collision.gameObject.GetComponent<WagonWeapon>().weaponHP -= damage;
            }
            else
            {
                if(collision.gameObject.tag == "Player")
                    collision.gameObject.GetComponent<PlayerController>().DamagePlayer(damage);
            }
        }
        else
            Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
