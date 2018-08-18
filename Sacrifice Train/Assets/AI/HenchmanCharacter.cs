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
    Vector2 movementVector;
    Vector3 target;
    WagonWeapon targetWeapon;
    [SerializeField]
    float speed;
    [SerializeField]
    Rigidbody2D rigidbody;
    [SerializeField]
    PlayerController playerController;
	// Use this for initialization
	void Start () {
		
	}
    private void Awake()
    {
        henchmenAI = GameObject.Find("HenchmenManager").GetComponent<HenchmenAI>();
        movementVector = Vector2.zero;
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
   
    public void ActionFireWeapon(WagonWeapon weapon)
    {
        myState = HenchmenAI.HenchmenState.FireWeapon;
        targetWeapon = weapon;
        target = weapon.transform.position;
        movementVector.x = (target - transform.position).normalized.x;
        targetWeapon.IsInUse = true;

        //Start coroutine to move to weapon then fire it
        StartCoroutine(FireWeapon());
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
    IEnumerator FireWeapon()
    {
        while((transform.position - target).magnitude>0)
        {
            yield return null;
        }
        //Fire weapon
        movementVector = Vector2.zero;
        yield return null;
    }
    IEnumerator MoveCharacter()
    {
        while(true)
        {
            rigidbody.velocity = movementVector * speed;
            yield return null;
        }
    }

}
