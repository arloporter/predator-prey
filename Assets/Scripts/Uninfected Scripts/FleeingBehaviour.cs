using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeingBehaviour : MonoBehaviour
{
    public float maxVelocity;
    public Vector3 velocity; // Public for testing Purposes

    public GameObject player;
    public float speedMultiplier;
    private Vector2 lastPosition;
    private Rigidbody2D rb2d;
    private LineRenderer lr;
    private const float RADIUS_TO_START_SLOWING_DOWN_FROM = 7f;


    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        lastPosition = player.transform.position;

    }

    private Vector2 Flee(Vector2 target)
    {
        Vector2 unAffectedCivillians = rb2d.position;
        Vector2 desiredVelocity = unAffectedCivillians - target;
        desiredVelocity.Normalize();

        desiredVelocity *= RADIUS_TO_START_SLOWING_DOWN_FROM / desiredVelocity.magnitude;



        desiredVelocity *= speedMultiplier;
        return desiredVelocity;
    }



    void FixedUpdate()
    {
        Vector2 changeInVelocity = Flee(player.transform.position);
        rb2d.AddForce(changeInVelocity);
    }

}
