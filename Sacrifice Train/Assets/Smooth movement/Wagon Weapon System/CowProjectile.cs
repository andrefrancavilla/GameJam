﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowProjectile : MonoBehaviour {

    public float damage;
    public float disappearTime;
    Animator anim;
    [SerializeField]
    Collider2D colliderToEnable;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(EnableCollider(0.3f));
    }
    IEnumerator EnableCollider(float time)
    {
        yield return new WaitForSeconds(time);
        colliderToEnable.enabled = true;
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
