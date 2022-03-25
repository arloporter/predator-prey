using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour { 

    public Transform player; // Get the transform of the player

    // These Integers are designed to keep the camera within the bounds of the Space, Currently is 0->25 X and Y.
    // With a camera size of 10, the camera sees 5 in each direction, therefore the min/Max should be 5-20
    // Z should always be -10
    public float maxY;
    public float minY;
    public float maxX;
    public float minX;
    public float cameraZ;


    void Update()
    {
        // Clamp code
        float cameraY = Mathf.Clamp(player.position.y, minY, maxY);
        float cameraX = Mathf.Clamp(player.position.x, minX, maxX);
        transform.position = new Vector3(cameraX, cameraY, cameraZ);
    }
}

