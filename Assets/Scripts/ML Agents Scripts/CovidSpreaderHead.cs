using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CovidSpreaderHead : MonoBehaviour
{
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "uninfectedCivillian")
        {
            other.gameObject.GetComponent<CovidSpreaderAgent>().HandleCollectionUninfectedCivillian();
        }
        if(other.gameObject.tag == "AntiCovidAgent")
        {
            other.gameObject.GetComponent<CovidSpreaderAgent>().HandleHitAntiCovidAgent();
        }
    }
}
