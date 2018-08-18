using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HenchmenAI : MonoBehaviour {
    List<List<GameObject>> henchmenListByWagon;
    List<HenchmanCharacter> henchmenList;

    
    public enum HenchmenState { FireWeapon, FireMachineGuns, RepairWeapon, RepairWagon}
    float[] statePriorities;
    List<HenchmenState> hState;
    float playerHealth, weaponHealth, trainHealth, accuracy;
    int shotsFired, shotsHit;
    PestilenceAI pestilenceAI;
    [SerializeField]
    bool debugDestroyWagon = false;
    
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
    }

    // Update is called once per frame
    void Update () {
		if(debugDestroyWagon)
        {
            debugDestroyWagon = false;
            DestroyHenchmenByWagon(0);
        }
        //Run all this based on an interval, not every frame
        UpdatePriorities();        
        int henchmanIndex = 0;
        List<bool> assignedHenchmen = new List<bool>();
        //Assign tasks
        for(int i=0; i<statePriorities.Length; i++)
        {
            HenchmenState task = (HenchmenState)GetHighestPriorityTask();
            for(int j=0; j<henchmenList.Count; j++)
            {
                if(assignedHenchmen[j])
                {
                    continue;
                }
                //If already at task to be assigned, skip
                if(hState[j] == task)
                {
                    assignedHenchmen[j] = true;
                    continue;
                }
                //assign task
                switch(task)
                {
                    case HenchmenState.FireWeapon:
                        henchmenList[j].ActionFireWeapon();
                        break;
                    case HenchmenState.FireMachineGuns:
                        henchmenList[j].ActionFireMachineGun();
                        break;
                    case HenchmenState.RepairWeapon:
                        henchmenList[j].ActionRepairWeapon();
                        break;
                    case HenchmenState.RepairWagon:
                        henchmenList[j].ActionRepairWagon();
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
        for (int i = 0; i < statePriorities.Length; i++)
        {
            statePriorities[i] = 0;
        }
        if(playerHealth<=50)
        {
            statePriorities[(int)HenchmenState.FireMachineGuns]++;
            statePriorities[(int)HenchmenState.FireWeapon]++;
        }
        if(weaponHealth<50)
        {
            statePriorities[(int)HenchmenState.RepairWeapon]++;
        }
        if(trainHealth<50)
        {
            statePriorities[(int)HenchmenState.RepairWagon]++;
        }
    }
    int GetHighestPriorityTask()
    {
        int currentIndex = 0;
        float maxPriority = 0;
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
        if(henchmenListByWagon.Count <= wagonNo)
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
