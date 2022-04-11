using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICHead : MonoBehaviour
{
    public GameObject player;
    public float playerSightDistance; // The distance determined as the boid Seeing the player
    public float bufferDistance; // Designed as a deadzone so that the boid focuses on the Fleeing state rather than flip-flopping at equal numbers

    public Sprite[] sprites;
    private SpriteRenderer spriteAnimation;


    private Rigidbody2D rig;

    // A simple Boolean to make the distance to return to wandering a bit bigger than the base sight radius
    private bool fleeBuffer;
    // Scripts are put here to be enabled/disabled to facilitate FSM
    private MonoBehaviour boidsBehaviour;
    // private MonoBehaviour ENTERSCRIPTNAME; PUT FLEE SCRIPT HERE

    private void Start()
    {
        spriteAnimation = GetComponent<SpriteRenderer>();
        rig = GetComponent<Rigidbody2D>();
        boidsBehaviour = GetComponent<WanderingBehaviour>();
        // GETCOMPONENT HERE

    }
    // So, what will be done here is the Vector2 Magnitude calculation, determining the distance between both the player and the script owner(In this case the uninfected Civillian)
    // As the game plays, the code will enable/disable scripts to facilitate with the ease and similarity of a typical FSM.
    // Iterating depending on what is occuring within the game.
    private void Update()
    {
        // Calculate Distance between player and current gameobject
        float distance = Vector2.Distance(player.transform.position, this.transform.position);
        // print(distance); DEBUG

        // In most cases the unInfeceted Civillian will spawn in as Wander, and then as the player is chasing them down and getting closer, the civillian will transfer to Flee

        // FSM Transition Check to Flee
        if (distance < playerSightDistance && fleeBuffer == false) // FSM Transistion to Flee if the player gets too close
        {
            boidsBehaviour.enabled = false;
            // FleeBehaviour = true
            fleeBuffer = true;
        }
        // FSM Transition Check to Wander
        if (distance > bufferDistance && fleeBuffer == true)
        {
            boidsBehaviour.enabled = true;
            // FleeBehaviour = false;
            fleeBuffer = false;
        }
        // The FSM state to Die is contained with the PlayerHead Script to facilitate with game scoring, for posterity, the code is a basic collider check and if the other trigger is a uninfectedCivillian
        // this civillian unit will "Die" as stated within the FSM



        Vector2 movement = rig.velocity;

        if (movement.x > 0)
        {
            this.spriteAnimation.sprite = this.sprites[4];
        }
        if (movement.x < 0)
        {
            this.spriteAnimation.sprite = this.sprites[3];
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
