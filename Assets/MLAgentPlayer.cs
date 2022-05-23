using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class MLAgentPlayer : Agent
{
    public static System.Random rm = new System.Random();
    public int maxIndex;
    private int choice;
    private double offset = 0.125;
    private double force;
    private int forceMultiplier = 20;
    public GameObject civilianPrefab;
    public int reward;
    private Vector2 initialPosition;
    private Rigidbody2D rb;
    private List<(float, float)> initialCivlocations = new List<(float, float)>() {(10.88959f, 18.23437f),
                                                          (9.99f, 16.89f), (11.82f, 15.29f), (13.17f,19.37f),
                                                           (16.47f, 19.18f), (19.53f, 19.42f), (18.91052f, 17.2753f),
                                                            (19.80305f, 15.02122f), (17.69f, 13.72f), (18.46f, 12.19f),
                                                            (19.51f, 10.0f), (17.55f, 9.53f), (14.77218f, 11.67274f),
                                                            (10.15f, 9.73f), (10.58907f, 12.36152f)};

    
    void FixedUpdate()
    {
        this.civilianPrefab = GameObject.FindGameObjectWithTag("uninfectedCivillian");
    }
    void Start()
    {
        Debug.unityLogger.logEnabled = true;
        this.rb = GetComponent<Rigidbody2D>();
        initialPosition = this.transform.position;
    }
  
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("uninfectedCivillian"))
        {
            AddReward(1);
            this.reward += 1;
            col.gameObject.SetActive(false);
            int spawnX = rm.Next(6, 15);
            int spawnY = rm.Next(6, 15);
            if (GameObject.FindGameObjectsWithTag("uninfectedCivillian").Length < 15)
            {
                GameObject civilian = Instantiate(this.civilianPrefab, new Vector3(spawnX, spawnY), Quaternion.identity);
                civilian.SetActive(true);
                civilian.GetComponent<WanderingBehaviour>().enabled = true;
                civilian.GetComponent<UICHead>().enabled = false;
                civilian.GetComponent<CapsuleCollider2D>().enabled = true;
            }
        }
    }
    
    /*public override void CollectObservations(VectorSensor sensor)
    {
       
        Transform position = this.transform;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position.position, 100.0f, 3);

        if (colliders.Length > 0)
        {
            foreach (Collider2D col in colliders)
            {
                Vector2 pos = (Vector2)col.transform.position - (Vector2)position.position;
                float tempDistance = pos.magnitude; // Vector2.Distance(position.position, pos);
                sensor.AddObservation(tempDistance);
                sensor.AddObservation(pos.x);
                sensor.AddObservation(pos.y);
            }
        }
    }
    */
    public override void OnActionReceived(ActionBuffers outputs)
    {
        int outAction = outputs.DiscreteActions[0];

        /*
        List<float> actions = new List<float>();
        for(int i = 0; i < 9; i++)
        {
            actions.Add(outputs.ContinuousActions[i]);
        }

        float max = actions.Max();
        float outAction = 0;
        for(int i = 0; i < 9; i++)
        {
            if(outputs.ContinuousActions[i] == max)
            {
                outAction = i;
            }
        }
        */
        

        float force = 10.0f;
        switch (outAction)
        {
            case 0:
                this.rb.AddForce(new Vector2( 0.0f, force));
                
                break;
            case 1:
                this.rb.AddForce(new Vector2(0.0f, -force));
                break;
            case 2:
                this.rb.AddForce(new Vector2(force, 0.0f));
                break;
            case 3:
                this.rb.AddForce(new Vector2(-force, 0.0f));
                break;
            case 4:
                this.rb.AddForce(new Vector2(force, force));
                break;
            case 5:
                this.rb.AddForce(new Vector2(-force, force));
                break;
            case 6:
                this.rb.AddForce(new Vector2(force, -force));
                break;
            case 7:
                this.rb.AddForce(new Vector2(-force, -force));
                break;
            

        }
    }


    public override void OnEpisodeBegin()
    {
        this.reward = 0;
        SetReward(0);
        transform.position = this.initialPosition;
        transform.rotation = Quaternion.identity;
        
        int i = 0;
        foreach(GameObject civilian in GameObject.FindGameObjectsWithTag("uninfectedCivillian"))
        {
            civilian.transform.position = new Vector2(this.initialCivlocations[i].Item1, this.initialCivlocations[i].Item2);
            i += 1;
        }
        
    }
}

// [1] https://gist.github.com/jogleasonjr/55641e503142be19c9d3692b6579f221