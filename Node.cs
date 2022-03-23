using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

    public int X;
    public int Y;
    public bool hasCollider;
    public Node parent;
    public int G;
    public int H;
    public float Distance;

    public Node(bool hasCollider, int X, int Y) {
        this.hasCollider = hasCollider;
        this.X = X;
        this.Y = Y;
    }
    
}
