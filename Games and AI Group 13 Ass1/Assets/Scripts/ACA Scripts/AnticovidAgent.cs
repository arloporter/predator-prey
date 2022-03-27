using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Math;
using Node;

public class Map : MonoBehaviour
{

    public Transform start;
    public float maxSizeX;
    public float maxSizeY;
    public float offset;
    Node[][] grid;
    public list<Node> kdTree;
    public list<Node> path;
    public int radius;
    public LayerMask obstacles;



    // public static RaycastHit2D CircleCast(Vector2 origin, float radius, Vector2 direction, float distance = Mathf.Infinity, 
    //                             int layerMask = DefaultRaycastLayers, float minDepth = -Mathf.Infinity, float maxDepth = Mathf.Infinity);
    
    public void initialise()
    {
        // from 0 to N rows
        for(float i = 0; i < this.maxSizeX[0]; i+=this.offset){
            //from 0 to N columns
            for(float j = 0; j < this.maxSizeY[1], i+= this.offset){
                // establish a new node at this location
                grid[i][j] = new Node(false, i, j);
                // determine if there is a collider at point
                Vector2 point = new Vector2(i, j);
                if CircleCast(point, this.radius, new Vector2(0, 0), 0.0) {
                    grid[i][j].hasCollider = true;
                }

            }
        }
    }

    public Node findClosestNode(int x, int y) {
        Vector2 point = new Vector2(x, y);


        foreach(Node[] N: this.grid) {
            foreach(Node node: N) {
                if(node.hasCollider == false) {
                    float euclideanDist = Math.Sqrt((point.x - node.x)^2 + (point.y - node.y)^2);
                    //if
                    minimum.add(node, euclideanDist) {

                    }

                }
            }

        }

    }
    public void transitionFunction() {

    }
    public void findNeighbors(int x, int y) {

    }

    // Start is called before the first frame update
    void Start()
    {
        this.initialise()
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
// https://www.youtube.com/watch?v=AKKpPmxx07w
//https://docs.unity3d.com/ScriptReference/Physics.CheckSphere.html
