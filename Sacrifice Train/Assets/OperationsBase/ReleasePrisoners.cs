using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReleasePrisoners : MonoBehaviour
{
    public Button btn;
    public GameObject prisonerPrefab;
    public Transform prisonerSpawnPoint;

    Queue<GameObject> prisoners = new Queue<GameObject>();

    public PlayerController player;
    public float prisonerSpawnRate = 0.75f;

    // makes it so NOT all prisoner positions and movement is exactly alike
    public float spawnPointMaxVarietyX = 6.0f;
    public float spawnPointMaxVarietyY = 2.5f;

    bool isCoroutineRunning = false;

    void Start()
    {
        btn.onClick.AddListener(() => StartCoroutine(Release()));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) StartCoroutine(Release());
    }

    IEnumerator Release()
    {
        isCoroutineRunning = true;
        for (int i = 0; i < player.prisonersSaved; i++)
        {
            var spawnPointVariety = new Vector2(
                Random.Range(-spawnPointMaxVarietyX, spawnPointMaxVarietyX), // x coordinate variance
                Random.Range(-spawnPointMaxVarietyY, spawnPointMaxVarietyY)); // y coordinate variance

            var prisoner = Instantiate(prisonerPrefab, prisonerSpawnPoint.position, Quaternion.identity);
            prisoner.GetComponent<Prisoner>().StartRunAcrossScreen();
            prisoners.Enqueue(prisoner);

            yield return new WaitForSeconds(prisonerSpawnRate);
        }
        isCoroutineRunning = false;
    }

    void SavePrisoners()
    {
        player.totalPrisonersSaved += player.prisonersSaved;
        player.ResetPrisonerCount();
    }

    public bool DestroyPrisoners()
    {
        if (isCoroutineRunning) return false;
        else
        {
            while(prisoners.Count > 0)
            {
                Destroy(prisoners.Dequeue());
            }
            return true;
        }
    }
}