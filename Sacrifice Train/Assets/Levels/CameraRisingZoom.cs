using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRisingZoom : MonoBehaviour {

    public Transform player;
    public Transform entranceToHeaven;
    public Transform cameraToMove;

    Vector3 startPos;
    Vector3 heavenPos;
    Vector3 cameraPos;

    const float CAMERA_FOV_TO_DISTANCE_RATIO = 90.0f;
    const float CAMERA_FOV_TO_DISTANCE_BASE = 35f;

    const float CAMERA_HEIGHT_TO_DISTANCE_RATIO = 0.4f;
    const float CAMERA_DEPTH_TO_DISTANCE_RATIO = 0.15f;

    const float CAMERA_MAX_HEIGHT = 32.5f;
    const float CAMERA_MAX_DEPTH = 2f;

    float startDistance;

    // Use this for initialization
    void Start () {
        startPos = player.position;
        heavenPos = entranceToHeaven.position;
        cameraPos = cameraToMove.position;

        startDistance = Vector3.Distance(player.position, heavenPos);
    }
	
	// Update is called once per frame
	void Update () {

        float distanceBetween = Vector3.Distance(player.position, heavenPos);
        ChangeFoV(distanceBetween);
        ChangeHeight(distanceBetween);

        if (transform.position.y <= 1.002831f)
            transform.position = new Vector3(transform.position.x, 1.002831f, transform.position.z);
    }

    void ChangeFoV(float distance)
    {
        float adjustingFOV = Mathf.Clamp(CAMERA_FOV_TO_DISTANCE_RATIO / distance, 0, 20f);
        float newFOV = CAMERA_FOV_TO_DISTANCE_BASE + adjustingFOV;
        Camera.main.fieldOfView = newFOV;
    }

    void ChangeHeight(float distance)
    {
        float adjustedHeight = Mathf.Clamp(CAMERA_HEIGHT_TO_DISTANCE_RATIO * distance, 0.0f, CAMERA_MAX_HEIGHT);
        float smoothenedDepth = CAMERA_DEPTH_TO_DISTANCE_RATIO * SmoothenDepth(distance);
        float adjustedDepth = Mathf.Clamp(smoothenedDepth, 0.0f, CAMERA_MAX_DEPTH);

        Camera.main.gameObject.transform.position = new Vector3(cameraPos.x, cameraPos.y - adjustedHeight + 8f, cameraPos.z + adjustedDepth - 6f);
    }

    float SmoothenDepth(float currentValue)
    {
        if (currentValue > startDistance)
            return currentValue;
        else
        {
            float percentageDistance = currentValue / startDistance;
            return startDistance * QuarticEaseIn(percentageDistance);
        }
    }

    float QuarticEaseIn(float p)
    {
        return p * p * p * p;
    }
}
