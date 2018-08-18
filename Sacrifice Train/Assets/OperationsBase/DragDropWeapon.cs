using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDropWeapon : MonoBehaviour
{
    public WeaponScript weaponScript;

    Transform draggedWeapon;
    Rigidbody2D draggedWeaponRB;
    bool isDragging = false;

    Vector3 weaponStartPosition;
    const float SPEED_MODIFIER = 600.0f;

    public Transform leftWeaponSlot;
    public Transform rightWeaponSlot;

    void Start()
    {
        weaponScript.ToggleFire();   
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) TryToDrag();
        if (isDragging && Input.GetMouseButtonUp(0)) ReleaseDragged();
        if (isDragging) FollowMouse();
    }

    void TryToDrag()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            Camera.main.ScreenToWorldPoint(Input.mousePosition), 
            Vector2.zero);

        if (hit.collider?.tag == "Weapon")
        {
            draggedWeapon = hit.collider.transform;
            draggedWeaponRB = draggedWeapon.GetComponent<Rigidbody2D>();
            weaponStartPosition = draggedWeapon.position;
            isDragging = true;
        }
    }

    void FollowMouse()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var diff = (mousePos - draggedWeapon.position) * SPEED_MODIFIER * Time.deltaTime;

        draggedWeaponRB.velocity = new Vector2(diff.x, diff.y);
    }

    void ReleaseDragged()
    {
        isDragging = false;
        var hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        RaycastHit2D hit = new RaycastHit2D(); int len = hits.Length;
        for (int i = 0; i < len; i++)
        {
            if (hits[i].collider.tag == "WeaponSlot")
            {
                hit = hits[i];
                break;
            }
        }
        
        if (hit.collider != null)
        {
            Debug.Log("code is called");
            if (hit.collider.name == "LeftWeapon")
            {
                switch (draggedWeapon.name)
                {
                    case "Railgun":
                        TryApplyingWeapon(WeaponType.Railgun, true);
                        break;
                    case "Missile":
                        TryApplyingWeapon(WeaponType.Straight_Line_Missile, true);
                        break;
                    case "Bomb":
                        TryApplyingWeapon(WeaponType.Bombs, true);
                        break;
                }
            }

            if (hit.collider.tag == "RightWeapon")
            {
                switch (draggedWeapon.name)
                {
                    case "Railgun":
                        TryApplyingWeapon(WeaponType.Railgun, false);
                        break;
                    case "Missile":
                        TryApplyingWeapon(WeaponType.Straight_Line_Missile, false);
                        break;
                    case "Bomb":
                        TryApplyingWeapon(WeaponType.Bombs, false);
                        break;
                }
            }
        }
        else
        {
            draggedWeapon.position = weaponStartPosition;
            ResetDragged();
        }
    }

    void TryApplyingWeapon(WeaponType weapon, bool isLeftWeapon)
    {
        if (isLeftWeapon)
        {
            if (weaponScript.SetLeftWeapon(weapon))
            {
                draggedWeapon.position = leftWeaponSlot.position;
            }
        }
        else
        {
            if (weaponScript.SetRightWeapon(weapon)) 
                draggedWeapon.position = rightWeaponSlot.position;
        }

        ResetDragged();
    }

    void ResetDragged()
    {
        draggedWeapon = null;
        draggedWeaponRB.velocity = Vector2.zero;
    }
}
