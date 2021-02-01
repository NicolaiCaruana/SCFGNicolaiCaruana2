using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NameRegister : MonoBehaviour
{
    public GameObject nameInputField, registerButton;
    string Name = "";
    HighScoreScript score_controller;
    // Start is called before the first frame update
    void Start()
    {
        nameInputField = GameObject.Find("NText");
        registerButton = GameObject.Find("StrtButton");
        score_controller = GameObject.Find("PlayerDetails").GetComponent<HighScoreScript>();
        registerButton.GetComponent<Button>().onClick.AddListener(registerButtonPressed);
    }

    void registerButtonPressed()
    {
        Name = nameInputField.GetComponent<Text>().text.ToString();
        score_controller.Name = Name;
        SceneManager.LoadScene("Lvl1");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
