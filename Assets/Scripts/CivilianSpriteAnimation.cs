using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianSpriteAnimation : MonoBehaviour
{

    public List<Sprite> sprites;
    public SpriteRenderer renderer;
    public Rigidbody player;


    // Start is called before the first frame update
    void Start()
    {
        this.player = GetComponent<Rigidbody>();
        this.renderer = GetComponent<SpriteRenderer>();

        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 force = player.velocity;
        if (force.x > 0)
        {
            this.renderer.sprite = this.sprites[5];
        }
        if (force.x < 0)
        {
            this.renderer.sprite = this.sprites[4];
        }
        if (force.y > 0)
        {
            this.renderer.sprite = this.sprites[1];
        }
        if (force.y < 0)
        {
            this.renderer.sprite = this.sprites[6];
        }
        if (force.x > 0 && force.y > 0)
        {
            this.renderer.sprite = this.sprites[2];
        }
        if (force.x < 0 && force.y > 0)
        {
            this.renderer.sprite = this.sprites[0];
        }
        if (force.x > 0 && force.y < 0)
        {
            this.renderer.sprite = this.sprites[7];
        }
        if (force.x < 0 && force.y < 0)
        {
            this.renderer.sprite = this.sprites[5];
        }

    }
}
