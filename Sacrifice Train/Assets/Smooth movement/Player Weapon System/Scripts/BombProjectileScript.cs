using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombProjectileScript : MonoBehaviour {

    public GameObject impactExplosion;
    public float explosionRadius;
    public LayerMask explosionInteraction;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != STRINGS.PLAYER)
        {
            Collider2D[] coll = Physics2D.OverlapCircleAll(transform.position, explosionRadius, explosionInteraction.value);
            foreach (Collider2D obj in coll)
            {
                Debug.Log("Explosion impacted on " + obj.gameObject.name + ".");
            }
            //Instantiate(impactExplosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
