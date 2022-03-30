using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(UninfectedCivillian))]
public class Cohesion : MonoBehaviour
{

    private UninfectedCivillian uninfectedCivillian;
    // Start is called before the first frame update
    void Start()
    {
        uninfectedCivillian = GetComponent<UninfectedCivillian>();
    }

    // Update is called once per frame
    void Update()
    {
        var unInfectedCivillianGroup = FindObjectOfType<UninfectedCivillian>();
        var average = Vector3.zero;
        var found = 0;

    }
}
