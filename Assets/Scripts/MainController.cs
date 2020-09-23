using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainController : MonoBehaviour
{
    public enum STEP {
        NONE = -1, 
        WAIT,
        PLAY,  
        NUM, 
    }
    public int score;
    public Text scoreText;

    void Start()
    {
        
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) {

        }
    }

    public void AddScore() {
        score++;
        scoreText.text = "SCORE: " + score.ToString();
    }
}
