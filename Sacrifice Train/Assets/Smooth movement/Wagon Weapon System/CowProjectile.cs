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
            anim.SetTrigger(STRINGS.TRIGGER_DEATH);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == STRINGS.PLAYER)
        {
            FindObjectOfType<PlayerController>().DamagePlayer(damage);
            anim.SetTrigger(STRINGS.TRIGGER_DEATH);
        }
    }

    public void CowDeath()
    {
        Destroy(gameObject);
    }
}
