using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    Text scoreText;
    public GameObject player;
    GameObject playerHead;
    GameObject foodPrefab;
    public GameObject enemyPrefab;
    public SnakeScript snake;
    public float scoreTimeMultiplier = 10f;
    HighScoreScript score_controller;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GameObject.Find("Score").GetComponent<Text>();
        score_controller = GameObject.Find("PlayerDetails").GetComponent<HighScoreScript>();
        playerHead = Resources.Load<GameObject>("Prefabs/PlayerSnake");
        foodPrefab = Resources.Load<GameObject>("Prefabs/Food");
        enemyPrefab = Resources.Load<GameObject>("Prefabs/EnemySnake");
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        if (!SceneManager.GetActiveScene().name.Equals("Start") || !SceneManager.GetActiveScene().name.Equals("Win"))
        {
            player = Instantiate(playerHead, GameObject.Find("Start").transform.position, Quaternion.identity);
            player.name = "Player";
            snake = player.GetComponent<SnakeScript>();
            if (SceneManager.GetActiveScene().name.Equals("Lvl2") || SceneManager.GetActiveScene().name.Equals("Lvl3"))
            {
                StartCoroutine(FoodSwap());
            }
        }
    }

    public IEnumerator FoodSwap()
    {
        yield return new WaitForSeconds(3f);
        GameObject[] foodList = GameObject.FindGameObjectsWithTag("Food");
        if (foodList.Length > 0)
        {
            List<GameObject> foodsBehindSnake = new List<GameObject>();
            foreach (GameObject food in foodList)
            {
                Vector3 forward = transform.TransformDirection(Vector3.up);
                Vector3 DirectiontoFood = food.transform.position - transform.position;
                if (0 < (Vector3.Dot((snake.tailPositions.First().position - transform.position), (food.transform.position - transform.position))))
                {
                    foodsBehindSnake.Add(food);
                }
            }
            List<GameObject> sortedPositions = foodsBehindSnake.OrderBy(x => Vector2.Distance(transform.position, x.transform.position)).ToList();

            Vector3 enemySpawnPosition = sortedPositions.First().transform.position;
            Destroy(sortedPositions.First().gameObject);
            GameObject newSnake = Instantiate(enemyPrefab, enemySpawnPosition, Quaternion.identity);
        }

        yield return null;
    }

    bool CanSpawnFood(Vector2 Location)
    {
        int SearchAttempts = 3;
        List<Vector2> obstaclePositions = new List<Vector2>();
        GameObject[] obstacles = (GameObject.FindGameObjectsWithTag("Obstacle"));
        if (obstacles.Length != 0)
        {
            foreach (GameObject obs in obstacles)
            {
                obstaclePositions.Add(obs.transform.position);
            }

            List<Vector2> sortedPositions = obstaclePositions.OrderBy(x => Vector2.Distance(Location, x)).ToList();
            for (int i = 0; i < SearchAttempts; i++)
            {
                if (Vector2.Distance(Location, sortedPositions[0]) < 1f)
                {
                    SearchAttempts--;
                }

                else
                {
                    return true;
                }
            }
            return false;
        }

        else
        {
            return true;
        }
    }

    public void ClearLevel()
    {
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        GameObject[] tail_parts = GameObject.FindGameObjectsWithTag("Tail");
        foreach (GameObject tailpart in tail_parts)
        {
            Destroy(tailpart);
        }

        score_controller.score -= (score_controller.score * 0.5f);
        GameObject[] seekers = GameObject.FindGameObjectsWithTag("Seeker Overlay");
        foreach (GameObject seeker in seekers)
        {
            Destroy(seeker);
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score_controller.score.ToString();
        if (snake.movesUntilFoodSpawn == 0)
        {
            Vector3 randLoc = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0f);
            if (CanSpawnFood(randLoc))
            {
                Instantiate(foodPrefab, randLoc, Quaternion.identity);
            }
            snake.movesUntilFoodSpawn = 4;
        }

        if (snake.snakeLength >= 0)
        {
            GameObject exitPoint = GameObject.Find("End");
            exitPoint.GetComponent<SpriteRenderer>().color = Color.blue;
            BoxCollider2D exitCollider = exitPoint.GetComponent<BoxCollider2D>();
            exitCollider.enabled = true;
        }

        else
        {
            GameObject exitPoint = GameObject.Find("End");
            exitPoint.GetComponent<SpriteRenderer>().color = Color.red;
            BoxCollider2D exitCollider = exitPoint.GetComponent<BoxCollider2D>();
            exitCollider.enabled = false;
        }
    }
}
