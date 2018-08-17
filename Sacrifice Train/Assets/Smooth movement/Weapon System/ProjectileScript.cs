using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour {

    public float projectileSpeed;
    public float tBeforeDestruction;
    Rigidbody2D rb;

    // Use this for initialization
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.right * projectileSpeed;
        Destroy(gameObject, tBeforeDestruction);
    }
}
