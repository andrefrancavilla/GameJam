using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    [SerializeField]
    [Range(0,1000)]
    float Speed = 0.1f;
    Vector2 Movement;
    Rigidbody2D ShipRB;
    [SerializeField]
    Weapon Weapon1;
    float Weapon1Delay;
    float Weapon1Timer=0.0f;
    [SerializeField]
    float MaxFuel = 120f;
    float CurrentFuel;
   
	// Use this for initialization
	void Start () {
		
	}
    private void Awake()
    {
        ShipRB = GetComponent<Rigidbody2D>();
        Weapon1Delay = Weapon1.Delay;
        CurrentFuel = MaxFuel;
    }

    // Update is called once per frame
    void Update () {
        Weapon1Timer -= Time.deltaTime;
        CurrentFuel -= Time.deltaTime;
        //Inputs
        Movement = Vector2.zero;
        if(Input.GetAxis("Vertical")!=0)
        {
            Movement += new Vector2(0, Input.GetAxis("Vertical"));
        }
        if(Input.GetAxis("Horizontal")!=0)
        {
            Movement += new Vector2(Input.GetAxis("Horizontal"), 0);
        }
        if(Input.GetButton("Fire1") && Weapon1Timer<=0.0f)
        {
            Weapon1Timer = Weapon1Delay;
            Weapon1.FireWeapon();
        }

        ShipRB.velocity = (Movement * Speed);



    }
}
