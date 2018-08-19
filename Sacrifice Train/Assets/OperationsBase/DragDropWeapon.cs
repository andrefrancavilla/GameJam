﻿

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

        if (hit.collider?.tag == TAGS_AND_NAMES.PICKABLE_WEAPON)
        {
            draggedWeapon = hit.collider.transform;
            draggedWeaponRB = draggedWeapon.GetComponent<Rigidbody2D>();
            weaponStartPosition = draggedWeapon.position;
            ActivateDragging();
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
        DeactivateDragging();
        var hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        RaycastHit2D hit = new RaycastHit2D(); int len = hits.Length;
        for (int i = 0; i < len; i++)
        {
            if (hits[i].collider.tag == TAGS_AND_NAMES.WEAPON_SLOT)
            {
                hit = hits[i];
                break;
            }
        }
        
        if (hit.collider != null)
        {
            if (hit.collider.name == TAGS_AND_NAMES.LEFT_WEAPON_SLOT)
            {
                switch (draggedWeapon.name)
                {
                    case TAGS_AND_NAMES.RAILGUN:
                        TryApplyingWeapon(WEAPON_TYPE.RAILGUN, true);
                        break;
                    case TAGS_AND_NAMES.MISSILE:
                        TryApplyingWeapon(WEAPON_TYPE.MISSILE, true);
                        break;
                    case TAGS_AND_NAMES.BOMB:
                        TryApplyingWeapon(WEAPON_TYPE.BOMBS, true);
                        break;
                }
            }

            if (hit.collider.tag == TAGS_AND_NAMES.RIGHT_WEAPON_SLOT)
            {
                switch (draggedWeapon.name)
                {
                    case TAGS_AND_NAMES.RAILGUN:
                        TryApplyingWeapon(WEAPON_TYPE.RAILGUN, false);
                        break;
                    case TAGS_AND_NAMES.MISSILE:
                        TryApplyingWeapon(WEAPON_TYPE.MISSILE, false);
                        break;
                    case TAGS_AND_NAMES.BOMB:
                        TryApplyingWeapon(WEAPON_TYPE.BOMBS, false);
                        break;
                }
            }
        }
        else
        {
            ReturnToOrigin();
            ResetDragging();
        }
    }

    void TryApplyingWeapon(WEAPON_TYPE weapon, bool isLeftWeapon)
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

        ResetDragging();
    }

    public void ActivateDragging()
    {
        isDragging = true;
    }

    public void DeactivateDragging()
    {
        isDragging = false;
    }

    public void ReturnToOrigin()
    {
        draggedWeapon.position = weaponStartPosition;
    }

    public void ResetDragging()
    {
        draggedWeapon = null;
        draggedWeaponRB.velocity = Vector2.zero;
    }
}
