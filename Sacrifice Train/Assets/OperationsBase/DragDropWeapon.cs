using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDropWeapon : MonoBehaviour
{
    public WeaponScript weaponScript;

    Transform draggedWeapon;
    bool isDragging = false;

	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
            TryToDrag();
        if (isDragging && Input.GetMouseButtonUp(0))
            ReleaseDragged();
        if (isDragging)
            FollowMouse();
    }

    void TryToDrag()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider?.tag == "Weapon")
        {
            draggedWeapon = hit.collider.transform;
            isDragging = true;
        }
    }

    void ReleaseDragged()
    {
        isDragging = false;
        RaycastHit2D hit = Physics2D.Raycast(
            Camera.main.ScreenToWorldPoint(Input.mousePosition), 
            Vector2.zero,
            Mathf.Infinity, // distance
            LayerMask.NameToLayer("WeaponSlots"));
        
        

        if(hit.collider?.tag == "PrimaryWeapon")
        {
            switch (hit.collider.name)
            {
                case "Railgun":
                    if (!weaponScript.SetLeftWeapon(WeaponType.Railgun))
                        OnScreenDuplicateWeaponError();
                    break;
                case "Missile":
                    if (!weaponScript.SetLeftWeapon(WeaponType.Straight_Line_Missile))
                        OnScreenDuplicateWeaponError();
                    break;
                case "Bomb":
                    if (!weaponScript.SetLeftWeapon(WeaponType.Bombs))
                        OnScreenDuplicateWeaponError();
                    break;
            }
        }

        if (hit.collider?.tag == "SecondaryWeapon")
        {
            switch (hit.collider.name)
            {
                case "Railgun":
                    if (!weaponScript.SetRightWeapon(WeaponType.Railgun))
                        OnScreenDuplicateWeaponError();
                    break;
                case "Missile":
                    if (!weaponScript.SetRightWeapon(WeaponType.Straight_Line_Missile))
                        OnScreenDuplicateWeaponError();
                    break;
                case "Bomb":
                    if (!weaponScript.SetRightWeapon(WeaponType.Bombs))
                        OnScreenDuplicateWeaponError();
                    break;
            }
        }
    }

    void FollowMouse()
    {

    }

    void OnScreenDuplicateWeaponError()
    {

    }
}
