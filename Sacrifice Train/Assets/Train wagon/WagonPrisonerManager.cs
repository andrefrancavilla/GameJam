using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagonPrisonerManager : MonoBehaviour {

    public Sprite emptyWindow;
    public List<SpriteRenderer> prisonerWindows;

    const float ABSORB_TIME = 6;

    int prisonerWindowsLeft;
    int currentPrisonerWindow;
    
    void Start()
    {
        prisonerWindowsLeft = prisonerWindows.Count;
        currentPrisonerWindow = 0;
    }

    IEnumerator AbsorbPrisoners()
    {
        yield return new WaitForSecondsRealtime(ABSORB_TIME);

        prisonerWindows[currentPrisonerWindow].sprite = emptyWindow;
        prisonerWindowsLeft--;
        currentPrisonerWindow++;
    }

    public void DisablePrisoners()
    {
        for (int i = 0; i < prisonerWindows.Count; i++)
            Destroy(prisonerWindows[i]);
        enabled = false;
    }
    
    public void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag == "Player")
            StartCoroutine(AbsorbPrisoners());
    }

    public void OnCollisionExit2D(Collision2D c)
    {
        if (c.gameObject.tag == "Player")
            StopCoroutine(AbsorbPrisoners());
    }
}
