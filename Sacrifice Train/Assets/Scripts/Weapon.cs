using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    //audio
    //particle

    public float Delay;
    [SerializeField]
    GameObject[] Projectile;
    [SerializeField]
    [Range(0,0.7f)]
    float Inaccuracy = 0.0f;
    
    [SerializeField]
    Rigidbody2D WeaponRB;
    bool IsOffset = false;
    [SerializeField]
    float CurrentInaccuracy=0.0f;
    [SerializeField]
    [Range(0,0.5f)]
    float InaccuracyRate = 0.1f;
	// Use this for initialization
	void Start () {
		
	}
    private void Awake()
    {
    }

    // Update is called once per frame
    void Update () {
		if(CurrentInaccuracy>0)
        {
            CurrentInaccuracy -= InaccuracyRate / 10;
            if(CurrentInaccuracy < 0)
            {
                CurrentInaccuracy = 0;
            }
        }
	}
    public void FireWeapon()
    {
        foreach (GameObject projectile in Projectile)
        {
            float random = Random.Range(CurrentInaccuracy*-1, CurrentInaccuracy);
            
           Instantiate(projectile, transform.position + new Vector3(0,random), transform.rotation);
        }
        CurrentInaccuracy += InaccuracyRate;
        if(CurrentInaccuracy>Inaccuracy)
        {
            CurrentInaccuracy = Inaccuracy;
        }
        
    }
    
}
