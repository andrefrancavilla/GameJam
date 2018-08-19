using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitOperationsBase : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject heavenSky;
    public GameObject operationsBase;
    public WeaponScript weaponScript;
    public Animator transitionFromBase;
    public Button btn;

    public ReleasePrisoners releasePrisoners;

    public List<DragDropWeapon> dragDropWeapons;

    public float animDuration = 1.0f;

    void Start()
    {
        btn.onClick.AddListener(() => StartCoroutine(ReturnToBattle())); 
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            StartCoroutine(ReturnToBattle());
	}

    IEnumerator ReturnToBattle()
    {
        if(releasePrisoners.DestroyPrisoners())
        {
            for (int i = 0; i < dragDropWeapons.Count; i++)
            {
                if(dragDropWeapons[i].IsChosen || dragDropWeapons[i].IsDragging)
                {
                    dragDropWeapons[i].DeactivateDragging();
                    dragDropWeapons[i].ReturnToOrigin();
                    dragDropWeapons[i].ResetDragging();
                }
            }

            transitionFromBase.SetTrigger(STRINGS.TRIGGER_FADE_TO_WHITE);
            heavenSky.SetActive(false);
            operationsBase.SetActive(false);
            yield return new WaitForSeconds(animDuration); 

            transitionFromBase.SetTrigger(STRINGS.TRIGGER_FADE_OUT_OF_WHITE);
            yield return new WaitForSeconds(animDuration);

            weaponScript.EnableFire();
            playerController.ToggleInTheClouds();
        }
    }
}
