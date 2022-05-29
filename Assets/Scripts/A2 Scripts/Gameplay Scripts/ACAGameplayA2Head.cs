using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACAGameplayA2Head: MonoBehaviour
{
    public Sprite[] sprites;
    private SpriteRenderer spriteAnimation;

    private Rigidbody2D rig;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        spriteAnimation = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = rig.velocity;

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
