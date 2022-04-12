using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Node;
using static PriorityQueue<Node>;

public class AnticovidAgent : MonoBehaviour

{
    public float maxSizeX;
    public float maxSizeY;
    public float offset;
    public float maxSpeed;
    public List<List<Node>> grid;
    public Rigidbody2D player;
    public Rigidbody2D target;
    private List<Vector2> finalPath;

    public GameObject gridNode;
    public GameObject gridRed;
    public GameObject gridBlack;
    private int count;
    public LayerMask Collidables;
    public LayerMask Costmap;
    
    // maps bottom left corner must start at 0,0.
    // starting at 0,0 creates a grid with offset-spaced nodes.

    public void initialise()
    {
        
        int yIndex = 0;
        this.grid = new List<List<Node>>();
        // from 0 to N rows
        for (float y = 0.0f; y < this.maxSizeY; y += this.offset)
        {
            int xIndex = 0;
            List<Node> row = new List<Node>();
            //from 0 to N columns
            for (float x = 0.0f; x < this.maxSizeX; x += this.offset)
            {
                // establish a new node at this location
                Node n = new Node(false, x, y);
                n.xMapIndex = xIndex;
                n.yMapIndex = yIndex;

                // determine if there is a collider at point
                Vector2 point = new Vector2(x, y);
                Vector2 NullVector = new Vector2(1, 1);
                Collider2D collider = Physics2D.OverlapBox(point, new Vector2(0.5f, 0.5f), 0f, Collidables);
                if (collider != null)
                {
                    n.hasCollider = true;
                    
                }
                else
                {
                    n.hasCollider = false;
                    n.cost = 1;
                }
                

                // Assign to grid
                row.Add(n);

                // uncomment to visualize grid
                /*if(n.hasCollider == true)
                {
                    Instantiate(this.gridBlack, new Vector2(x, y), Quaternion.identity);
                } else
                {
                    Instantiate(this.gridNode, new Vector2(x, y), Quaternion.identity);
                }
                */
                xIndex += 1;
            }
            this.grid.Add(row);
            yIndex += 1;
        }
        
    }

    // deduce the index from coordinate/offset
    public Node findClosestNode(float x, float y)
    {
        int IndexX = (int)Math.Floor(x / this.offset);
        int IndexY = (int)Math.Floor(y / this.offset);
        Node closestNode = this.grid[IndexY][IndexX];
        return closestNode;
    }


    public Vector2 transitionFunction(Node start, Node target)
    {
        Vector2 startVector = new Vector2(start.X, start.Y);
        Vector2 targetVector = new Vector2(target.X, target.Y);
        Vector2 velocity = targetVector - startVector;
        return velocity;
    }

    // find neighboring nodes from self.grid and store action required to get to them as action attribute
    // returns list of neighbors
    public List<Node> findNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();
        if (node.yMapIndex < (this.maxSizeY-1))
        {
            Node up = this.grid[node.yMapIndex + 1][node.xMapIndex];
            if (!up.hasCollider)
            {
                up.action = transitionFunction(node, up);
                neighbors.Add(up);
            }
        }

        if (node.yMapIndex > 0) 
        {
            Node down = this.grid[node.yMapIndex - 1][node.xMapIndex];
            if (!down.hasCollider)
            { 
                down.action = transitionFunction(node, down);
                neighbors.Add(down);
            }
        }

        if(node.xMapIndex > 0)
        {
            Node left = this.grid[node.yMapIndex][node.xMapIndex - 1];
            if (!left.hasCollider)
            {
                left.action = transitionFunction(node, left);
                neighbors.Add(left);
            }
        }

        if(node.xMapIndex < (this.maxSizeX-1))
        {
            Node right = this.grid[node.yMapIndex][node.xMapIndex + 1];
            if (!right.hasCollider)
            {
                right.action = transitionFunction(node, right);
                neighbors.Add(right);
            }
        }

        if(node.xMapIndex < (this.maxSizeX-1) && node.yMapIndex < (this.maxSizeY-1))
        {
            Node upRight = this.grid[node.yMapIndex + 1][node.xMapIndex + 1];
            if (!upRight.hasCollider)
            {
                upRight.action = transitionFunction(node, upRight);
                neighbors.Add(upRight);
            }
        }

