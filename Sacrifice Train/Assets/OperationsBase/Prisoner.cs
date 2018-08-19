

using UnityEngine;

public class Prisoner : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;
    bool isRunning = false;

    const float RUN_SPEED = 50.0f;
    const float RUN_DISTANCE = 500.0f;
    const float RUN_MAX_DIRECTION_VARIETY = 5.0f;

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
            Random.Range(-RUN_MAX_DIRECTION_VARIETY, RUN_MAX_DIRECTION_VARIETY));
    }
}