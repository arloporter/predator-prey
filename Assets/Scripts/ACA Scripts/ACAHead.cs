using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACAHead : MonoBehaviour
{

    
    public Sprite[] sprites;
    private SpriteRenderer spriteAnimation;
    private GameObject player;

    // Component list
    private MonoBehaviour pathfindingBehaviour;
    private Rigidbody2D rb;

    private void Start()
    {
        Debug.unityLogger.logEnabled = false;
        rb = GetComponent<Rigidbody2D>();
        spriteAnimation = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
    }


    // So, what will be done here is the Vector2 Magnitude calculation, determining the distance between both the player and the script owner(In this case the Anti-Covid Agent)
    // As the game plays, the code will enable/disable scripts to facilitate with the ease and similarity of a typical FSM.
    // Iterating depending on what is occuring within the game.
    private void FixedUpdate()
    {
       

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
