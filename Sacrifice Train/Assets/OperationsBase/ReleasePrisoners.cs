using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ReleasePrisoners : MonoBehaviour
{
    public Button btn;
    public GameObject prisonerPrefab;
    public Transform prisonerSpawnPoint;

    public PlayerController player;
    public float prisonerSpawnRate = 0.75f;

    void Start()
    {
        btn.onClick.AddListener(() => StartCoroutine(Release()));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            Release();
    }

    IEnumerator Release()
    {
        for (int i = 0; i < player.prisonersSaved; i++)
        {
            var prisoner = Instantiate(prisonerPrefab, prisonerSpawnPoint.position, Quaternion.identity);
            prisoner.GetComponent<Prisoner>().RunAcrossScreen();

            yield return new WaitForSeconds(prisonerSpawnRate);
        }
        
    }
}