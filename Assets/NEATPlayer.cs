using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using SharpNeat.Phenomes;
using UnitySharpNEAT;

public class NEATPlayer : UnitController
{
    private double northForce;
    private double eastForce;
    private double southForce;
    private double westForce;
    public GameObject civilianPrefab;

    private int reward;
    private System.Random rnd = new System.Random();
    void Start()
    {

    }
  
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("uninfectedCivillian"))
        {
            this.reward += 1;
            col.gameObject.SetActive(false);
            int spawnX = this.rnd.Next(5, 35);
            int spawnY = this.rnd.Next(20);
            Instantiate(this.civilianPrefab, new Vector3(spawnX, spawnY), Quaternion.identity);
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
        this.northForce = outputs[0];
        Debug.Log(northForce);
        this.eastForce  = outputs[1];
        Debug.Log(eastForce);
        this.southForce = outputs[2];
        Debug.Log(southForce);
        this.westForce  = outputs[3];
        Debug.Log(westForce);

        GetComponent<Rigidbody2D>().AddForce(new Vector2((float)Math.Abs(this.eastForce)*5+ (float)-Math.Abs(this.westForce)*5, (float)Math.Abs(this.northForce)*5+(float)-Math.Abs(this.southForce)*5));
    }

    public override float GetFitness()
    {
        // Called during the evaluation phase (at the end of each trail)
        // The performance of this unit, i.e. it's fitness, is retrieved by this function.
        // Implement a meaningful fitness function here
        Debug.Log(this.reward);
        return this.reward;

        /* public override float GetFitness()
        {
            // calculate a fitness value based on how many laps were driven, how many roads crossed and how many walls touched.

            if (Lap == 1 && CurrentPiece == 0)
            {
                return 0;
            }

            int piece = CurrentPiece;
            if (CurrentPiece == 0)
            {
                piece = 17;
            }

            float fit = Lap * piece - WallHits * 0.2f;
            if (fit > 0)
            {
                return fit;
            }
            return 0;
        }*/

    }

    protected override void HandleIsActiveChanged(bool newIsActive)
    {
        // Called whenever the value of IsActive has changed
        // Since NeatSupervisor.cs is making use of Object Pooling, this Unit will never get destroyed. 
        // Make sure that when IsActive gets set to false, the variables and the Transform of this Unit are reset!
        // Consider to also disable MeshRenderers until IsActive turns true again.
        ;
        /* if (newIsActive == false)
            {
                // the unit has been deactivated, IsActive was switched to false

                // reset transform
                transform.position = _initialPosition;
                transform.rotation = _initialRotation;

                // reset members
                Lap = 1;
                CurrentPiece = 0;
                LastPiece = 0;
                WallHits = 0;
                _movingForward = true;
            }

            // hide/show children 
            // the children happen to be the car meshes => we hide this Unit when IsActive turns false and show it when it turns true
            foreach (Transform t in transform)
            {
                t.gameObject.SetActive(newIsActive);
            }
        }
        */
    }
}
