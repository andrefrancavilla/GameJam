

using UnityEngine;

public class Prisoner : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;
    bool isRunning = false;

    const float RUN_SPEED = 375.0f;
    const float RUN_DISTANCE = 500.0f;
    const float RUN_MAX_DIRECTION_VARIETY = 0.15f;

    const float DOWNWARDS_ADJUSTMENT = 0.2f;

    float distanceCovered;

    void Update()
    {
        if (isRunning) distanceCovered += (RUN_SPEED * Time.deltaTime);
        if (distanceCovered > RUN_DISTANCE) isRunning = false;
    }

    public void StartRunAcrossScreen()
    {
        anim.SetTrigger(STRINGS.TRIGGER_PRISONER_RUN);
        isRunning = true;
        rb.velocity = new Vector2(
            RUN_SPEED * Time.deltaTime,
            Random.Range(-RUN_MAX_DIRECTION_VARIETY, RUN_MAX_DIRECTION_VARIETY) - DOWNWARDS_ADJUSTMENT);
    }
}