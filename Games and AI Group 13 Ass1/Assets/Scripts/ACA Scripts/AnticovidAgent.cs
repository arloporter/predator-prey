using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnticovidAgent : MonoBehaviour
{

    public GameObject player;
    public float speedMultiplier;
    private Vector2 lastPosition;
    private Rigidbody2D rb2d;
    private const float RADIUS_TO_START_SLOWING_DOWN_FROM = 7f;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        lastPosition = player.transform.position;
        print(lastPosition);
    }

    void FixedUpdate()
    {
        Vector2 currentAntiCovidAgentPos = rb2d.position;
        Vector2 desiredVelocity = lastPosition - currentAntiCovidAgentPos;

        if (desiredVelocity.magnitude < RADIUS_TO_START_SLOWING_DOWN_FROM)
        {
            desiredVelocity *= (desiredVelocity.magnitude / RADIUS_TO_START_SLOWING_DOWN_FROM);
            print(desiredVelocity);
        }

        desiredVelocity *= speedMultiplier;
        Vector2 changeInVelocity = (desiredVelocity - rb2d.velocity);
        rb2d.AddForce(changeInVelocity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
            other.gameObject.SetActive(false);
    }
}
