using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HenchmenAI : MonoBehaviour {
    List<List<GameObject>> henchmenList;
    enum HenchmenState { Idle, FireMachineGuns, FireSweepShot, SeekWeapon, FireWeapon, RepairWeapon, RepairWagon }
    float[] statePriorities;
    HenchmenState[] hState;
    float playerHealth, weaponHealth, trainHealth, accuracy;
    PestilenceAI pestilenceAI;
    [SerializeField]
    bool debugDestroyWagon = false;
    // Use this for initialization
    void Start () {
		
	}
    private void Awake()
    {
        henchmenList = new List<List<GameObject>>();
        pestilenceAI = GameObject.Find("Pestilence").GetComponent<PestilenceAI>();
    }

    // Update is called once per frame
    void Update () {
		if(debugDestroyWagon)
        {
            debugDestroyWagon = false;
            DestroyHenchmen(0);
        }
        //Change priorities
        //If player health low, increase offensive priorities
        //If weapons are damaged, increase priority of repairing weapons
        //If train is damaged, increase priority of repairing armour
        //While firing, if bullets have few hits, do a wide sweep attack
        //Based on final priority scores, choose an action

        //All actions are in a priority queue
        //Pop the first action, assign as many as possible to it
        //Pop the next action, do the same

        //Update all henchmen
        //
    }
    public void AddHenchmen(GameObject henchman, int wagonNo)
    {
        if(henchmenList.Count <= wagonNo)
        {
            henchmenList.Add(new List<GameObject>());
        }
        henchmenList[wagonNo].Add(henchman);

    }
    public void DestroyHenchmen(int wagonNo)
    {
        pestilenceAI.UpdateHenchmenCount(henchmenList[wagonNo].Count);
        //Death animation
        for(int i=0; i< henchmenList[wagonNo].Count; i++)
        {
            GameObject.Destroy(henchmenList[wagonNo][i]);
        }
    }
}
