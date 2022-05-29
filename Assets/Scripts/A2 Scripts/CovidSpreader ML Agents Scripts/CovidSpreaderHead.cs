using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CovidSpreaderHead : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "uninfectedCivillian")
        {
            other.gameObject.SetActive(false);
            this.gameObject.GetComponent<CovidSpreaderAgentKanika>().HandleCollectionUninfectedCivillian();
        }
        if (other.gameObject.tag == "AntiCovidAgent")
        {
            other.gameObject.SetActive(false);
            this.gameObject.GetComponent<CovidSpreaderAgentKanika>().HandleHitAntiCovidAgent();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

    }
}
