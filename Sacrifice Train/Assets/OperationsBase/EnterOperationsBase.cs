
using System.Collections;
using UnityEngine;

public class EnterOperationsBase : MonoBehaviour
{
    public GameObject operationsBase;
    public WeaponScript weaponScript;
    public Animator transitionToBase;

    public float animDuration = 1.0f;

    IEnumerator TransitionToBase()
    {
        transitionToBase.SetTrigger(STRINGS.TRIGGER_FADE_TO_OPERATIONS_BASE);
        weaponScript.ToggleFire();

        yield return new WaitForSeconds(animDuration);
        operationsBase.SetActive(true);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == STRINGS.PLAYER)
            StartCoroutine(TransitionToBase());
    }
}
