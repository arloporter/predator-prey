using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICHeadGameplayA2: MonoBehaviour
{
    public float startVelocitySpeedAvg;

    public Sprite[] sprites;
    private SpriteRenderer spriteAnimation;

    private Rigidbody2D rig;

    private void Start()
    {
        spriteAnimation = GetComponent<SpriteRenderer>();
        rig = GetComponent<Rigidbody2D>();

        RandomEarlyMovement();
    }

    public void RandomEarlyMovement()
    {
        float x = Random.Range(-startVelocitySpeedAvg, startVelocitySpeedAvg);
        float y = Random.Range(-startVelocitySpeedAvg, startVelocitySpeedAvg);
        rig.velocity = new Vector2(x, y);
    }

    // Update is called once per frame
    void Update()
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
}
