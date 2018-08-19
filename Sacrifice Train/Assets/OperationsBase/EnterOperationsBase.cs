
using System.Collections;
using UnityEngine;

public class EnterOperationsBase : MonoBehaviour
{
    public PlayerController playerController;
    public WeaponScript weaponScript;
    public Animator transitionToBase;

    public GameObject hangar;
    public GameObject heavenSky;
    public GameObject operationsBase;

    public float animDuration = 1.0f;

    bool isInCollision;

    IEnumerator TransitionToBase()
    {
        transitionToBase.SetTrigger(STRINGS.TRIGGER_FADE_TO_WHITE);
        yield return new WaitForSeconds(animDuration);

        heavenSky.SetActive(true);
        hangar.SetActive(true);
        operationsBase.SetActive(true);
        weaponScript.DisableFire();
        playerController.ToggleInTheClouds();

        var temp = playerController.GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < temp.Length; i++)
        {
            temp[i].enabled = false;
        }

        transitionToBase.SetTrigger(STRINGS.TRIGGER_FADE_OUT_OF_WHITE);
        yield return new WaitForSeconds(animDuration);

        isInCollision = false;
        gameObject.SetActive(false);
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
