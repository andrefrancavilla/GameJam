
using System.Collections;
using UnityEngine;

public class EnterOperationsBase : MonoBehaviour
{
    public WeaponScript weaponScript;
    public GameObject operationsBase;
    public Animator anim;
    public float transitionTime;

    IEnumerator TransitionToBase()
    {
        anim.SetTrigger(STRINGS.TRIGGER_OPERATIONS_BASE);
        yield return new WaitForSeconds(transitionTime);
        operationsBase.SetActive(true);
        weaponScript.ToggleFire();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == STRINGS.PLAYER)
            TransitionToBase();
    }
}
