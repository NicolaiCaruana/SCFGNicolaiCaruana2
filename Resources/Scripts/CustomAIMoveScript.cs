using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Pathfinding;

public class CustomAIMoveScript : MonoBehaviour
{
    Seeker seeker;
    Path pathToFollow;
    public Transform target;
    public EnemySnakeScript enemySnake;
    GameObject graphParent;
    public float enemyMovementDelay;
    public List<Transform> obstacleNodes;
    GameObject seekerCrumb;
    GameObject seekerMapPrefab;
    GameObject seekerMapOverlay;
    public GameObject sMap;
    public bool isSeekerModeOn = false;

    // Start is called before the first frame update
    void Start()
    {
        seekerMapOverlay = Resources.Load<GameObject>("Prefabs/SeekerMapOverlay");
        seekerMapPrefab = Resources.Load<GameObject>("Prefabs/SeekerMap");
        seekerCrumb = Resources.Load<GameObject>("Prefabs/SeekerCrumb");
        enemySnake = this.gameObject.GetComponent<EnemySnakeScript>();
        target = GameObject.Find("Player").GetComponent<SnakeScript>().tailPositions.Last().gameObject.transform;
        seeker = GetComponent<Seeker>();
        sMap = Instantiate<GameObject>(seekerMapPrefab, new Vector3(0, 0, 5), Quaternion.identity);
        sMap.name = "Seeker Map";
        sMap.SetActive(false);
        GameObject Overlay = Instantiate<GameObject>(seekerMapOverlay, new Vector3(0, 0, 0), Quaternion.identity);
        Overlay.transform.SetParent(sMap.transform);
        graphParent = GameObject.Find("AStarGrid");
        graphParent.GetComponent<AstarPath>().Scan();
        pathToFollow = seeker.StartPath(transform.position, target.position);
        StartCoroutine(updateGraph());
        StartCoroutine(moveTowardsEnemy(this.transform));
    }

    IEnumerator updateGraph()
    {
        while (true)
        {
            graphParent.GetComponent<AstarPath>().Scan();
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        List<Vector3> posns = pathToFollow.vectorPath;
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (isSeekerModeOn == false)
            {
                Debug.Log("SEEKER DEBUG MODE ON");
            }

            else if (isSeekerModeOn == true)
            {
                Debug.Log("SEEKER DEBUG MODE OFF");
            }
            isSeekerModeOn = !isSeekerModeOn;
            sMap.SetActive(isSeekerModeOn);
        }

        GameObject[] seekers = GameObject.FindGameObjectsWithTag("Seeker");
        foreach (GameObject seeker in seekers)
        {
            Destroy(seeker);
        }

        foreach (Vector3 pos in posns)
        {
            GameObject sCrumb = Instantiate(seekerCrumb, pos, Quaternion.identity);
            sCrumb.transform.SetParent(sMap.transform);
        }
    }

    IEnumerator moveTowardsEnemy(Transform t)
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            List<Vector3> posns = pathToFollow.vectorPath;
            Debug.Log("Positions Count: " + posns.Count);

            for (int counter = 0; counter < posns.Count; counter++)
            {
                if (posns[counter] != null)
                {
                    while (Vector3.Distance(t.position, posns[counter]) >= 0.5f)
                    {
                        enemySnake.pastPositions.Add(transform.position);
                        t.position = Vector3.MoveTowards(t.position, posns[counter], 1f);
                        pathToFollow = seeker.StartPath(t.position, target.position);
                        yield return seeker.IsDone();
                        posns = pathToFollow.vectorPath;
                        enemySnake.MoveEnemyTail();
                        yield return new WaitForSeconds(enemyMovementDelay);
                    }
                }
                pathToFollow = seeker.StartPath(t.position, target.position);
                yield return seeker.IsDone();
                posns = pathToFollow.vectorPath;
            }
            yield return null;
        }
    }
}
