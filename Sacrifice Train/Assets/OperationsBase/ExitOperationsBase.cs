using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitOperationsBase : MonoBehaviour
{
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

            transitionFromBase.SetTrigger(STRINGS.TRIGGER_FADE_FROM_OPERATIONS_BASE);
            yield return new WaitForSeconds(animDuration);

            operationsBase.SetActive(false);

            Debug.Log(weaponScript.CanFire);
            weaponScript.ToggleFire();
            Debug.Log(weaponScript.CanFire);
        }
    }
}
