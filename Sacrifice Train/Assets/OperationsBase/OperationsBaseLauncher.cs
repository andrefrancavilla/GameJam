
using System.Collections;
using UnityEngine;

public class OperationsBaseLauncher : MonoBehaviour
{
    public GameObject operationsBase;
    public Animator anim;
    public float transitionTime;

    IEnumerator TransitionToBase()
    {
        anim.SetTrigger("GoTransition");
        yield return new WaitForSeconds(transitionTime);
        operationsBase.SetActive(true);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
            TransitionToBase();
    }
}
