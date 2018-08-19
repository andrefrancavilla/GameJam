
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
    bool isFading;
    float currentAlpha = 0;

    public CanvasRenderer[] operationsBaseCanvases;

    void Start()
    {
        int len = operationsBaseCanvases.Length;
        for (int i = 0; i < len; i++)
        {
            operationsBaseCanvases[i].SetAlpha(0);
        }
    }

    void Update()
    {
        if (isFading)
        {
            FadeRenderers();
            currentAlpha += Time.deltaTime / animDuration;
            if (currentAlpha >= 1.0f) ResetFade();
        }
    }

    IEnumerator TransitionToBase()
    {
        transitionToBase.SetTrigger(STRINGS.TRIGGER_FADE_TO_WHITE);
        yield return new WaitForSeconds(animDuration);
        
        var sprRen = GetComponent<SpriteRenderer>();
        var newAlpha = new Color(sprRen.color.r, sprRen.color.g, sprRen.color.b, 0);
        sprRen.color = newAlpha;

        operationsBase.SetActive(true);
        heavenSky.SetActive(true);
        hangar.SetActive(true);

        weaponScript.DisableFire();
        playerController.ToggleInTheClouds();

        var temp = playerController.GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < temp.Length; i++)
        {
            temp[i].enabled = false;
        }

        currentAlpha = 0;
        isFading = true;

        transitionToBase.SetTrigger(STRINGS.TRIGGER_FADE_OUT_OF_WHITE);
        yield return new WaitForSeconds(animDuration);

        isInCollision = false;
        gameObject.SetActive(false);
    }

    void FadeRenderers()
    {
        for (int i = 0; i < operationsBaseCanvases.Length; i++)
        {
            operationsBaseCanvases[i].SetAlpha(currentAlpha);
        }
    }

    void ResetFade()
    {
        isFading = false;
        currentAlpha = 0.0f;
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
