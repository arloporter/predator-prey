using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Node;
using static PriorityQueue<Node>;

public class Map : MonoBehaviour
{
    public float maxSizeX;
    public float maxSizeY;
    public float offset;
    Node[][] grid;
    public List<Node> kdTree;
    public float radius;
    public LayerMask obstacles;
    public float maxVelocity;
    public Rigidbody2D player;


    // public static RaycastHit2D CircleCast(Vector2 origin, float radius, Vector2 direction, float distance = Mathf.Infinity, 
    //                             int layerMask = DefaultRaycastLayers, float minDepth = -Mathf.Infinity, float maxDepth = Mathf.Infinity);

    public void initialise()
    {
        int xIndex = 0;
        int yIndex = 0;

        // from 0 to N rows
        for (float y = 0; y < this.maxSizeY; y += this.offset)
        {
            
            //from 0 to N columns
            for (float x = 0; x < this.maxSizeX; x += this.offset)
            {
                // establish a new node at this location
                Node n = new Node(false, x, y);
                n.xMapIndex = xIndex;
                n.yMapIndex = yIndex;

                // determine if there is a collider at point
                Vector2 point = new Vector2(x, y);
                Vector2 NullVector = new Vector2(0, 0);
                if(Physics2D.CircleCast(point, this.radius, NullVector , 0.0f)) {
                    n.hasCollider = true;
                } else {
                    n.hasCollider = false;
                }
                n.cost = 0;
                grid[xIndex][yIndex] = n;
                xIndex += 1;

            }
            yIndex += 1;
        }
    }
    // Math.floor(CurrentX/offset), Math.floor(currentY/offset).
    public Node findClosestNode(float x, float y)
    {
        int IndexX = (int) Math.Floor(x/this.offset);
        int IndexY = (int) Math.Floor(y/this.offset);
        return grid[IndexX][IndexY];
    }
    
    public Vector2 transitionFunction(Node start, Node target)
    {
        Vector2 startVector = new Vector2(start.X, start.Y);
        Vector2 targetVector = new Vector2(target.X, target.Y);
        Vector2 velocity = targetVector - startVector;
        return velocity;
    }

    public Node[] findNeighbors(Node node)
    {
        Node up = grid[node.xMapIndex][node.yMapIndex + 1];
        Node down = grid[node.xMapIndex][node.yMapIndex - 1];
        Node left = grid[node.xMapIndex - 1][node.yMapIndex];
        Node right = grid[node.xMapIndex + 1][node.yMapIndex];
        Node upRight = grid[node.xMapIndex + 1][node.yMapIndex + 1];
        Node upLeft  = grid[node.xMapIndex - 1][node.yMapIndex + 1];
        Node downRight = grid[node.xMapIndex + 1][node.yMapIndex - 1];
        Node downLeft = grid[node.xMapIndex - 1][node.yMapIndex - 1];
        up.parent = node;
        up.action = transitionFunction(node, up);
        down.parent = node;
        down.action = transitionFunction(node, down);
        left.parent = node;
        left.action = transitionFunction(node, left);
        right.parent = node;
        right.action = transitionFunction(node, right);
        upRight.parent = node;
        upRight.action = transitionFunction(node, upRight);
        upLeft.parent = node;
        upLeft.action = transitionFunction(node, upLeft);
        downRight.parent = node;
        downRight.action = transitionFunction(node, downRight);
        downLeft.parent = node;
        downLeft.action = transitionFunction(node, downLeft);

        return new Node[] {upLeft, up, upRight, left, right, downLeft, down, downRight};
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

    public List<Vector2> AStarSearch(Vector2 Start, Vector2 Target, Rigidbody2D entity)
    {
        List<Node> path = new List<Node>();
        HashSet<Node> visited = new HashSet<Node>();
        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        Node startNode = findClosestNode(Start.x, Start.y);
        Vector2 startVector = new Vector2(startNode.X, startNode.Y);
        Node targetNode = findClosestNode(Target.x, Target.y);
        Vector2 position = entity.position;
        Vector2 goToStart = startVector - entity.position;
        goToStart = goToStart - entity.velocity;
        path.Add(startNode);
        frontier.Enqueue(startNode);
        Node current = startNode;
        while(current != targetNode)
        {
            current = frontier.Dequeue();
            visited.Add(current);
            foreach (Node neighbor in findNeighbors(current))
            {

            }
        }
        
            
            
        




        List<Vector2> actions = new List<Vector2>();
        foreach(Node n in path)
        {
            actions.Add(n.action);
        }


        return actions;
    }
    
    

    // Start is called before the first frame update
    void Start()
    {
        this.initialise();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
// https://www.youtube.com/watch?v=AKKpPmxx07w
//https://docs.unity3d.com/ScriptReference/Physics.CheckSphere.html