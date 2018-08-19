

using UnityEngine;
using UnityEngine.UI;

public class ExitOperationsBase : MonoBehaviour
{
    public DragDropWeapon dragDropWeapon;
    public GameObject operationsBase;
    public Button btn;

    public ReleasePrisoners releasePrisoners;

    void Start()
    {
        btn.onClick.AddListener(ReturnToBattle); 
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ReturnToBattle();
	}

    void ReturnToBattle()
    {
        if(releasePrisoners.DestroyPrisoners())
        {
            dragDropWeapon.DeactivateDragging();
            dragDropWeapon.ReturnToOrigin();
            dragDropWeapon.ResetDragging();
            operationsBase.SetActive(false);
        }
    }
}
