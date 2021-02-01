using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeScript : MonoBehaviour
{
    public Text timerText;
    public bool timerStart;
    public float timerValue = 0f;
    Text TotalTimeText;
    GameManager GameManager;
    HighScoreScript ScoreController;

    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("Scripts").GetComponent<GameManager>();
        ScoreController = GameObject.Find("PlayerDetails").GetComponent<HighScoreScript>();
        timerText = GameObject.Find("Time").GetComponent<Text>();
        StartCoroutine(timer());
    }

    IEnumerator timer()
    {
        while (true)
        {
            if (timerStart)
            {
                timerValue++;
                float minutes = timerValue / 60f;
                float seconds = timerValue % 60f;
                timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
                ScoreController.score -= GameManager.scoreTimeMultiplier;
                Debug.Log("Score: " + ScoreController.score);
                yield return new WaitForSeconds(1f);
            }

            else
            {
                timerValue = 0f;
                timerText.text = string.Format("{0:00}:{1:00}", 0f, 0f);
                yield return null;
            }

            if (SceneManager.GetActiveScene().name == "Win")
            {
                TotalTimeText = GameObject.Find("TimeText").GetComponent<Text>();
                string time = timerText.text;
                TotalTimeText.text = "Total Time: " + time;
                Debug.Log(timerText.text);
                Destroy(this.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
