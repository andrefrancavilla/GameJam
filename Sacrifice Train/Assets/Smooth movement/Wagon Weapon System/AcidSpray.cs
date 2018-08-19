using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidSpray : MonoBehaviour {

    //[HideInInspector]
    public bool playerInArea;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == STRINGS.PLAYER)
        {
            playerInArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerInArea = false;
    }
}
