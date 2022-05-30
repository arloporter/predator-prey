using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
public class CovidSpreaderAgentGameplay : Agent
{
    public bool trainingMode;

    public float speedMultiplier;

    public Sprite[] sprites;

    private Rigidbody2D rb2d;
    private SpriteRenderer sr;

    private GameObject gameManagerObject;
    void Start()
    {

        gameManagerObject = GameObject.Find("GameManager");
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        if (!trainingMode)
        {
            MaxStep = 0;
        }

        Time.timeScale = 20;

    }

    public void HandleCollectionUninfectedCivillian()
    {
        Debug.Log("Got a Civillian");
        AddReward(1.0f);
    }

    public void HandleHitAntiCovidAgent()
    {
        AddReward(-1.0f);
        Debug.Log("Got Caught");
    }

    public override void OnEpisodeBegin()
    {
        gameManagerObject.GetComponent<GameManager>().cleanGame();
        gameManagerObject.GetComponent<GameManager>().StartNormalGame();

        float randomXPos = Random.Range(20, 24);
        float randomYPos = Random.Range(13, 17);

        transform.position = new Vector3(randomXPos, randomYPos, 0.0f);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(0.0f); // Dummy
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

        Vector2 forceDirection = new Vector2(moveHorizontal, moveVertical);

        rb2d.AddForce(forceDirection * speedMultiplier);
    }

    private void Update()
    {
        if (rb2d.velocity.x > 0)
        {
            this.sr.sprite = this.sprites[4];
        }
        if (rb2d.velocity.x < 0)
        {
            this.sr.sprite = this.sprites[3];
        }
        if (rb2d.velocity.y > 0)
        {
            this.sr.sprite = this.sprites[1];
        }
        if (rb2d.velocity.y < 0)
        {
            this.sr.sprite = this.sprites[6];
        }
        if (rb2d.velocity.x > 0 && rb2d.velocity.y > 0)
        {
            this.sr.sprite = this.sprites[2];
        }
        if (rb2d.velocity.x < 0 && rb2d.velocity.y > 0)
        {
            this.sr.sprite = this.sprites[0];
        }
        if (rb2d.velocity.x > 0 && rb2d.velocity.y < 0)
        {
            this.sr.sprite = this.sprites[7];
        }
        if (rb2d.velocity.x < 0 && rb2d.velocity.y < 0)
        {
            this.sr.sprite = this.sprites[5];
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "uninfectedCivillian")
        {
            // other.gameObject.SetActive(false);
            this.gameObject.GetComponent<CovidSpreaderAgentGameplay>().HandleCollectionUninfectedCivillian();
            gameManagerObject.GetComponent<GameManager>().spawnNewUninfected(other);
        }
        if (other.gameObject.tag == "AntiCovidAgent" && !trainingMode)
        {
            this.gameObject.GetComponent<CovidSpreaderAgentGameplay>().HandleHitAntiCovidAgent();
            // this.gameObject.SetActive(false);
            EndEpisode();
        }
        if (other.gameObject.tag == "AntiCovidAgent" && trainingMode)
        {
            this.gameObject.GetComponent<CovidSpreaderAgentGameplay>().HandleHitAntiCovidAgent();
            // other.gameObject.SetActive(false);
            gameManagerObject.GetComponent<GameManager>().spawnNewAntiCovidAgent(other);
        }
    }
}
