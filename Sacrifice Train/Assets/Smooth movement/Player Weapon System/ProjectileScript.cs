using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour {

    public float projectileSpeed;
    public float tBeforeDestruction;
    public float damage;
    Rigidbody2D rb;


    // Use this for initialization
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.right * projectileSpeed;
        Destroy(gameObject, tBeforeDestruction);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*
         * if(collision.gameObject.tag == "Enemy")
         *  collision.gameObject.GetComponent<EnemyAI>().Damage(damage);
        */
        if (collision.gameObject.tag == "Wagon_Weapon")
            collision.gameObject.GetComponent<WagonWeapon>().weaponHP -= damage;
        Destroy(gameObject);
    }
}
