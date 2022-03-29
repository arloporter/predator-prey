using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UninfectedCivillian : MonoBehaviour
{












    // Basic Collider code for testing purposes
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
            this.gameObject.SetActive(false);
    }
}
