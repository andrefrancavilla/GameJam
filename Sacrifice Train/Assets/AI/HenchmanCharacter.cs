using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HenchmanCharacter : MonoBehaviour {
    [SerializeField]
    float maxHealth=30;
    float currentHealth;
    HenchmenAI.HenchmenState myState;
    HenchmenAI henchmenAI;
    [HideInInspector]
    public int wagonNo;
	// Use this for initialization
	void Start () {
		
	}
    private void Awake()
    {
        henchmenAI = GameObject.Find("HenchmenManager").GetComponent<HenchmenAI>();
    }

    // Update is called once per frame
    void Update () {
		
	}
    public void Damage(float damage)
    {
        currentHealth -= damage;
        if(currentHealth<0)
        {
            henchmenAI.DestroyHenchman(gameObject, wagonNo);
            Destroy(gameObject);
        }
    }
   
    public void ActionFireWeapon()
    {
        myState = HenchmenAI.HenchmenState.FireWeapon;
        //Start coroutine to move to weapon then fire it
    }
    public void ActionFireMachineGun()
    {
        myState = HenchmenAI.HenchmenState.FireMachineGuns;
        //Aim at player and shoot projectiles
    }
    public void ActionRepairWeapon()
    {
        myState = HenchmenAI.HenchmenState.RepairWeapon;
        //Start coroutine to move to weapon and repair it
    }
    public void ActionRepairWagon()
    {
        myState = HenchmenAI.HenchmenState.RepairWagon;
        //Start coroutine to repair wagon
    }

}
