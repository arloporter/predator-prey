using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;

    private Rigidbody2D rig;
    private PlayerInput playerInput;
    public Sprite[] sprites;
    private SpriteRenderer spriteAnimation;
    private float movementX;
    private float movementY;


    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        spriteAnimation = GetComponent<SpriteRenderer>();
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void Update()
    {
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

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 movement = new Vector2(movementX, movementY);

        rig.AddForce(movement * moveSpeed);
    }
}
