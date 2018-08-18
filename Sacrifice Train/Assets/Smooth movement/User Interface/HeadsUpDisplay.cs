using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadsUpDisplay : MonoBehaviour {

    PlayerController player;
    public Image hpBar;

	// Use this for initialization
	void Start ()
    {
        if (GameObject.FindWithTag("Player"))
            player = FindObjectOfType<PlayerController>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(player)
        {
            hpBar.fillAmount = Mathf.Lerp(hpBar.fillAmount, player.HP / player.maxHP, 0.075f);
        }
	}
}
