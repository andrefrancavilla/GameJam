using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombProjectileScript : MonoBehaviour {

    public GameObject impactExplosion;
    public float explosionRadius;
    public LayerMask explosionInteraction;

    int rotDir = 0;
    float zRot;

    private void Awake()
    {
        while(rotDir == 0)
        {
            rotDir = Random.Range(-10, 10);
        }
    }

    private void Update()
    {
        zRot += Time.deltaTime * rotDir * 30;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, zRot));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != STRINGS.PLAYER)
        {
            Collider2D[] coll = Physics2D.OverlapCircleAll(transform.position, explosionRadius, explosionInteraction.value);
            foreach (Collider2D obj in coll)
            {
                Instantiate(impactExplosion, transform.position, transform.rotation);
            }
            Destroy(gameObject);
        }
    }
}
