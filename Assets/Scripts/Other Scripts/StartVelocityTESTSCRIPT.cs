using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartVelocityTESTSCRIPT : MonoBehaviour
{
    public float startingVelocityX;
    public float startingVelocityY;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(startingVelocityX, startingVelocityY);
    }
}