        if(node.xMapIndex > 0 && node.yMapIndex < (this.maxSizeY-1))
        {
            Node upLeft = this.grid[node.yMapIndex + 1][node.xMapIndex - 1];
            if (!upLeft.hasCollider)
            {
                upLeft.action = transitionFunction(node, upLeft);
                neighbors.Add(upLeft);
            }
        }
         
        if(node.xMapIndex < (this.maxSizeX-1) && node.yMapIndex > 0)
        {
            Node downRight = this.grid[node.yMapIndex - 1][node.xMapIndex + 1];
            if (!downRight.hasCollider)
            {
                downRight.action = transitionFunction(node, downRight);
                neighbors.Add(downRight);
            }
        }
        
        if(node.xMapIndex > 0 && node.yMapIndex > 0)
        {
            Node downLeft = this.grid[node.yMapIndex - 1][node.xMapIndex - 1];
            if (!downLeft.hasCollider)
            {
                downLeft.action = transitionFunction(node, downLeft);
                neighbors.Add(downLeft);
            }
        }
        
        return neighbors;
    }

    /* OPEN = priority queue containing START
       CLOSED = empty set
        while lowest rank in OPEN is not the GOAL:
            current = remove lowest rank item from OPEN
            add current to CLOSED
            for neighbors of current:
                    cost = g(current) + movementcost(current, neighbor)
                    if neighbor in OPEN and cost less than g(neighbor):
                        remove neighbor from OPEN, because new path is better
                    if neighbor in CLOSED and cost less than g(neighbor): ⁽²⁾
                        remove neighbor from CLOSED
                    if neighbor not in OPEN and neighbor not in CLOSED:
                        set g(neighbor) to cost
                        add neighbor to OPEN
                        set priority queue rank to g(neighbor) + h(neighbor)
                        set neighbor's parent to current

        reconstruct reverse path from goal to start by following parent pointers
        https://theory.stanford.edu/~amitp/GameProgramming/ImplementationNotes.html
    */

    public void AStarSearch(Vector2 Start, Vector2 Target, Rigidbody2D entity)
    {
        
        HashSet<Node> visited = new HashSet<Node>();
        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        Node startNode = findClosestNode((float) Start.x, (float) Start.y);
        startNode.cost = 0;

        Vector2 startVector = new Vector2(startNode.X, startNode.Y);
        Vector2 position = entity.position;
        Vector2 goToStart = startVector - entity.position;
        startNode.action = goToStart;
        Node targetNode = findClosestNode((float) Target.x, (float) Target.y);
        frontier.Enqueue(startNode);
        Node current = startNode;
        

        while (frontier.getList().Count > 0)
        {
            if (!visited.Contains(current)) {
                visited.Add(current);
                
                if ((current.xMapIndex == targetNode.xMapIndex) && (current.yMapIndex == targetNode.yMapIndex))
                {
                    this.reconstructPath(startNode, targetNode);
                    break;
                }
                else
                {
                    foreach (Node neighbor in findNeighbors(current))
                    {
                        if (!visited.Contains(neighbor) &&
                            !frontier.getList().Contains(neighbor))
                        {
                            neighbor.cost = current.cost +1;
                            Collider2D CostCollider = Physics2D.OverlapBox(new Vector2(neighbor.X, neighbor.Y), new Vector2(0.5f, 0.5f), 0f, this.Costmap);
                            if (CostCollider != null)
                            {
                                neighbor.cost = current.cost + 5;
                            }
                            neighbor.parent = current;
                            Vector2 neighborVector = new Vector2(neighbor.X, neighbor.Y);
                            

                            // Euclidean
                            double distance = Math.Sqrt((Math.Pow((neighborVector.x - Target.x), 2) + Math.Pow((neighborVector.y- Target.y), 2)));
                            
                            // manhattan
                            //double distance = Math.Abs(neighborVector.x - Target.x) + Math.Abs(neighborVector.y - Target.y);
                            neighbor.heuristic = distance;
                            frontier.Enqueue(neighbor);
                        } 
                    }
                }
            }
            current = frontier.Dequeue();
        }
        Debug.Log("failed");
        
    }
    
    public void reconstructPath(Node startNode, Node targetNode)
    {
        finalPath = new List<Vector2>();
        finalPath.Add(new Vector2(targetNode.X, targetNode.Y));
        while (targetNode.parent != startNode)
        {
            
            finalPath.Add(new Vector2(targetNode.parent.X, targetNode.parent.Y));
            // Destroy(Instantiate(this.gridRed, new Vector2(targetNode.X, targetNode.Y), Quaternion.identity), 0.5f);
            targetNode = targetNode.parent;
        }
        finalPath.Reverse();
        this.finalPath = finalPath;

    }
    void Awake()
    {
        this.initialise();
        AStarSearch(this.player.transform.position, this.target.transform.position, this.player);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.count += 1;
        float distanceCheck = Vector2.Distance(this.player.transform.position, this.target.transform.position);
        if (this.count % 20 == 0 && distanceCheck > 0.2f)
        {
            AStarSearch(this.player.transform.position, this.target.transform.position, this.player);
        }

        if (this.finalPath.Count > 0)
        {
            List<Vector2> smoothedPath = new List<Vector2>();
            for(int i = 1; i < this.finalPath.Count-1; i++) {
                Vector2 circlecast = (Vector2)this.player.transform.position - this.finalPath[i];
                float distance = (float) Math.Sqrt(Math.Pow((this.player.transform.position.x - this.finalPath[i].x), 2) + Math.Pow((this.player.transform.position.y - this.finalPath[i].y), 2));
                RaycastHit2D hit = Physics2D.CircleCast(this.player.transform.position, 0.5f, circlecast, distance, this.Collidables);
                if (hit.collider != null)
                {
                    Debug.Log("Hit");
                    smoothedPath.Add(this.finalPath[i]);
                } else if (hit.collider == null)
                {
                    Debug.Log("Clear");
                    this.finalPath.RemoveAt(i);
                   

                    
                }
            }
            Vector2 nextWaypoint = finalPath[0];
            if (smoothedPath.Count > 0)
            {
                nextWaypoint = smoothedPath[0];
            } 
            Vector2 desiredVel = nextWaypoint - (Vector2)this.player.transform.position;
            float dist = desiredVel.magnitude;
            desiredVel.Normalize();
            desiredVel = desiredVel * this.maxSpeed; // Change to max speed
            Vector2 force = desiredVel - this.player.velocity;
            Vector2 velocity = this.player.velocity;
            this.player.AddForce(force);
<<<<<<< Updated upstream
            if(velocity.x > 0)
            {
                this.spriteAnimation.sprite = this.sprites[5];
            }
            if (velocity.x < 0)
            {
                this.spriteAnimation.sprite = this.sprites[4];
            }
            if (velocity.y > 0)
            {
                this.spriteAnimation.sprite = this.sprites[1];
            }
            if (velocity.y < 0)
            {
                this.spriteAnimation.sprite = this.sprites[6];
            }
            if (velocity.x > 0 && velocity.y > 0)
            {
                this.spriteAnimation.sprite = this.sprites[2];
            }
            if (velocity.x < 0 && velocity.y > 0)
            {
                this.spriteAnimation.sprite = this.sprites[0];
            }
            if (velocity.x > 0 && velocity.y < 0)
            {
                this.spriteAnimation.sprite = this.sprites[7];
            }
            if (velocity.x < 0 && velocity.y <0)
            {
                this.spriteAnimation.sprite = this.sprites[3];
            }
=======
>>>>>>> Stashed changes

            //Destroy(Instantiate(this.gridRed, new Vector2(nextWaypoint.x, nextWaypoint.y), Quaternion.identity), 0.5f);
            if (dist < 0.3f && smoothedPath.Count > 0)
            {
                smoothedPath.RemoveAt(0);
            } else if (dist < 0.3f && smoothedPath.Count == 0)
            {
                finalPath.RemoveAt(0);
            }
           



        }

    }
}

// https://www.youtube.com/watch?v=AKKpPmxx07w
//https://docs.unity3d.com/ScriptReference/Physics.CheckSphere.html