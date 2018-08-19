using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagonPrisonerManager : MonoBehaviour {

    public List<Animator> prisonerWindowsAnim;
    public PlayerController player;

    const float ABSORB_TIME = 6;

    int prisonerWindowsLeft;
    int currentPrisonerWindow;
    
    void Start()
    {
        prisonerWindowsLeft = prisonerWindowsAnim.Count;
        currentPrisonerWindow = 0;
    }

    IEnumerator AbsorbPrisoners()
    {
        yield return new WaitForSecondsRealtime(ABSORB_TIME);

        if(player.RegisterNewPrisoners())
        {
            prisonerWindowsAnim[currentPrisonerWindow].SetTrigger(STRINGS.TRIGGER_EMPTY_WINDOW);
            prisonerWindowsLeft--;
            currentPrisonerWindow++;
        }
    }

    public void DisablePrisoners()
    {
        for (int i = 0; i < prisonerWindowsAnim.Count; i++)
            Destroy(prisonerWindowsAnim[i]);
        enabled = false;
    }
    
    public void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag == STRINGS.PLAYER)
            StartCoroutine(AbsorbPrisoners());
    }

    public void OnCollisionExit2D(Collision2D c)
    {
        if (c.gameObject.tag == STRINGS.PLAYER)
            StopCoroutine(AbsorbPrisoners());
    }
}
