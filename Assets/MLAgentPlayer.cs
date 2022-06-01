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
    private double force;
    private int forceMultiplier = 20;
    public GameObject civilianPrefab;
    public int reward;
    private Vector2 initialPosition;
    private Rigidbody2D rb;
    public List<Vector2> initialACALocations;
    public List<Vector2> initialCivLocations;
    public Transform parent;
    public Vector2 previousPosition;
    
    void FixedUpdate()
    {
        this.civilianPrefab = GameObject.FindGameObjectWithTag("uninfectedCivillian");
    }
    IEnumerator drawLines()
    {
        while (true)
        {
            if (this.previousPosition != null)
            {
                Debug.DrawLine(this.previousPosition, transform.position, Color.red, 99999999.9f, false);
            }
            this.previousPosition = transform.position;
            yield return new WaitForSeconds(0.1f);
        }
    }
    void Start()
    {
        Debug.unityLogger.logEnabled = true;
        this.rb = GetComponent<Rigidbody2D>();
        initialPosition = this.transform.position;
        foreach(Transform civilian in parent)
        {
            initialCivLocations.Add(civilian.position);
        }
        foreach(GameObject ACA in GameObject.FindGameObjectsWithTag("AntiCovidAgent"))
        {
            initialACALocations.Add(ACA.transform.position);
        }
        //StartCoroutine(drawLines());
    }
  
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("uninfectedCivillian"))
        {
            AddReward(1);
            
            col.gameObject.SetActive(false);
            int spawn = rm.Next(0, this.initialCivLocations.Count);
            if (GameObject.FindGameObjectsWithTag("uninfectedCivillian").Length < 15)
            {
                GameObject civilian = Instantiate(this.civilianPrefab, this.initialCivLocations[spawn], Quaternion.identity);
                civilian.SetActive(true);
                civilian.GetComponent<UICHead>().enabled = true;
                civilian.GetComponent<CapsuleCollider2D>().enabled = true;
            }
        }
        if (col.gameObject.CompareTag("AntiCovidAgent"))
        {
            EndEpisode();
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
            civilian.transform.position = new Vector2(this.initialCivLocations[i].x, this.initialCivLocations[i].y);
            civilian.GetComponent<UICHead>().RandomEarlyMovement(-1.0f, 1.0f);
            StartCoroutine(civilian.GetComponent<UICHead>().EnumMovement(-1.0f, 1.0f));
            i += 1;
        }
        int y = 0;
        foreach(GameObject anticovidagent in GameObject.FindGameObjectsWithTag("AntiCovidAgent"))
        {
            anticovidagent.transform.position = new Vector2(this.initialACALocations[y].x, this.initialACALocations[y].y);
            y += 1;
        }
        
    }
}

// [1] https://gist.github.com/jogleasonjr/55641e503142be19c9d3692b6579f221