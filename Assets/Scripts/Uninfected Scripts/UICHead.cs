using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICHead : MonoBehaviour
{
    public float startVelocitySpeedAvg;

    public Sprite[] sprites;
    private SpriteRenderer spriteAnimation;

    private Rigidbody2D rig;

    private void Start()
    {
        spriteAnimation = GetComponent<SpriteRenderer>();
        rig = GetComponent<Rigidbody2D>();

        // rig.velocity = RandomEarlyMovement(-startVelocitySpeedAvg, startVelocitySpeedAvg);
    }

    public void RandomEarlyMovement(float min, float max)
    {
        float x = Random.Range(min, max);
        float y = Random.Range(min, max);
        rig.velocity = new Vector2(x, y);
    }

    public IEnumerator EnumMovement(float min, float max)
    {
        float x = Random.Range(min, max);
        float y = Random.Range(min, max);
        rig.AddForce(new Vector2(x, y));
        yield return new WaitForSeconds(2);
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
