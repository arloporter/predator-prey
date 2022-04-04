using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IComparable<Node>
{

    public float X;
    public float Y;
    public int xMapIndex;
    public int yMapIndex;
    public bool hasCollider;
    public Node parent;
    public Vector2 action;
    public int cost;
    public double heuristic;
    public double priority;

    public Node(bool hasCollider, float X, float Y)
    {
        this.hasCollider = hasCollider;
        this.X = X;
        this.Y = Y;
    }

    public int CompareTo(Node n)
    {
        this.priority = this.cost + this.heuristic;
        n.priority = n.cost + n.heuristic;
        if (this.priority == n.priority)
        {
            return 0;
        }
        else if (this.priority > n.priority)
        {
            return -1;
        }
        return 1;
    }
    public int Compare(Node x, Node y)
    {
        x.priority = x.cost + x.heuristic;
        y.priority = y.cost + y.heuristic;
        if (x.priority == y.priority)
        {
            return 0;
        }
        else if (x.priority < y.priority)
        {
            return -1;
        }
        return 1;
    }

}
