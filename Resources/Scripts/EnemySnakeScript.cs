using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemySnakeScript : MonoBehaviour
{
    List<Transform> tailPositions = new List<Transform>();
    bool didSnakeEat = false;
    public GameObject tailPrefab;
    public GameObject TargetPlayer;
    public int snakeLength = 1;
    public GameManager GameManager;
    public List<Vector3> pastPositions;

    // Start is called before the first frame update
    void Start()
    {
        tailPrefab = Resources.Load<GameObject>("Prefabs/EnemyTail");
        pastPositions = new List<Vector3>();
        GameManager = GameObject.Find("Scripts").GetComponent<GameManager>();
        snakeLength = 3;

        for (int i = 0; i < snakeLength; i++)
        {
            GameObject g = Instantiate(tailPrefab, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + (i + 1f), 0f), Quaternion.identity);
            g.GetComponent<SpriteRenderer>().color = new Vector4(0.79f, 0.54f, 0.22f, 1f);
            tailPositions.Insert(tailPositions.Count, g.transform);
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Food"))
        {
            didSnakeEat = true;
            Destroy(coll.gameObject);
        }

        else if (coll.gameObject.CompareTag("Tail") || coll.gameObject.CompareTag("Player"))
        {
            GameManager.ClearLevel();
            GameManager.SpawnPlayer();
        }
    }

    public void MoveEnemyTail()
    {
        Vector2 v = transform.position;

        if (tailPositions.Count > 0)
        {
            tailPositions.Last().position = pastPositions[pastPositions.Count - 1];
            tailPositions.Insert(0, tailPositions.Last());
            tailPositions.RemoveAt(tailPositions.Count - 1);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (didSnakeEat)
        {
            GameObject g = Instantiate(tailPrefab, pastPositions[pastPositions.Count - 1], Quaternion.identity);
            g.GetComponent<SpriteRenderer>().color = new Vector4(0.79f, 0.54f, 0.22f, 1f);
            tailPositions.Insert(tailPositions.Count, g.transform);
            snakeLength++;
            didSnakeEat = false;
        }
    }
}
