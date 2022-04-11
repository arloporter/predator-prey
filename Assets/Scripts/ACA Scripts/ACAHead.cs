using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACAHead : MonoBehaviour
{
    public GameObject player;
    public float playerSightDistance;

    // private MonoBehaviour ENTERSCRIPTNAME;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // GETCOMPONENT HERE
    }


    // So, what will be done here is the Vector2 Magnitude calculation, determining the distance between both the player and the script owner(In this case the Anti-Covid Agent)
    // As the game plays, the code will enable/disable scripts to facilitate with the ease and similarity of a typical FSM.
    // Iterating depending on what is occuring within the game.
    private void Update()
    {
        //float distance = Vector2.Distance(player.transform.position, rb.transform.position);
        //print(distance);
        //if(distance < playerSightDistance)
        //{
        //    boidsBehaviour.enabled = false;
        //}
    }
}
