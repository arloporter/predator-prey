using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICHead : MonoBehaviour
{
    // public GameObject player;
    // public float playerSightDistance;

    // private MonoBehaviour boidsBehaviour;
    // private Rigidbody2D rb;

    private void Start()
    {
        // rb = GetComponent<Rigidbody2D>();
        // boidsBehaviour = GetComponent<WanderingBehaviour>();
    }

    // Basic Collider code for testing purposes
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
            this.gameObject.SetActive(false);
    }

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
