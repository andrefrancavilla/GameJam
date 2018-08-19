using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PestilenceAI : MonoBehaviour {

    //Need to be managed
    [SerializeField]
    GameObject[] spawnPoints;
    [SerializeField]
    WagonManager[] wagonManagerList;
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
    [SerializeField]
    GameObject beam;
    [SerializeField]
    GameObject toxicCloud;
   
	// Use this for initialization
	void Start () {
		
	}
    private void Awake()
    {
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
        //pState = PestilenceState.ToxicCloud;
        pState = (PestilenceState)Random.Range(0, 5);
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
                StartCoroutine(ActionFireBeam());
                break;
            case PestilenceState.ToxicCloud:
                ActionToxicCloud();
                break;
            default:
                break;
        }
    }
    IEnumerator ActionAnimateDead()
    {
        int wagonNumber;
        FindMostDamagedCarriage();
        //Choose spawn point
            //If player health below 50%, spawn there
            //If carriage health below 50%, spawn there
            //else spawn near player
        if(lowestCarriageHealth<=50.0f && playerHealth >= 50.0f)
        {
            wagonNumber = lowestCarriageHealthIndex;
        }
        else 
        {
            wagonNumber = FindSpawnPointNearPlayer();
        }       
        //Spawn undead
        for(int i=0; i<undeadAnimatedPerAction; i++)
        {
            if (currentHenchmenCount >= maxHenchmen)
                break;
            GameObject henchman = Instantiate(henchmenCharacter, spawnPoints[wagonNumber].transform.position, spawnPoints[wagonNumber].transform.rotation);
            henchman.GetComponent<HenchmanCharacter>().wagonNo = wagonNumber;
            henchmenAI.AddHenchmen(henchman, wagonNumber);
            currentHenchmenCount++;
            yield return new WaitForSeconds(1);
        }
        //Set state to Idle
        pState = PestilenceState.Idle;
        yield return null;
    }
   
   IEnumerator ActionFireBeam()
    {
        //Locate player
        Vector3 target = playerCharacter.transform.position;
        float multiplier = Random.Range(-1f, 1f);
        if (multiplier <= 0)
        {
            multiplier = -1;
        }
        else
        {
            multiplier = 1;
        }
        Vector3 startPoint = target + new Vector3(multiplier * 10, 0);
        GameObject beamInstance = Instantiate(beam, transform.position, Quaternion.identity);
        startPoint.z = 0;
        beamInstance.transform.right = (startPoint - transform.position);
        Vector3 beamStartPoint = (startPoint - transform.position);
        float beamDuration = 4.0f;
        float beamTimer = 0f;
        while(beamTimer<beamDuration)
        {
            beamTimer += Time.deltaTime;
            Vector3 endPoint = playerCharacter.transform.position;
            beamStartPoint += (endPoint - beamStartPoint).normalized * 0.05f;
            beamInstance.transform.right = beamStartPoint;
            yield return null;
        }
        Destroy(beamInstance);
        //Fire
        //Set state to Idle
        pState = PestilenceState.Idle;
        yield return null;
    }
    void ActionToxicCloud()
    {
        int wagonNo = Random.Range(0, wagonManagerList.Length);
        Instantiate(toxicCloud, wagonManagerList[wagonNo].transform.position + new Vector3(0, Random.Range(0f,40f)), Quaternion.identity);
        //Choose location (maybe check if its free of clouds)
        //Spawn cloud
        //Set state to Idle
        pState = PestilenceState.Idle;
    }
    float GetTrainHealth()
    {
        float tHealth = 0.0f;
        for (int i = 0; i < wagonManagerList.Length; i++)
        {
            tHealth += wagonManagerList[i].Health;
        }
        return tHealth;
    }
    void FindMostDamagedCarriage()
    {
        float currentMinHealth = float.MaxValue;
        int currentMinIndex = 0;
        for (int i = 0; i < wagonManagerList.Length; i++)
        {
            if (wagonManagerList[i].Health < currentMinHealth)
            {
                currentMinHealth = wagonManagerList[i].Health;
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
