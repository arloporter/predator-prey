using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance2 : MonoBehaviour
{
    // Initialise
    public LayerMask Collidables;
    public float antiColliderStrength = 1f;
    public float raycastDistance = 2f;

    private Rigidbody2D player;


    // Start is called before the first frame update
    void Start()
    {
        this.player = GetComponent<Rigidbody2D>();
    }

    // Function that takes in a direction and a raycast distance
    public void collisionAvoidance(Vector2 directionalVector, float rcDistance)
    {
        //Centre the raycast on the player and shoot into the direction desired
        Vector2 position = this.player.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(position, directionalVector , rcDistance, Collidables);
        // If a hit is made
        if(hit.collider != null)
        {
            // colliable tracks the transform position - hit.point(NOT hit.transform)
            Vector2 collidable = (Vector2)player.transform.position - hit.point;
            float distance = collidable.magnitude; 
            float distancePower = 1.0f / distance; // Function that makes the distane a multiple rather than a division, linear power increase, the closer to the obstacle, the higher the power(Lower distance)
            player.AddForce((collidable.normalized * distancePower) * antiColliderStrength); // Then pushes away from the hit.point transform of the collider
        }
        else if (hit.collider == null)
        {

        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        // Unfortunate cost of low time, to avoid large increase of force on wall push, diagnoal raycasts are dignificantly smaller but are still present to aid in collision detection
        float raycastDistanceDiagonal = raycastDistance / 2;

        collisionAvoidance(new Vector2(1.0f,0.0f), raycastDistance);
        collisionAvoidance(new Vector2(0.707f, 0.707f), raycastDistanceDiagonal);
        collisionAvoidance(new Vector2(0.0f, 1.0f), raycastDistance);
        collisionAvoidance(new Vector2(-0.707f, 0.707f), raycastDistanceDiagonal);
        collisionAvoidance(new Vector2(-1.0f, 0.0f), raycastDistance);
        collisionAvoidance(new Vector2(-0.707f, -0.707f), raycastDistanceDiagonal);
        collisionAvoidance(new Vector2(0.0f, -1.0f), raycastDistance);
        collisionAvoidance(new Vector2(0.707f, -0.707f), raycastDistanceDiagonal);
    }

}
