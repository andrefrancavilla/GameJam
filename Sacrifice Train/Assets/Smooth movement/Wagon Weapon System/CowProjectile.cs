using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowProjectile : MonoBehaviour {

    public float damage;
    public float disappearTime;
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (disappearTime > 0)
            disappearTime -= Time.deltaTime;
        else
            anim.SetTrigger("Death");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            FindObjectOfType<PlayerController>().DamagePlayer(damage);
            anim.SetTrigger("Death");
        }
    }

    public void CowDeath()
    {
        Destroy(gameObject);
    }
}
