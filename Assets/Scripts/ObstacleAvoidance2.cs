using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance2 : MonoBehaviour
{
    public LayerMask Collidables;
    public float antiColliderStrength = 1;
    public float raycastDistance = 2;

    private Rigidbody2D player;


    // Start is called before the first frame update
    void Start()
    {
        this.player = GetComponent<Rigidbody2D>();
    }

    public void collisionAvoidance(Vector2 directionalVector, float rcDistance)
    {
        Vector2 position = this.player.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(position, directionalVector , rcDistance, Collidables);
        if(hit.collider != null)
        {
            Vector2 collidable = (Vector2)player.transform.position - hit.point;
            float distance = collidable.magnitude;
            float distancePower = 1 / distance;
            player.AddForce((collidable.normalized * distancePower) * antiColliderStrength);
        }
        else if (hit.collider == null)
        {

        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
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
