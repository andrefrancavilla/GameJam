using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HenchmanCharacter : MonoBehaviour {
    [SerializeField]
    float maxHealth=30;
    [SerializeField]
    float currentHealth;
    [SerializeField]
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

    //Weapon stuff
    [SerializeField]
    float weaponSpread= 0.1f;
    [SerializeField]
    float fireRate = 0.1f;
    float currentFireTime = 0.0f;
    [SerializeField]
    float repairRate = 1.0f;
   
    [SerializeField]
    GameObject projectile;
    WagonManager currentWagon;

    GameObject aimDummy;

	// Use this for initialization
	void Start () {
        StartCoroutine(MoveCharacter());
	}
    private void Awake()
    {
        henchmenAI = GameObject.Find("HenchmenManager").GetComponent<HenchmenAI>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        movementVector = Vector2.zero;
        currentHealth = maxHealth;
        aimDummy = new GameObject();
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
        StartCoroutine(FireMachineGun());
        //Aim at player and shoot projectiles
    }
    public void ActionRepairWeapon(WagonWeapon weapon)
    {
        myState = HenchmenAI.HenchmenState.RepairWeapon;
        targetWeapon = weapon;
        target = weapon.transform.position;
        movementVector.x = (target - transform.position).normalized.x;

        //Start coroutine to move to weapon then fire it
        StartCoroutine(RepairWeapon());
        //Start coroutine to move to weapon and repair it
    }
    public void ActionRepairWagon(WagonManager wagon)
    {
        myState = HenchmenAI.HenchmenState.RepairWagon;
        currentWagon = wagon;

        wagon.InteractWithWagon(WAGON_INTERACTION.ATTACH_REPAIR_WORKER);
        StartCoroutine(RepairWagon());
        //Start coroutine to repair wagon
    }
    IEnumerator FireWeapon()
    {
        while((transform.position - target).magnitude>19.5)
        {
            Debug.Log((transform.position - target).magnitude);
            yield return null;
        }
        //Fire weapon
        movementVector = Vector2.zero;
        yield return null;
    }
    IEnumerator FireMachineGun()
    {
        while (myState == HenchmenAI.HenchmenState.FireMachineGuns)
        {
            currentFireTime -= Time.deltaTime;
            if (currentFireTime <= 0.0f)
            {
                currentFireTime = fireRate;
                Vector3 diff = playerController.transform.position - transform.position;
                diff.Normalize();
                aimDummy.transform.right = diff;
                Instantiate(projectile, transform.position + diff*3, aimDummy.transform.rotation);
            }
            yield return null;
        }
    }
    IEnumerator RepairWeapon()
    {
        while ((transform.position - target).magnitude > 1)
        {
            //Debug.Log((transform.position - target).magnitude);
            yield return null;
        }
        //Repair weapon
        movementVector = Vector2.zero;
        while(targetWeapon.weaponHP<=50 && myState == HenchmenAI.HenchmenState.RepairWeapon)
        {
            targetWeapon.weaponHP += Time.deltaTime * repairRate;
            yield return null;
        }
        if (myState == HenchmenAI.HenchmenState.RepairWeapon)
            myState = HenchmenAI.HenchmenState.Idle;
        yield return null;
    }
    IEnumerator RepairWagon()
    {
        movementVector = Vector2.zero;
        while (currentWagon.health <= 1000 && myState==HenchmenAI.HenchmenState.RepairWagon)
        {            
            yield return null;
        }
        if(myState==HenchmenAI.HenchmenState.RepairWagon)
        myState = HenchmenAI.HenchmenState.Idle;
        currentWagon.InteractWithWagon(WAGON_INTERACTION.DETACH_REPAIR_WORKER);
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
