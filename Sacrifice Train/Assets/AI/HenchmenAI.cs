using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HenchmenAI : MonoBehaviour {
    List<List<GameObject>> henchmenListByWagon;
    List<HenchmanCharacter> henchmenList;

    
    public enum HenchmenState { FireWeapon, FireMachineGuns, RepairWeapon, RepairWagon, Idle}
    float[] statePriorities;
    List<HenchmenState> hState;
    List<bool> assignedHenchmen;
    float playerHealth, weaponHealth, trainHealth, accuracy;
    int shotsFired, shotsHit;
    PestilenceAI pestilenceAI;
    [SerializeField]
    bool debugDestroyWagon = false;
    [SerializeField]
    float actionCooldown = 10.0f;
    float currentCooldown = 0.0f;
    [SerializeField]
    List<WagonManager> wagonManagerList;
    List<WagonRoofManager> wagonRoofList;
    [SerializeField]
    PlayerController playerController;

    
    //Need a wagon list

    // Use this for initialization
    void Start () {
		
	}
    private void Awake()
    {
        henchmenListByWagon = new List<List<GameObject>>();
        henchmenList = new List<HenchmanCharacter>();
        pestilenceAI = GameObject.Find("Pestilence").GetComponent<PestilenceAI>();
        statePriorities = new float[5];
        hState = new List<HenchmenState>();
        assignedHenchmen = new List<bool>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        wagonRoofList = new List<WagonRoofManager>();
        for(int i=0; i<wagonManagerList.Count; i++)
        {
            wagonRoofList.Add(wagonManagerList[i].GetComponentInChildren<WagonRoofManager>());
        }
    }

    // Update is called once per frame
    void Update () {
		if(debugDestroyWagon)
        {
            debugDestroyWagon = false;
            DestroyHenchmenByWagon(0);
        }
        currentCooldown -= Time.deltaTime;
        //Run all this based on an interval, not every frame
        if (currentCooldown <= 0.0f)
        {
            UpdateHenchmenActions();
        }
    }

    private void UpdateHenchmenActions()
    {
        currentCooldown = actionCooldown;
        UpdatePriorities();
        int henchmanIndex = 0;
        assignedHenchmen.Clear();
        //hState.Clear();
        for (int i = 0; i < henchmenList.Count; i++)
        {
            assignedHenchmen.Add(false);
            if (hState.Count < henchmenList.Count)
            {
                hState.Add(HenchmenState.Idle);
            }
        }
        //Assign tasks
        for (int i = 0; i < statePriorities.Length; i++)
        {
            HenchmenState task = (HenchmenState)GetHighestPriorityTask();
            for (int j = 0; j < henchmenList.Count; j++)
            {

                if (assignedHenchmen[j])
                {
                    continue;
                }
                //If already at task to be assigned, skip
                if (hState[j] == task)
                {
                    assignedHenchmen[j] = true;
                    continue;
                }
                //assign task
                switch (task)
                {
                    case HenchmenState.FireWeapon:
                        WagonWeapon weapon = wagonManagerList[henchmenList[j].wagonNo].GetClosestAvailableWeapon(henchmenList[j].transform);
                        if (weapon == null)
                        {
                            continue;
                        }
                        henchmenList[j].ActionFireWeapon(weapon);
                        break;
                    case HenchmenState.FireMachineGuns:
                        if(wagonRoofList[henchmenList[j].wagonNo])
                        {
                            continue;
                        }
                        henchmenList[j].ActionFireMachineGun();
                        break;
                    case HenchmenState.RepairWeapon:
                        WagonWeapon weaponToRepair = wagonManagerList[henchmenList[j].wagonNo].GetClosestAvailableWeapon(henchmenList[j].transform);
                        if (weaponToRepair == null)
                        {
                            continue;
                        }
                        henchmenList[j].ActionRepairWeapon(weaponToRepair);
                        break;
                    case HenchmenState.RepairWagon:
                        henchmenList[j].ActionRepairWagon(wagonManagerList[henchmenList[j].wagonNo]);
                        break;
                }
                hState[j] = task;
                //For weapon, check if weapon is free
                //For repair, check if wagon health <50
                //For weapon repair, check weapon health
                assignedHenchmen[j] = true;
            }
        }
    }

    void UpdatePriorities()
    {
        //Change priorities
        //If player health low, increase offensive priorities
        //If weapons are damaged, increase priority of repairing weapons
        //If train is damaged, increase priority of repairing armour
        //While firing, if bullets have few hits, do a wide sweep attack
        //Based on final priority scores, choose an action
        playerHealth = playerController.HP;
        weaponHealth = GetWeaponHealth();
        trainHealth = GetTrainHealth();
        for (int i = 0; i < statePriorities.Length; i++)
        {
            statePriorities[i] = 0;
        }
        if(playerHealth<=50)
        {
            statePriorities[(int)HenchmenState.FireMachineGuns]++;
            statePriorities[(int)HenchmenState.FireWeapon]++;
        }
        if(weaponHealth<20)
        {
            statePriorities[(int)HenchmenState.RepairWeapon]++;
        }
        if(trainHealth<500)
        {
            statePriorities[(int)HenchmenState.RepairWagon]++;
        }
    }
    float GetWeaponHealth()
    {
        float wHealth=0.0f;
        for(int i=0; i<wagonManagerList.Count; i++)
        {
            for(int j=0; j<wagonManagerList[i].wagonWeapons.Count; j++)
            {
                wHealth += wagonManagerList[i].wagonWeapons[j].weaponHP;
            }
        }
        return wHealth;
    }
    float GetTrainHealth()
    {
        float tHealth = 0.0f;
        for(int i=0; i< wagonManagerList.Count; i++)
        {
            tHealth += wagonManagerList[i].Health;
        }

        return tHealth;
    }
    int GetHighestPriorityTask()
    {
        int currentIndex = 0;
        float maxPriority = -1;
        for(int i=0; i<statePriorities.Length; i++)
        {
            if(statePriorities[i]>maxPriority)
            {
                currentIndex = i;
                maxPriority = statePriorities[i];
            }
        }
        statePriorities[currentIndex] = -1;
        return currentIndex;
    }
    public void AddHenchmen(GameObject henchman, int wagonNo)
    {
        while(henchmenListByWagon.Count <= wagonNo)
        {
            henchmenListByWagon.Add(new List<GameObject>());
        }
        henchmenListByWagon[wagonNo].Add(henchman);
        henchmenList.Add(henchman.GetComponent<HenchmanCharacter>());
       
    }
    public void DestroyHenchmenByWagon(int wagonNo)
    {
        pestilenceAI.UpdateHenchmenCount(henchmenListByWagon[wagonNo].Count *-1);
        //Death animation
        for(int i=0; i< henchmenListByWagon[wagonNo].Count; i++)
        {
            henchmenList.Remove(henchmenListByWagon[wagonNo][i].GetComponent<HenchmanCharacter>());
            GameObject.Destroy(henchmenListByWagon[wagonNo][i]);
        }
        henchmenListByWagon[wagonNo].Clear();
        
    }
    public void DestroyHenchman(GameObject henchman, int wagonNo)
    {
        pestilenceAI.UpdateHenchmenCount(-1);
        henchmenList.Remove(henchman.GetComponent<HenchmanCharacter>());
        henchmenListByWagon[wagonNo].Remove(henchman);
    }
}
