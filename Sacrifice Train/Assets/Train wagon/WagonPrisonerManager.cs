using System.Collections.Generic;
using UnityEngine;

public class WagonPrisonerManager : MonoBehaviour {

    public Sprite emptyWindow;
    public List<SpriteRenderer> prisonerWindows;

    int prisonerWindowsLeft;
    int currentPrisonerWindow;

    void Start()
    {
        prisonerWindowsLeft = prisonerWindows.Count;
        currentPrisonerWindow = 0;
    }

    public void AbsorbPrisoners()
    {
        prisonerWindows[currentPrisonerWindow].sprite = emptyWindow;
        prisonerWindowsLeft--;
        currentPrisonerWindow++;
    }

    public void DisablePrisoners()
    {
        for (int i = 0; i < prisonerWindows.Count; i++)
        {
            Destroy(prisonerWindows[i]);
        }

        enabled = false;
    }
}
