using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class SnakeScript : MonoBehaviour
{
    public List<Transform> tailPositions = new List<Transform>();
    bool didSnakeEat = false;
    public GameObject tailPrefab;
    HighScoreScript score_controller;
    TimeScript TimeManager;
    public int movesUntilFoodSpawn = 4;
    public int snakeLength = 1;
    List<Vector3> pastPositions;
    GameManager GameManager;

    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("Scripts").GetComponent<GameManager>();
        pastPositions = new List<Vector3>();
        score_controller = GameObject.Find("PlayerDetails").GetComponent<HighScoreScript>();
        TimeManager = GameObject.Find("Canvas").GetComponent<TimeScript>();

        if (SceneManager.GetActiveScene().name == "Lvl2")
        {
            snakeLength = 5;
        }

        if (SceneManager.GetActiveScene().name == "Lvl3")
        {
            snakeLength = 1;
        }

        for (int i = 0; i < snakeLength; i++)
        {
            GameObject g = Instantiate(tailPrefab, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + (i + 1), 0f), Quaternion.identity);

            // Keep track of it in our tail list
            tailPositions.Insert(tailPositions.Count, g.transform);
        }
    }

    void MoveTail()
    {
        Vector2 v = transform.position;

        //if tail is longer than 0
        if (tailPositions.Count > 0)
        {
            //Last Tail element move to prevoius head position
            tailPositions.Last().position = pastPositions[pastPositions.Count - 1];

            //Add tail element to front of list, remove from back
            tailPositions.Insert(0, tailPositions.Last());
            tailPositions.RemoveAt(tailPositions.Count - 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        //check for food
        if (coll.gameObject.CompareTag("Food"))
        {
            didSnakeEat = true;
            Destroy(coll.gameObject);
        }

        else if (coll.gameObject.CompareTag("Exit"))
        {
            if (SceneManager.GetActiveScene().name == "Lvl1")
            {
                SceneManager.LoadScene("Lvl2");
            }

            if (SceneManager.GetActiveScene().name == "Lvl2")
            {
                SceneManager.LoadScene("Lvl3");
            }

            if (SceneManager.GetActiveScene().name == "Lvl3")
            {
                SceneManager.LoadScene("Win");
            }
        }

        else if (coll.gameObject.CompareTag("Tail") || coll.gameObject.CompareTag("Obstacle") || coll.gameObject.CompareTag("Enemy"))
        {
            GameManager.ClearLevel();
            snakeLength = 1;
            GameManager.SpawnPlayer();
        }
    }

    void checkBounds()
    {
        if ((transform.position.x < -(Camera.main.orthographicSize)) || (transform.position.x > (Camera.main.orthographicSize)))
        {
            transform.position = new Vector3(-transform.position.x, transform.position.y);
        }

        if ((transform.position.y < -(Camera.main.orthographicSize)) || (transform.position.y > (Camera.main.orthographicSize)))
        {
            transform.position = new Vector3(transform.position.x, -transform.position.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            pastPositions.Add(transform.position);
            transform.Translate(new Vector3(-1f, 0));
            MoveTail();

            movesUntilFoodSpawn--;
            checkBounds();

        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            pastPositions.Add(transform.position);
            transform.Translate(new Vector3(1f, 0));
            MoveTail();

            movesUntilFoodSpawn--;
            checkBounds();

        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            pastPositions.Add(transform.position);
            transform.Translate(new Vector3(0, 1f));
            MoveTail();
            movesUntilFoodSpawn--;
            checkBounds();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            pastPositions.Add(transform.position);
            transform.Translate(new Vector3(0, -1f));
            MoveTail();
            movesUntilFoodSpawn--;
            checkBounds();
        }

        if (didSnakeEat)
        {
            GameObject g = Instantiate(tailPrefab, pastPositions[pastPositions.Count - 1], Quaternion.identity);
            tailPositions.Insert(tailPositions.Count, g.transform);
            snakeLength++;
            score_controller.score += 100;
            didSnakeEat = false;
        }
    }
}