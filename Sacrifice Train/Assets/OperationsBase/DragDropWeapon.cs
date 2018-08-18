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

    const float BASE_APPROACH_SPEED = 100.0f;
    const float TIME_SLOW = 1.0f;
    const float MIN_DISTANCE = 10.0f;

    void FixedUpdate ()
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
            draggedWeaponRB = draggedWeapon.GetComponent<Rigidbody2D>();
            weaponStartPosition = draggedWeapon.position;
            isDragging = true;
        }
    }

    void FollowMouse()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        float newX = Mathf.Lerp(draggedWeapon.position.x, mousePos.x, Time.time);
        float newY = Mathf.Lerp(draggedWeapon.position.y, mousePos.y, Time.time);
        draggedWeapon.position = new Vector2(newX, newY);
    }

    void ReleaseDragged()
    {
        isDragging = false;
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        
        if(hit.collider?.tag == "WeaponSlot")
        {
            if (hit.collider?.name == "LeftWeapon")
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

            if (hit.collider?.tag == "RightWeapon")
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

        else ResetDragged();
    }

    void TryApplyingWeapon(WeaponType weapon, bool isLeftWeapon)
    {
        if (isLeftWeapon)
        {
            if (!weaponScript.SetLeftWeapon(weapon))
                ResetDragged();
        }
        else
        {
            if (!weaponScript.SetRightWeapon(weapon))
                ResetDragged();
        }
    }

    void ResetDragged()
    {
        draggedWeapon.position = weaponStartPosition;
        draggedWeapon = null;
        weaponStartPosition = Vector3.zero;
        draggedWeaponRB.velocity = Vector2.zero;
    }
}
