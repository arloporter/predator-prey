using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
public class CovidSpreaderAgent : Agent
{

    public float speedMultiplier;

    public Sprite[] sprites;

    private Rigidbody2D rb2d;
    private SpriteRenderer sr;

    private Vector3 covidSpreaderStartPos;

    public Transform uninfectedCivillianParent;

    private int uninfectedCivilliansCaught;
    void Start()
    {
        Application.runInBackground = true;

        MaxStep = 2000;

        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        covidSpreaderStartPos = transform.position;
    }

    public void HandleCollectionUninfectedCivillian()
    {
        AddReward(1.0f);
        uninfectedCivilliansCaught++;

        if (uninfectedCivilliansCaught == uninfectedCivillianParent.childCount)
        {
            EndEpisode();
        }
    }

    // Unused
    public void HandleHitAntiCovidAgent()
    {
        AddReward(-0.25f);
    }

    public override void OnEpisodeBegin()
    {
        uninfectedCivilliansCaught = 0;

        transform.position = covidSpreaderStartPos;

        foreach (Transform uninfected in uninfectedCivillianParent)
        {
            uninfected.gameObject.SetActive(true);
        }
        // foreach (Transform antiCovidAgent in antiCovidAgentParent)
        // {
        // antiCovidAgent.gameObject.setActive(true);
        // }

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        
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
