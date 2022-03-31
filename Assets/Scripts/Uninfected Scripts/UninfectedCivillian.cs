using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UninfectedCivillian : MonoBehaviour
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

        if (velocity.magnitude > maxVelocity)
        {
            velocity = velocity.normalized * maxVelocity;
        }

        this.transform.position += velocity * Time.deltaTime;
        // Rotation code could be handy if a proper model is implemented.
        // this.transform.rotation = Quaternion.LookRotation(velocity);
    }

    private Vector2 SeekAndArrive(Vector2 target)
    {
        Vector2 currentAntiCovidAgentPos = rb2d.position;
        Vector2 desiredVelocity = target - currentAntiCovidAgentPos;



        if (desiredVelocity.magnitude < RADIUS_TO_START_SLOWING_DOWN_FROM)
        {
            desiredVelocity *= (desiredVelocity.magnitude / RADIUS_TO_START_SLOWING_DOWN_FROM);
            print(desiredVelocity);
        }



        desiredVelocity *= speedMultiplier;
        return desiredVelocity;
    }



    private Vector2 OffsetPursuit()
    {
        Vector2 target = player.transform.position;
        Vector2 prediction = player.transform.GetComponent<Rigidbody2D>().velocity * 2;
        return (target + prediction);
    }



    void FixedUpdate()
    {
        Vector2 changeInVelocity = (rb2d.velocity- (OffsetPursuit() * -1));
        Vector2 predictedChangeInVelocity = SeekAndArrive(changeInVelocity);
        rb2d.AddForce(predictedChangeInVelocity);
    }

}
