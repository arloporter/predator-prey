using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UninfectedCivillian : MonoBehaviour
{
    public float maxVelocity;
    public Vector3 velocity; // Public for testing Purposes

    private void Update()
    {
        if (velocity.magnitude > maxVelocity)
        {
            velocity = velocity.normalized * maxVelocity;
        }

        this.transform.position += velocity * Time.deltaTime;
        this.transform.rotation = Quaternion.LookRotation(velocity);
    }







    // Basic Collider code for testing purposes
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
            this.gameObject.SetActive(false);
    }
}
