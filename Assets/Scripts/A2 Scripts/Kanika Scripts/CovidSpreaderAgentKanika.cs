using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
public class CovidSpreaderAgentKanika : Agent
{

    public float speedMultiplier;
    public List<float> observations;
    public float visibleDistance;
    public int uninfectedCivilliansVisible;
    public int antiCovidAgentsVisible;
    public Sprite[] sprites;

    private Rigidbody2D rb2d;
    private SpriteRenderer sr;

    // private Vector3 covidSpreaderStartPos;

    public Transform uninfectedCivillianParent;
    public Transform antiCovidAgentParent;

    private Vector3[] uninfectedCivillianTransformArray;
    private Vector3[] antiCovidAgentTransformArray;

    private int uninfectedCivilliansCaught;
    void Start()
    {
        Application.runInBackground = true;

        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        uninfectedCivillianTransformArray = new Vector3[uninfectedCivillianParent.childCount];
        int uninfectedRecordSpawnPositionCounter = 0;

        foreach (Transform uninfected in uninfectedCivillianParent)
        {
            uninfectedCivillianTransformArray[uninfectedRecordSpawnPositionCounter] = uninfected.position;
            uninfectedRecordSpawnPositionCounter++;
        }

        antiCovidAgentTransformArray = new Vector3[antiCovidAgentParent.childCount];
        int antiCovidAgentRecordSpawnPositionCounter = 0;
        foreach (Transform antiCovidAgent in antiCovidAgentParent)
        {
            antiCovidAgentTransformArray[antiCovidAgentRecordSpawnPositionCounter] = antiCovidAgent.position;
            antiCovidAgentRecordSpawnPositionCounter++;
        }
    }



    public void HandleCollectionUninfectedCivillian()
    {
        Debug.Log("Got a Civillian");
        AddReward(1.0f);
        uninfectedCivilliansCaught++;

        if (uninfectedCivilliansCaught == uninfectedCivillianParent.childCount)
        {
            EndEpisode();
        }
    }

    private void collectObsFromParent(Transform parent, int max_obs, List<float> obs)
    {
        List<Vector2> objectOffsets = new List<Vector2>();
        foreach (Transform t in parent)
        {
            if (t.gameObject.activeSelf)
            {
                objectOffsets.Add(t.position - transform.position);
            }
        }
        objectOffsets.Sort((v1, v2) => v1.magnitude.CompareTo(v2.magnitude));

        for (int i = 0; i < max_obs; i++)
        {
            if (i < objectOffsets.Count)
            {
                observations.Add(Mathf.Sign(objectOffsets[i].x) * Mathf.Max(1 - Mathf.Abs(objectOffsets[i].x) / visibleDistance, 0.0f));
                observations.Add(Mathf.Sign(objectOffsets[i].y) * Mathf.Max(1 - Mathf.Abs(objectOffsets[i].y) / visibleDistance, 0.0f));
            }
            else
            {
                observations.Add(0.0f);
                observations.Add(0.0f);
            }
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        observations.Clear();

        collectObsFromParent(uninfectedCivillianParent, uninfectedCivilliansVisible, observations);
        collectObsFromParent(antiCovidAgentParent, antiCovidAgentsVisible, observations);

        observations.Add(rb2d.velocity.x);
        observations.Add(rb2d.velocity.y);

        foreach (float obs in observations)
        {
            sensor.AddObservation(obs);
        }
    }
    public void HandleHitAntiCovidAgent()
    {
        AddReward(-0.5f);
        Debug.Log("Got Caught");
    }

    public override void OnEpisodeBegin()
    {
        uninfectedCivilliansCaught = 0;

        // To be used for randomised spawning of the Covid Spreader
        float randomXPos = Random.Range(20, 24);
        float randomYPos = Random.Range(10, 20);

        transform.position = new Vector3 (randomXPos, randomYPos, 0.0f);

        int uninfectedPositionResetCounter = 0;
        foreach (Transform uninfected in uninfectedCivillianParent)
        {
            uninfected.gameObject.SetActive(true);
            uninfected.position = uninfectedCivillianTransformArray[uninfectedPositionResetCounter];
            uninfectedPositionResetCounter++;
            //uncomment when testing
            //uninfected.GetComponent<UICHeadA2>().RandomEarlyMovement();
        }

        int antiCovidAgentPositionResetCounter = 0;
        foreach (Transform antiCovidAgent in antiCovidAgentParent)
        {
            antiCovidAgent.gameObject.SetActive(true);
            antiCovidAgent.position = antiCovidAgentTransformArray[antiCovidAgentPositionResetCounter];
            antiCovidAgentPositionResetCounter++;
            // antiCovidAgent.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int action = actions.DiscreteActions[0];

        float moveHorizontal = 0.0f;
        float moveVertical = 0.0f;

        switch (action)
        {
            case 0:
                break;
            case 1:
                moveHorizontal = -1.0f;
                break;
            case 2:
                moveHorizontal = -0.71f;
                moveVertical = -0.71f;
                break;
            case 3:
                moveVertical = -1f;
                break;
            case 4:
                moveVertical = -0.71f;
                moveHorizontal = 0.71f;
                break;
            case 5:
                moveHorizontal = 1f;
                break;
            case 6:
                moveHorizontal = 0.71f;
                moveVertical = 0.71f;
                break;
            case 7:
                moveVertical = 1f;
                break;
            case 8:
                moveHorizontal = -0.71f;
                moveVertical = 0.71f;
                break;
        }

        if (moveHorizontal > 0)
        {
            this.sr.sprite = this.sprites[4];
        }
        if (moveHorizontal < 0)
        {
            this.sr.sprite = this.sprites[3];
        }
        if (moveVertical > 0)
        {
            this.sr.sprite = this.sprites[1];
        }
        if (moveVertical < 0)
        {
            this.sr.sprite = this.sprites[6];
        }
        if (moveHorizontal > 0 && moveVertical > 0)
        {
            this.sr.sprite = this.sprites[2];
        }
        if (moveHorizontal < 0 && moveVertical > 0)
        {
            this.sr.sprite = this.sprites[0];
        }
        if (moveHorizontal > 0 && moveVertical < 0)
        {
            this.sr.sprite = this.sprites[7];
        }
        if (moveHorizontal < 0 && moveVertical < 0)
        {
            this.sr.sprite = this.sprites[5];
        }

        Vector2 forceDirection = new Vector2(moveHorizontal, moveVertical);

        rb2d.AddForce(forceDirection * speedMultiplier);
    }
}
