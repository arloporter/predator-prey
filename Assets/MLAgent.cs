using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;


public class MLAgent : Agent
{

    private float forceNorth;
    private float forceNorthEast;
    private float forceEast;
    private float forceSouthEast;
    private float forceSouth;
    private float forceSouthWest;
    private float forceWest;
    private float forceNorthWest;


    void Start()
    {
        
    }

    
    void FixedUpdate()
    {
        
    }
    public override void OnEpisodeBegin()
    {
        ;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        ;
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        this.forceNorth     = actions.ContinuousActions[0];
        this.forceNorthEast = actions.ContinuousActions[0];
        this.forceEast      = actions.ContinuousActions[0];
        this.forceSouthEast = actions.ContinuousActions[0];
        this.forceSouth     = actions.ContinuousActions[0];
        this.forceSouthWest = actions.ContinuousActions[0];
        this.forceWest      = actions.ContinuousActions[0];
        this.forceNorthWest = actions.ContinuousActions[0];
    }
}
