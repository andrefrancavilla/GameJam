using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitOperationsBase : MonoBehaviour
{
    public PlayerController playerController;
    public WeaponScript weaponScript;
    public Animator transitionFromBase;
    public Button btn;

    public GameObject wayToHeaven;
    public GameObject hangar;
    public GameObject heavenSky;
    public GameObject operationsBase;

    public ReleasePrisoners releasePrisoners;
    public List<DragDropWeapon> dragDropWeapons;

    public float animDuration = 1.0f;

    bool isFading;
    float currentAlpha = 1;

    public CanvasRenderer[] operationsBaseCanvases;

    void Start()
    {
        btn.onClick.AddListener(() => StartCoroutine(ReturnToBattle()));
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            StartCoroutine(ReturnToBattle());
        if (isFading)
        {
            FadeRenderers();
            currentAlpha -= Time.deltaTime / animDuration;
            if (currentAlpha <= 0.0f) ResetFade();
        }
    }

    IEnumerator ReturnToBattle()
    {
        if(releasePrisoners.DestroyPrisoners())
        {
            for (int i = 0; i < dragDropWeapons.Count; i++)
            {
                if(dragDropWeapons[i].IsChosen || dragDropWeapons[i].IsDragging)
                {
                    dragDropWeapons[i].DeactivateDragging();
                    dragDropWeapons[i].ReturnToOrigin();
                    dragDropWeapons[i].ResetDragging();
                }
            }

            transitionFromBase.SetTrigger(STRINGS.TRIGGER_FADE_TO_WHITE);
            heavenSky.SetActive(false);
            hangar.SetActive(false);

            currentAlpha = 1.0f;
            isFading = true;

            yield return new WaitForSeconds(animDuration);

            var sprRen = wayToHeaven.GetComponent<SpriteRenderer>();
            var newAlpha = new Color(sprRen.color.r, sprRen.color.g, sprRen.color.b, 1);
            sprRen.color = newAlpha;
            wayToHeaven.SetActive(true);

            var temp = playerController.GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i].enabled = true;
            }

            transitionFromBase.SetTrigger(STRINGS.TRIGGER_FADE_OUT_OF_WHITE);
            yield return new WaitForSeconds(animDuration);

            weaponScript.EnableFire();
            playerController.ToggleInTheClouds();
            operationsBase.SetActive(false);
        }
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
}
