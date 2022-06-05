using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool enableBackgroundRun = true;

    public float numOfSpawnUIC;
    public float numOfSpawnedACA;

    public Transform covidSpreaderParent;
    public Transform uninfectedCivillianParent;
    public Transform antiCovidAgentParent;

    public GameObject prefabCovidSpreader;
    public GameObject[] prefabUIC;
    public GameObject prefabACA;

    public Transform uninfectedCivillianSpawnParent;
    public Transform antiCovidAgentSpawnParent;

    private Vector3[] uninfectedCivillianSpawnTransformArray;
    private Vector3[] antiCovidAgentSpawnTransformArray;



    // Start is called before the first frame update
    void Awake()
    {
        if (enableBackgroundRun)
        {
            Application.runInBackground = true;
        }

        uninfectedCivillianSpawnTransformArray = new Vector3[uninfectedCivillianSpawnParent.childCount];
        int uninfectedRecordSpawnPositionCounter = 0;
        foreach (Transform uninfected in uninfectedCivillianSpawnParent)
        {
            uninfectedCivillianSpawnTransformArray[uninfectedRecordSpawnPositionCounter] = uninfected.position;
            uninfectedRecordSpawnPositionCounter++;
        }

        antiCovidAgentSpawnTransformArray = new Vector3[antiCovidAgentSpawnParent.childCount];
        int antiCovidAgentRecordSpawnPositionCounter = 0;
        foreach (Transform antiCovidAgent in antiCovidAgentSpawnParent)
        {
            antiCovidAgentSpawnTransformArray[antiCovidAgentRecordSpawnPositionCounter] = antiCovidAgent.position;
            antiCovidAgentRecordSpawnPositionCounter++;
        }

        StartNormalGame();

    }

    public void StartNormalGame()
    {
        for (int spawnUIC = 0; spawnUIC < numOfSpawnUIC; spawnUIC++)
        {
            Instantiate(prefabUIC[Random.Range(0, prefabUIC.Length - 1)], uninfectedCivillianSpawnTransformArray[Random.Range(0, uninfectedCivillianSpawnTransformArray.Length - 1)], Quaternion.identity, uninfectedCivillianParent);
        }

        for (int spawnACA = 0; spawnACA < numOfSpawnedACA; spawnACA++)
        {
            Instantiate(prefabACA, antiCovidAgentSpawnTransformArray[Random.Range(0, antiCovidAgentSpawnTransformArray.Length - 1)], Quaternion.identity, antiCovidAgentParent);
        }
    }

    public void spawnNewUninfected(Collision2D other)
    {
        other.transform.position = uninfectedCivillianSpawnTransformArray[Random.Range(0, uninfectedCivillianSpawnTransformArray.Length - 1)];
        other.gameObject.GetComponent<UICHeadGameplayA2>().RandomEarlyMovement();
        // Instantiate(prefabUIC[Random.Range(0, prefabUIC.Length - 1)], uninfectedCivillianSpawnTransformArray[Random.Range(0, uninfectedCivillianSpawnTransformArray.Length - 1)], Quaternion.identity, uninfectedCivillianParent);
    }

    public void spawnNewAntiCovidAgent(Collision2D other)
    {
        other.transform.position = antiCovidAgentSpawnTransformArray[Random.Range(0, antiCovidAgentSpawnTransformArray.Length - 1)];
        // Instantiate(prefabACA, antiCovidAgentSpawnTransformArray[Random.Range(0, antiCovidAgentSpawnTransformArray.Length - 1)], Quaternion.identity, antiCovidAgentParent);
    }

    public void cleanGame()
    {
        //foreach (Transform covidSpreader in covidSpreaderParent)
        //{
        //    Destroy(covidSpreader.gameObject);
        //}

        foreach (Transform uninfectedCivillian in uninfectedCivillianParent)
        {
            Destroy(uninfectedCivillian.gameObject);
        }
        foreach (Transform antiCovidAgent in antiCovidAgentParent)
        {
            Destroy(antiCovidAgent.gameObject);
        }
    }
}
