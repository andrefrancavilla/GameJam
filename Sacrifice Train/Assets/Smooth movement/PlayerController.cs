﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    //Character movement
    public float maxHP = 100;
    public float HP;
    public float moveSpeed;
    public AnimationCurve accelerationBehaviourCurve;
    public float deltaTimeToDecellerate; //Lower the value, the more time it will take for the player to reach 0 velocity
    public float controlResponsiveness; //The lower the value, the less responsive the controls are. Hence, the movement is more floaty the more you lower the value.

    Vector2 inputAxis; //Input axis is stored here
    Rigidbody2D ShipRB;
    float tMoving;

    private void Awake()
    {
        ShipRB = GetComponent<Rigidbody2D>();
    }
    
    void Update ()
    {
        //Movement
        inputAxis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (inputAxis.magnitude != 0)
            tMoving += Time.deltaTime;
        else
        {
            tMoving = 0;
            ShipRB.velocity = Vector2.Lerp(ShipRB.velocity, Vector2.zero, deltaTimeToDecellerate);
        }

        ShipRB.velocity = Vector2.Lerp(ShipRB.velocity, new Vector2(inputAxis.x * moveSpeed, inputAxis.y * moveSpeed * accelerationBehaviourCurve.Evaluate(tMoving)), controlResponsiveness);
    }

    public void DamagePlayer(int damage)
    {
        HP -= damage;
    }
}
