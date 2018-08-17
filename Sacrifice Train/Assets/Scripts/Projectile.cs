using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    [SerializeField]
    Rigidbody2D ProjectileRB;
    [SerializeField]
    Collider2D ProjectileCollider;
    [SerializeField]
    bool HasGravity;
    [SerializeField]
    Vector2 Movement;
    [SerializeField]
    float Speed;
    [SerializeField]
    GameObject Explosion;
    [SerializeField]
    float Lifetime = 1.0f;
    [SerializeField]
    float Damage = 1.0f;
    [SerializeField]
    bool Pierce = false;

	// Use this for initialization
	void Start () {
        GameObject.Destroy(gameObject, Lifetime);
	}
    private void Awake()
    {
    }

    // Update is called once per frame
    void Update () {
        ProjectileRB.velocity = Movement*Speed;
	}
}
