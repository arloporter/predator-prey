using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using SharpNeat.Phenomes;
using UnitySharpNEAT;

public class NEATPlayer : UnitController
{
    private int choice;
    private double offset = 0.125;
    private double force;
    private int forceMultiplier = 20;
    public GameObject civilianPrefab;
    private HashSet<Vector2> positionsPassed;
    private int reward;
    private System.Random rnd = new System.Random();
    private Vector2 initialPosition;
    private Rigidbody2D rb;
    private List<List<Vector2>> previousPaths = new List<List<Vector2>>();
    private List<Vector2> currentPath; 
    
    void Start()
    {
        this.currentPath = new List<Vector2>();
        this.rb = GetComponent<Rigidbody2D>();
        initialPosition = this.transform.position;
        this.positionsPassed = new HashSet<Vector2>();

    }
    void Update()
    {
        Vector2 discretizedPos = new Vector2((float)Math.Floor(transform.position.x), (float)Math.Floor(transform.position.y));
        this.positionsPassed.Add(discretizedPos);
        if(!this.currentPath.Contains(discretizedPos))
        {
            this.currentPath.Add(discretizedPos);
        }
        

    }
  
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("uninfectedCivillian"))
        {
            this.reward += 1;
            col.gameObject.SetActive(false);
            int spawnX = this.rnd.Next(5, 35);
            int spawnY = this.rnd.Next(20);
            GameObject civilian = Instantiate(this.civilianPrefab, new Vector3(spawnX, spawnY), Quaternion.identity);
            civilian.SetActive(true);
        }
    }
    protected override void UpdateBlackBoxInputs(ISignalArray inputSignalArray)
    {
        // Called by the base class on FixedUpdate

        // Feed inputs into the Neural Net (IBlackBox) by modifying its InputSignalArray
        // The size of the input array corresponds to NeatSupervisor.NetworkInputCount
        Vector2 position = transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 40.0f);
        bool civilian = false;
        float distanceCivilian = 0.0f;
        Rigidbody2D closestColliderCivilian = null;

        bool ACA = false;
        float distanceACA = 0.0f;
        Rigidbody2D closestACA = null;


        if (colliders.Length > 0)
        {
            foreach (Collider2D col in colliders)
            {
                if (col.tag == "uninfectedCivilian")
                {
                    float tempDistance = Vector2.Distance(position, col.attachedRigidbody.transform.position);
                    if(civilian != false)
                    {
                        if (tempDistance < distanceCivilian)
                        {
                            distanceCivilian = tempDistance;
                            closestColliderCivilian = col.attachedRigidbody;
                        }
                    } else {
                        distanceCivilian = tempDistance;
                        closestColliderCivilian = col.attachedRigidbody;
                        civilian = true;
                    }

                }
                else if (col.tag == "AntiCovidAgent")
                {
                    float tempDistance = Vector2.Distance(position, col.attachedRigidbody.transform.position);
                    if (ACA != false)
                    {
                        if (tempDistance < distanceACA)
                        {
                            distanceACA = tempDistance;
                            closestACA = col.attachedRigidbody;
                        }
                    }
                    else
                    {
                        distanceACA = tempDistance;
                        closestACA = col.attachedRigidbody;
                        ACA = true;
                    }
                }
            }
            if (closestColliderCivilian && closestACA)
            {
                inputSignalArray[0] = position.x;
                inputSignalArray[1] = position.y;
                inputSignalArray[2] = distanceCivilian;
                inputSignalArray[3] = closestColliderCivilian.transform.position.x;
                inputSignalArray[4] = closestColliderCivilian.transform.position.y;
                inputSignalArray[5] = distanceACA;
                inputSignalArray[6] = closestACA.transform.position.x;
                inputSignalArray[7] = closestACA.transform.position.y;

            } else if (closestColliderCivilian && !closestACA) {
                inputSignalArray[0] = position.x;
                inputSignalArray[1] = position.y;
                inputSignalArray[2] = distanceCivilian;
                inputSignalArray[3] = closestColliderCivilian.transform.position.x;
                Debug.Log(inputSignalArray[3]);
                inputSignalArray[4] = closestColliderCivilian.transform.position.y;
                Debug.Log(inputSignalArray[4]);
                inputSignalArray[5] = 0;
                inputSignalArray[6] = 0;
                inputSignalArray[7] = 0;

            } else if (!closestColliderCivilian && closestACA) {
                inputSignalArray[0] = position.x;
                inputSignalArray[1] = position.y;
                inputSignalArray[2] = 0;
                inputSignalArray[3] = 0;
                inputSignalArray[4] = 0;
                inputSignalArray[5] = distanceACA;
                inputSignalArray[6] = closestACA.transform.position.x;
                inputSignalArray[7] = closestACA.transform.position.y;

            } else {
                inputSignalArray[0] = position.x;
                inputSignalArray[1] = position.y;
                inputSignalArray[2] = 0;
                inputSignalArray[4] = 0;
                inputSignalArray[5] = 0;
                inputSignalArray[6] = 0;
                inputSignalArray[7] = 0;
            }
        }
        else
        {
            inputSignalArray[0] = position.x;
            inputSignalArray[1] = position.y;
            inputSignalArray[2] = 0;
            inputSignalArray[3] = 0;
            inputSignalArray[4] = 0;
            inputSignalArray[5] = 0;
            inputSignalArray[6] = 0;
            inputSignalArray[7] = 0;
        }
        //...
    }

    protected override void UseBlackBoxOutpts(ISignalArray outputs)
    {
        Debug.Log(outputs[0]);
        this.choice = (int) Math.Floor(outputs[0] / this.offset);
        this.force = outputs[1];
        switch(this.choice)
        {
            case 0:
                this.rb.AddForce(new Vector2( 0.0f, (float)this.force * this.forceMultiplier));
                break;
            case 1:
                this.rb.AddForce(new Vector2(0.0f, -((float)this.force * this.forceMultiplier)));
                break;
            case 2:
                this.rb.AddForce(new Vector2(((float)this.force * this.forceMultiplier), 0.0f));
                break;
            case 3:
                this.rb.AddForce(new Vector2(-((float)this.force * this.forceMultiplier), 0.0f));
                break;
            case 4:
                this.rb.AddForce(new Vector2(((float)this.force * this.forceMultiplier), ((float)this.force * this.forceMultiplier)));
                break;
            case 5:
                this.rb.AddForce(new Vector2(-((float)this.force * this.forceMultiplier), ((float)this.force * this.forceMultiplier)));
                break;
            case 6:
                this.rb.AddForce(new Vector2(((float)this.force * this.forceMultiplier), -((float)this.force * this.forceMultiplier)));
                break;
            case 7:
                this.rb.AddForce(new Vector2(-((float)this.force * this.forceMultiplier), -((float)this.force * this.forceMultiplier)));
                break;
        }
    }

    public override float GetFitness()
    {
        // Called during the evaluation phase (at the end of each trail)
        // The performance of this unit, i.e. it's fitness, is retrieved by this function.
        // Implement a meaningful fitness function here
        Debug.Log("reward: "+this.reward);
        if(!this.previousPaths.Contains(this.currentPath))
        {
            this.reward += 10;
        }
        return (this.reward*5) + this.positionsPassed.Count;
    }

    protected override void HandleIsActiveChanged(bool newIsActive)
    {
        // Called whenever the value of IsActive has changed
        // Since NeatSupervisor.cs is making use of Object Pooling, this Unit will never get destroyed. 
        // Make sure that when IsActive gets set to false, the variables and the Transform of this Unit are reset!
        // Consider to also disable MeshRenderers until IsActive turns true again.
        
        if (newIsActive == false)
            {
                this.previousPaths.Add(this.currentPath);
                this.positionsPassed = new HashSet<Vector2>();
                transform.position = this.initialPosition;
                transform.rotation = Quaternion.identity;
            }

            foreach (Transform t in transform)
            {
                t.gameObject.SetActive(newIsActive);
            }
        
        
    }
}
