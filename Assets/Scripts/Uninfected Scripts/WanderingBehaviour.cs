using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingBehaviour : MonoBehaviour
{
    public float detectionRadius = 2f;
    public float startVelocitySpeedAvg = 2f;
    public float seperationRadius;

    private float nearbyUninfected = 0;
    private int interval = 10;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Instantiate early movement within the boid
        rb.velocity = RandomEarlyMovement(-startVelocitySpeedAvg, startVelocitySpeedAvg);
    }

    // Function that Instantiates early movement, until entering a boid group and inheriting velocity from them.
    private Vector2 RandomEarlyMovement(float min, float max)
    {
        float x = Random.Range(min, max);
        float y = Random.Range(min, max);
        return new Vector2(x, y);
    }

    // Hit Collider check, searching for nearby 2D Colliders that are both Uninfected Civillians and not themself.
    void nearbyCivillians(Vector2 centre, float radius)
    {
        Collider2D[] hitcolliders = Physics2D.OverlapCircleAll(centre, radius);

        int i = 0;
        while (i < hitcolliders.Length)
        {
            if(hitcolliders[i].gameObject.tag == "uninfectedCivillian" && hitcolliders[i].gameObject != this.gameObject)
            {
                Rigidbody2D otherrb2D = hitcolliders[i].gameObject.GetComponent<Rigidbody2D>();
                cohesion(otherrb2D);
                seperation(otherrb2D, seperationRadius);
                print("Friend");
            }
            i++;
        }
    }

    void cohesion(Rigidbody2D nearbyUninfectedRB)
    {
        Vector2 currentVelocity = rb.velocity;
        rb.velocity = (nearbyUninfectedRB.velocity + nearbyUninfectedRB.velocity) / 2;
    }

    void seperation(Rigidbody2D nearbyUninfectedRB, float seperationRadius)
    {
        if (nearbyUninfectedRB.transform == rb.transform)
        {

        }
    }

    void Update()
    {
        // Efficiency Function, I don't think it's neccessary to call nearby colliders 60/144 times a second.
        if (Time.frameCount % interval == 0)
        {
            nearbyCivillians(this.transform.position, detectionRadius);
            print(nearbyUninfected); // DEBUG
        }
        


    }
}
