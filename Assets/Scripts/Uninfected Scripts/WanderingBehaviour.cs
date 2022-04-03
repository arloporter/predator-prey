using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingBehaviour : MonoBehaviour
{
    public Vector2 currentVelocity;

    public float detectionRadius = 2f;
    public float startVelocitySpeedAvg = 2f;

    public bool cohesionEnabled = true;
    public bool seperationEnabled = true;
    public bool alignmentEnabled = true;

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
        Collider2D[] hitcolliders = Physics2D.OverlapCircleAll(centre, radius, 1);

        int i = 0;
        int j = 0;
        if(hitcolliders.Length > 1)
        {
            Rigidbody2D[] otherrb2D = new Rigidbody2D[hitcolliders.Length];
            while (i < hitcolliders.Length)
            {
                // Keep an eye on this code
                if (hitcolliders[i].gameObject.tag == "uninfectedCivillian" && hitcolliders[i].gameObject != this.gameObject)
                {
                    otherrb2D[j] = hitcolliders[i].GetComponent<Rigidbody2D>();
                    // print(otherrb2D[j].position);
                    j++;
                }                    
                i++;
            }

            if (cohesionEnabled == true)
            {
                cohesion(otherrb2D);
            }
            if (seperationEnabled == true)
            {
                seperation(otherrb2D, seperationRadius);
            }
            if (alignmentEnabled == true)
            {
                alignment(otherrb2D);
            }
        }
        
    }

    void cohesion(Rigidbody2D[] nearbyUninfectedRB)
    {

    }

    void seperation(Rigidbody2D[] nearbyUninfectedRB, float seperationRadius)
    {
        
        for(int i = 0; i < nearbyUninfectedRB.Length; i++)
        {
            if(nearbyUninfectedRB[i] != null)
            {
                Vector2 movingtowards = rb.position - nearbyUninfectedRB[i].position;
                float distance = movingtowards.sqrMagnitude;
                Vector2 direction = movingtowards / distance;
                if (distance < seperationRadius)
                {
                    rb.AddForce(direction);
                }
            }
        }



        // print(direction); DEBUG
    }

    void alignment(Rigidbody2D[] nearbyUninfectedRB)
    {
        Vector2 currentVelocity = rb.velocity;
        Vector2 avgVelocity = Vector2.zero;

        int velocityAmnt = 0;

        for(int i = 0; i < nearbyUninfectedRB.Length; i++)
        {
            if (nearbyUninfectedRB[i] != null)
            {
                avgVelocity += nearbyUninfectedRB[i].velocity;
                velocityAmnt++;
            }
        }
        

        rb.velocity = avgVelocity / velocityAmnt;
        // print(avgVelocity.normalized); // DEBUG
    }

    void Update()
    {
        // Efficiency Function, I don't think it's neccessary to call nearby colliders 60/144 times a second.
        if (Time.frameCount % interval == 0)
        {
            nearbyCivillians(this.transform.position, detectionRadius);
            // print(nearbyUninfected); // DEBUG
        }

        // currentVelocity = rb.velocity; // DEBUG

    }
}
