using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveObstacle : MonoBehaviour
{
    GameManager GameManager;

    private GameObject obstacle;
    [SerializeField] public int obstacleSteps = 2;
    [SerializeField] public float stepDistance = 1;
    [SerializeField] public float obstacleStepDelay = 0.1f;
    [SerializeField] public enum Movements { Horizontal, Vertical };
    [SerializeField] public Movements MovementType;

    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("Scripts").GetComponent<GameManager>();
        obstacle = this.gameObject;
        StartCoroutine(MovementController());
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Tail"))
        {
            GameManager.ClearLevel();
            GameManager.SpawnPlayer();
        }
    }

    IEnumerator MovementController()
    {
        if (MovementType == Movements.Horizontal)
        {
            while (true)
            {
                for (int i = 0; i < obstacleSteps; i++)
                {
                    obstacle.transform.position += new Vector3(stepDistance, 0f);
                    yield return new WaitForSeconds(obstacleStepDelay);
                }

                for (int i = 0; i < obstacleSteps; i++)
                {
                    obstacle.transform.position -= new Vector3(stepDistance, 0f);
                    yield return new WaitForSeconds(obstacleStepDelay);
                }

                for (int i = 0; i < obstacleSteps; i++)
                {
                    obstacle.transform.position -= new Vector3(stepDistance, 0f);
                    yield return new WaitForSeconds(obstacleStepDelay);
                }

                for (int i = 0; i < obstacleSteps; i++)
                {
                    obstacle.transform.position += new Vector3(stepDistance, 0f);
                    yield return new WaitForSeconds(obstacleStepDelay);
                }
            }
        }

        else if (MovementType == Movements.Vertical)
        {
            while (true)
            {
                for (int i = 0; i < obstacleSteps; i++)
                {
                    obstacle.transform.position += new Vector3(0f, stepDistance);
                    yield return new WaitForSeconds(obstacleStepDelay);
                }

                for (int i = 0; i < obstacleSteps; i++)
                {
                    obstacle.transform.position -= new Vector3(0f, stepDistance);
                    yield return new WaitForSeconds(obstacleStepDelay);
                }

                for (int i = 0; i < obstacleSteps; i++)
                {
                    obstacle.transform.position -= new Vector3(0f, stepDistance);
                    yield return new WaitForSeconds(obstacleStepDelay);
                }

                for (int i = 0; i < obstacleSteps; i++)
                {
                    obstacle.transform.position += new Vector3(0f, stepDistance);
                    yield return new WaitForSeconds(obstacleStepDelay);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
