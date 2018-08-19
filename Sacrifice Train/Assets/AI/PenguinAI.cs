using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinAI : MonoBehaviour {

    public WEAPON_TYPE currentWeapon;
    public GameObject weaponProjectile; //hard-coded indexes of each weapon. See weapon autoconfig for projectile index positions.
    public Transform projectileSpawnPosition; //Same as above applies here.
    //Shooting system
    public float primaryWeaponFireRate; //bullets per second
    bool canFire = false;
    float primaryWeaponBulletSpread;
   
    float primaryFireT;
   
    int primaryWeaponIndex;
    //List<HenchmanCharacter> henchmenList;
    HenchmanCharacter target;
    GameObject dummyTarget;
    [SerializeField]
    bool debugToggleTargeting = false;
   
  
    // Use this for initialization
    void Start () {
        primaryWeaponIndex = 0;
        primaryWeaponFireRate = 0.2f;
        primaryWeaponBulletSpread = 0.5f;
        StartCoroutine(FireSnowballs());
        dummyTarget = new GameObject();
        dummyTarget.transform.parent = transform;
    }
    private void Awake()
    {
       // henchmenList = new List<HenchmanCharacter>();
    }
    private void Update()
    {
        if(debugToggleTargeting)
        {
            TogglesFiringSnowballs();
            debugToggleTargeting = false;
        }
        primaryFireT -= Time.deltaTime;
        if(Input.GetButtonDown("AutoFire"))
        {
            TogglesFiringSnowballs();
        }
    }
    public void TogglesFiringSnowballs()
    {
        canFire = !canFire;
        if(canFire)
        FindHenchmenTarget();
    }
    IEnumerator FireSnowballs()
    {

        while(true)
        {
            if (canFire && primaryFireT<=0f)
            {
                primaryFireT = primaryWeaponFireRate;
                if(!target)
                {
                    canFire = false;
                    continue;
                }
                Vector3 diff = target.transform.position - transform.position;
                diff.Normalize();
                dummyTarget.transform.right = diff;
                Instantiate(weaponProjectile, projectileSpawnPosition.position + Vector3.up * Random.Range(-primaryWeaponBulletSpread / 2, primaryWeaponBulletSpread / 2), dummyTarget.transform.rotation);
            }
            yield return null;
        }
    }
    void FindHenchmenTarget()
    {
        HenchmanCharacter[] henchmenList = FindObjectsOfType<HenchmanCharacter>();
        float minDist = float.MaxValue;
        int index = -1;
        for(int i=0; i< henchmenList.Length; i++)
        {
            float dist = (henchmenList[i].transform.position - transform.position).sqrMagnitude;
            if (dist<minDist)
            {
                minDist = dist;
                index = i;
            }
        }
        target = henchmenList[index];
    }
}
