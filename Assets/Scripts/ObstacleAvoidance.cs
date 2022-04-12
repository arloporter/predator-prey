using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mathf;

public class ObstacleAvoidance : MonoBehaviour
{
    private Rigidbody2D player;
    public LayerMask Collidables;

    public void Start()
    {
        this.player = GetComponent<Rigidbody2D>();
    }

    public float distance(Vector2 x, Vector2 y)
    {
        float distance = Mathf.Abs(x.x - y.x) + Mathf.Abs(x.y - y.y);
        return distance;
    }
    public void collisionAvoidance()
    {
        Vector2 upForce = findObstacleUp();
        Vector2 downForce = findObstacleDown();
        Vector2 leftForce = findObstacleLeft();
        Vector2 rightForce = findObstacleRight();
        Vector2 nullVector = new Vector2(0.0f, 0.0f);

        if (upForce != nullVector)
        {
            this.player.AddForce(upForce*2);
            return;
        }
        else if (downForce != nullVector)
        {
            this.player.AddForce(downForce*2);
            return;
        }
        else if (leftForce != nullVector)
        {
            this.player.AddForce(leftForce*2);
            return;
        }
        else if (rightForce != nullVector)
        {
            this.player.AddForce(rightForce*2);
            return;
        }

    }

    public Vector2 findObstacleUp() { 


        Vector2 position = this.player.transform.position;
        Vector2 upForce = new Vector2(0.0f, 1.0f);
        float distance = 2.0f;
        RaycastHit2D hit = Physics2D.CircleCast(this.player.transform.position, 0.5f, upForce, distance);
        if (hit.collider != null)
        {
            return -upForce;
        }
        else if (hit.collider == null)
        {
            return new Vector2(0.0f, 0.0f);
        }
        return new Vector2(0.0f, 0.0f);
    }
    public Vector2 findObstacleRight()
    {

        Vector2 position = this.player.transform.position;
        Vector2 rightForce = new Vector2(1.0f, 0.0f);
        float distance = 2.0f;
        RaycastHit2D hit = Physics2D.CircleCast(this.player.transform.position, 0.5f, rightForce, distance);
        if (hit.collider != null)
        {
            return -rightForce;
        }
        else if (hit.collider == null)
        {
            return new Vector2(0.0f, 0.0f);
        }
        return new Vector2(0.0f, 0.0f);
    }
    public Vector2 findObstacleLeft()
    {
        Vector2 position = this.player.transform.position;
        Vector2 leftForce = new Vector2(-1.0f, 0.0f);
        float distance = 2.0f;
        RaycastHit2D hit = Physics2D.CircleCast(this.player.transform.position, 0.5f, leftForce, distance);
        if (hit.collider != null)
        {
            return -leftForce;
        }
        else if (hit.collider == null)
        {
            return new Vector2(0.0f, 0.0f);
        }
        return new Vector2(0.0f, 0.0f);
    }
    public Vector2 findObstacleDown()
    {

        Vector2 downForce = new Vector2(0.0f, -1.0f);
        float distance = 2.0f;
        RaycastHit2D hit = Physics2D.CircleCast(this.player.transform.position, 0.5f, downForce, distance);
        if (hit.collider != null)
        {
            return -downForce;

        }
        else if (hit.collider == null)
        {
            return new Vector2(0.0f, 0.0f);
        }
        return new Vector2(0.0f, 0.0f);
    }

    // Start is called before the first frame update

    // Update is called once per frame
    void FixedUpdate()
        {
            this.collisionAvoidance();
        }
    }
