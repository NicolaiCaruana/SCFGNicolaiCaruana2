using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class HighScore
{
    public string playername;
    public float playtime;

    public HighScore(string pname, float ptime)
    {
        playername = pname;
        playtime = ptime;
    }
}

public class HighScoreScript : MonoBehaviour
{
    List<HighScore> myHighScores;
    public string Name = "";
    public float score = 0f;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        myHighScores = new List<HighScore>();
        LoadList();
    }

    void RenderList()
    {
        Text ScoreList = GameObject.Find("HighScoreText").GetComponent<Text>();
        List<HighScore> OrderedScores = myHighScores.OrderByDescending(item => item.playtime).ToList();

        foreach (HighScore s in OrderedScores)
        {
            ScoreList.text += s.playername + ": " + s.playtime + "\n\n";
        }
    }

    void DebugDisplayList()
    {
        List<HighScore> OrderedScores = myHighScores.OrderByDescending(item => item.playtime).ToList();
        foreach (HighScore s in OrderedScores)
        {
            Debug.Log(s.playername + " " + s.playtime);
        }
    }

    void SaveList()
    {
        string[] names = new string[myHighScores.Count];
        float[] playertimes = new float[myHighScores.Count];

        int counter = 0;
        foreach (HighScore s in myHighScores)
        {
            names[counter] = s.playername;
            playertimes[counter] = s.playtime;
            counter++;
        }

        PlayerPrefsX.SetStringArray("PlayerNames", names);
        PlayerPrefsX.SetFloatArray("PlayerTimes", playertimes);
    }

    void DeleteList()
    {
        Debug.Log("Deleting List");
        PlayerPrefs.DeleteAll();
    }

    void LoadList()
    {
        string[] names;
        float[] playertimes;

        names = PlayerPrefsX.GetStringArray("PlayerNames");
        playertimes = PlayerPrefsX.GetFloatArray("PlayerTimes");

        for (int i = 0; i < names.Length; i++)
        {
            myHighScores.Add(new HighScore(names[i], playertimes[i]));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SaveList();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            LoadList();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            DebugDisplayList();
        }

        if (Input.GetKeyDown(KeyCode.F12))
        {
            DeleteList();
        }

        if (SceneManager.GetActiveScene().name == "Win")
        {
            myHighScores.Add(new HighScore(Name, score));
            SaveList();
            myHighScores.Clear();
            LoadList();
            DebugDisplayList();
            RenderList();
            Destroy(this.gameObject);
        }
    }
}