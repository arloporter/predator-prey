using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACAHead : MonoBehaviour
{

    public float playerSightDistance; // The distance determined as the boid Seeing the player
    public float bufferDistance; // Designed as a deadzone so that the boid focuses on the Fleeing state rather than flip-flopping at equal numbers
    private bool pursuitBuffer = false; // A simple Boolean to make the distance to return to wandering a bit bigger than the base sight radius

    public Sprite[] sprites;
    private SpriteRenderer spriteAnimation;
    private GameObject player;

    // Component list
    private MonoBehaviour pathfindingBehaviour;
    private MonoBehaviour steeringPursuitBehaviour;
    private MonoBehaviour obstacleAvoidance;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pathfindingBehaviour = GetComponent<AnticovidAgentGameplayA2>();
        steeringPursuitBehaviour = GetComponent<ACASteerPursuit>();
        obstacleAvoidance = GetComponent<ObstacleAvoidance2>();
        spriteAnimation = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
    }


    // So, what will be done here is the Vector2 Magnitude calculation, determining the distance between both the player and the script owner(In this case the Anti-Covid Agent)
    // As the game plays, the code will enable/disable scripts to facilitate with the ease and similarity of a typical FSM.
    // Iterating depending on what is occuring within the game.
    private void Update()
    {
        float distance = Vector2.Distance(player.transform.position, rb.transform.position);
        if (distance < playerSightDistance && pursuitBuffer == false) // FSM Transistion to Flee if the player gets too close
        {
            pathfindingBehaviour.enabled = false;
            steeringPursuitBehaviour.enabled = true;
            obstacleAvoidance.enabled = true;
            pursuitBuffer = true;
        }
        
        if(distance > bufferDistance && pursuitBuffer == true)
        {
            pathfindingBehaviour.enabled = true;
            steeringPursuitBehaviour.enabled = false;
            obstacleAvoidance.enabled = false;
            pursuitBuffer=false;
        }

        Vector2 movement = rb.velocity;

        if (movement.x > 0)
        {
            this.spriteAnimation.sprite = this.sprites[5];
        }
        if (movement.x < 0)
        {
            this.spriteAnimation.sprite = this.sprites[4];
        }
        if (movement.y > 0)
        {
            this.spriteAnimation.sprite = this.sprites[1];
        }
        if (movement.y < 0)
        {
            this.spriteAnimation.sprite = this.sprites[6];
        }
        if (movement.x > 0 && movement.y > 0)
        {
            this.spriteAnimation.sprite = this.sprites[2];
        }
        if (movement.x < 0 && movement.y > 0)
        {
            this.spriteAnimation.sprite = this.sprites[0];
        }
        if (movement.x > 0 && movement.y < 0)
        {
            this.spriteAnimation.sprite = this.sprites[7];
        }
        if (movement.x < 0 && movement.y < 0)
        {
            this.spriteAnimation.sprite = this.sprites[5];
        }
    }
}
