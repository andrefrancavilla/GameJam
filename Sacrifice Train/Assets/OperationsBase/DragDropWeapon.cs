using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class DragDropWeapon : MonoBehaviour
{
    public WeaponScript weaponScript;

    Transform draggedWeapon;
    Rigidbody2D draggedWeaponRB;
    public bool IsDragging { get; private set; } = false;
    public bool IsChosen { get; private set; } = false;

    Vector3 weaponStartPosition;
    const float SPEED_MODIFIER = 0.175f;

    public Transform leftWeaponSlot;
    public Transform rightWeaponSlot;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) TryToDrag();
        if (IsDragging) FollowMouse();
        if (IsDragging && Input.GetMouseButtonUp(0)) ReleaseDragged();
    }

    void TryToDrag()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition;

        List<RaycastResult> hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, hits);

        for (int i = 0; i < hits.Count; i++)
        {
            if (hits[i].gameObject?.tag == STRINGS.PICKABLE_WEAPON)
            {
                draggedWeapon = hits[i].gameObject.transform;
                draggedWeaponRB = draggedWeapon.GetComponent<Rigidbody2D>();
                weaponStartPosition = draggedWeapon.position;
                ActivateDragging();
                break;
            }
        }
        
    }

    void FollowMouse()
    {
        //var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var diff = (Input.mousePosition - draggedWeapon.localPosition);
        float speedMod = SPEED_MODIFIER * Time.deltaTime;

        draggedWeaponRB.velocity = new Vector2(diff.x * speedMod, diff.y * speedMod);
    }

    void ReleaseDragged()
    {
        DeactivateDragging();

        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition;

        List<RaycastResult> hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, hits);

        GameObject hit = null;

        for (int i = 0; i < hits.Count; i++)
        {
            if (hits[i].gameObject.tag == STRINGS.TAG_WEAPON_SLOT)
            {
                hit = hits[i].gameObject;
                break;
            }
        }
        
        if (hit != null)
        {
            if (hit.gameObject.name == STRINGS.NAME_LEFT_WEAPON_SLOT)
            {
                switch (draggedWeapon.name)
                {
                    case STRINGS.TAG_RAILGUN:
                        TryApplyingWeapon(WEAPON_TYPE.RAILGUN, true);
                        break;
                    case STRINGS.TAG_MISSILE:
                        TryApplyingWeapon(WEAPON_TYPE.MISSILE, true);
                        break;
                    case STRINGS.TAG_BOMB:
                        TryApplyingWeapon(WEAPON_TYPE.BOMBS, true);
                        break;
                }
            }

            if (hit.gameObject.tag == STRINGS.NAME_RIGHT_WEAPON_SLOT)
            {
                switch (draggedWeapon.name)
                {
                    case STRINGS.TAG_RAILGUN:
                        TryApplyingWeapon(WEAPON_TYPE.RAILGUN, false);
                        break;
                    case STRINGS.TAG_MISSILE:
                        TryApplyingWeapon(WEAPON_TYPE.MISSILE, false);
                        break;
                    case STRINGS.TAG_BOMB:
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
                IsChosen = true;
            }
        }
        else
        {
            if (weaponScript.SetRightWeapon(weapon))
            {
                draggedWeapon.position = rightWeaponSlot.position;
                IsChosen = true;
            }
        }

        ResetDragging();
    }

    public void ActivateDragging()
    {
        IsDragging = true;
    }

    public void DeactivateDragging()
    {
        IsDragging = false;
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
