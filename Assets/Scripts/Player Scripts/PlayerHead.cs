using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHead : MonoBehaviour
{
    public TextMeshProUGUI scoreIncrementText;
    public TextMeshProUGUI uninfectedAmountText;
    public GameObject gameOverMenu;
    public TextMeshProUGUI finalScoreText;
    public GameObject victoryScreen;

    public GameObject covidAgent2;
    public GameObject covidAgent3;

    private bool setCovid2Active = false;
    private bool setCovid3Active = false;


    private int infectedCount;
    private int uninfectedLeft;

    // Start is called before the first frame update
    void Start()
    {
        infectedCount = 0;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("uninfectedCivillian");

        uninfectedLeft = enemies.Length;

        SetCountText();

    }

    // Update is called once per frame

    private void Update()
    {
        if (uninfectedLeft <= 0)
        {
            this.gameObject.SetActive(false);
            victoryScreen.SetActive(true);
            Time.timeScale = 0;
        }

        if (infectedCount >= 10 && setCovid2Active == false)
        {
            setCovid2Active = true;
            covidAgent2.SetActive(true);
        }

        if (infectedCount >= 25 && setCovid3Active == false)
        {
            setCovid3Active = true;
            covidAgent3.SetActive(true);
        }
    
    }

    void SetCountText()
    {
        scoreIncrementText.text = "Infected Score: " + infectedCount.ToString();

        uninfectedAmountText.text = "Uninfected Left: " + uninfectedLeft.ToString();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("uninfectedCivillian"))
        {
            other.gameObject.SetActive(false);
            infectedCount++;
            uninfectedLeft--;

            SetCountText();
        }

        if (other.gameObject.tag == "AntiCovidAgent")
        {
            this.gameObject.SetActive(false);
            gameOverMenu.SetActive(true);
            finalScoreText.text = "Total Infected: " + infectedCount.ToString();
            Time.timeScale = 0;
        }
    }
}
