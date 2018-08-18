using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PestilenceAI : MonoBehaviour {

    //Need to be managed
    [SerializeField]
    GameObject[] spawnPoints;
    float[] carriageHealth;
    float lowestCarriageHealth;
    int lowestCarriageHealthIndex=0;
    float playerHealth;
    [SerializeField]
    GameObject playerCharacter;
    [SerializeField]
    GameObject henchmenCharacter;
    [SerializeField]
    HenchmenAI henchmenAI;


    enum PestilenceState { Idle, AnimateDead, Beam, ToxicCloud }
    PestilenceState pState = PestilenceState.Idle;
    [SerializeField]
    float maxCooldownTime = 10.0f;
    float currentCooldown = 0.0f;    
    [SerializeField]
    int undeadAnimatedPerAction;
    [SerializeField]
    int maxHenchmen = 20;
    int currentHenchmenCount = 0;
   
	// Use this for initialization
	void Start () {
		
	}
    private void Awake()
    {
        carriageHealth = new float[] { 0 };
    }

    // Update is called once per frame
    void Update () {
        currentCooldown -= Time.deltaTime;
		//States are
        //Idle, Raise Dead, Fire beam, create toxic cloud
        if(currentCooldown <= 0.0f && pState == PestilenceState.Idle)
        {
            currentCooldown = maxCooldownTime;
            ChangeState();
            DoAction();
        }
	}
    void ChangeState()
    {
        pState = PestilenceState.AnimateDead;
        //pState = (PestilenceState)Random.Range(0, 5);
    }
    void DoAction()
    {
        switch(pState)
        {
            case PestilenceState.Idle:
                break;
            case PestilenceState.AnimateDead:
                StartCoroutine(ActionAnimateDead());
                break;
            case PestilenceState.Beam:
                break;
            case PestilenceState.ToxicCloud:
                break;
            default:
                break;
        }
    }
    IEnumerator ActionAnimateDead()
    {
        int currentSpawnPoint;
        FindMostDamagedCarriage();
        //Choose spawn point
            //If player health below 50%, spawn there
            //If carriage health below 50%, spawn there
            //else spawn near player
        if(lowestCarriageHealth<=50.0f && playerHealth >= 50.0f)
        {
            currentSpawnPoint = lowestCarriageHealthIndex;
        }
        else 
        {
            currentSpawnPoint = FindSpawnPointNearPlayer();
        }       
        //Spawn undead
        for(int i=0; i<undeadAnimatedPerAction; i++)
        {
            if (currentHenchmenCount >= maxHenchmen)
                break;
            henchmenAI.AddHenchmen(Instantiate(henchmenCharacter, spawnPoints[currentSpawnPoint].transform.position, spawnPoints[currentSpawnPoint].transform.rotation), currentSpawnPoint);
            currentHenchmenCount++;
            yield return new WaitForSeconds(1);
        }
        //Set state to Idle
        pState = PestilenceState.Idle;
        yield return null;
    }
   
    void ActionBeam()
    {
        //Locate player
        //Fire
        //Set state to Idle
    }
    void ActionToxicCloud()
    {
        //Choose location (maybe check if its free of clouds)
        //Spawn cloud
        //Set state to Idle
    }
    void FindMostDamagedCarriage()
    {
        float currentMinHealth = float.MaxValue;
        int currentMinIndex = 0;
        for (int i = 0; i < carriageHealth.Length; i++)
        {
            if (carriageHealth[i] < currentMinHealth)
            {
                currentMinHealth = carriageHealth[i];
                currentMinIndex = i;
            }
        }
        lowestCarriageHealth = currentMinHealth;
        lowestCarriageHealthIndex = currentMinIndex;
    }
    int FindSpawnPointNearPlayer()
    {
        float minDistance = float.MaxValue;
        int index = 0;
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            float currentDist = DistanceFromPlayer(spawnPoints[i].transform.position);
            if (currentDist < minDistance)
            {
                minDistance = currentDist;
                index = i;
            }
        }
        return index;
    }
    float DistanceFromPlayer(Vector3 pos)
    {
        return (playerCharacter.transform.position - pos).sqrMagnitude;
    }
    public void UpdateHenchmenCount(int count)
    {
        currentHenchmenCount += count;
    }
}
