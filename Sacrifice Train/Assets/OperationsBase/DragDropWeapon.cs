

using UnityEngine;

public class DragDropWeapon : MonoBehaviour
{
    public WeaponScript weaponScript;

    Transform draggedWeapon;
    Rigidbody2D draggedWeaponRB;
    public bool IsDragging { get; private set; } = false;
    public bool IsChosen { get; private set; } = false;

    Vector3 weaponStartPosition;
    const float SPEED_MODIFIER = 60000.0f;

    public Transform leftWeaponSlot;
    public Transform rightWeaponSlot;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) TryToDrag();
        if (IsDragging && Input.GetMouseButtonUp(0)) ReleaseDragged();
        if (IsDragging) FollowMouse();
    }

    void TryToDrag()
    {
        var hits = Physics2D.RaycastAll(
            Camera.main.ScreenToWorldPoint(Input.mousePosition), 
            Vector2.zero);

        int len = hits.Length;
        for (int i = 0; i < len; i++)
        {
            if (hits[i].collider?.tag == STRINGS.PICKABLE_WEAPON)
            {
                draggedWeapon = hits[i].collider.transform;
                draggedWeaponRB = draggedWeapon.GetComponent<Rigidbody2D>();
                weaponStartPosition = draggedWeapon.position;
                ActivateDragging();
                break;
            }
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
            if (hits[i].collider.tag == STRINGS.TAG_WEAPON_SLOT)
            {
                hit = hits[i];
                break;
            }
        }
        
        if (hit.collider != null)
        {
            if (hit.collider.name == STRINGS.NAME_LEFT_WEAPON_SLOT)
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

            if (hit.collider.tag == STRINGS.NAME_RIGHT_WEAPON_SLOT)
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
