
using System.Collections;
using UnityEngine;

public class EnterOperationsBase : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject heavenSky;
    public GameObject operationsBase;
    public WeaponScript weaponScript;
    public Animator transitionToBase;

    public float animDuration = 1.0f;

    bool isInCollision;

    IEnumerator TransitionToBase()
    {
        playerController.ToggleInTheClouds();
        weaponScript.DisableFire();
        transitionToBase.SetTrigger(STRINGS.TRIGGER_FADE_TO_WHITE);
        yield return new WaitForSeconds(animDuration);

        heavenSky.SetActive(true);
        operationsBase.SetActive(true);

        transitionToBase.SetTrigger(STRINGS.TRIGGER_FADE_OUT_OF_WHITE);
        yield return new WaitForSeconds(animDuration);

        isInCollision = false;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(!isInCollision && other.tag == STRINGS.PLAYER)
        {
            StartCoroutine(TransitionToBase());
            isInCollision = true;
        }
    }
}
