using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraScript : MonoBehaviour
{
    public float baseCamSize;
    public float t;
    float realSize;
    float additionalSize;
    float starterYPos; //Player's initial starting position
    float yPos; //Camera's adjusted Y position
    Transform player;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        starterYPos = player.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player)
        {
            additionalSize = Mathf.Clamp(player.position.y - starterYPos, 0, additionalSize + 0.1f);
        }
        realSize = baseCamSize + additionalSize;
        realSize = Mathf.Clamp(realSize, 0, 7.9f);
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, realSize, t);
        yPos = 0.95f * Camera.main.orthographicSize - 4.74f;
        gameObject.transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
    }
}
