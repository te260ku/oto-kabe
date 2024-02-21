using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainController : MonoBehaviour
{
    [SerializeField] MusicController musicController;
    [SerializeField] TextMeshProUGUI scoreText;
    public enum GAME_STATE {
        IDLE,
        PLAY,  
        NUM, 
    }
    
    GAME_STATE gameState = GAME_STATE.IDLE;
    public GAME_STATE GameState {
        get { return gameState; }
    }
    int score;

    void Start()
    {
        
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            StartGame();
        }
    }

    void StartGame() {
        musicController.StartGame();
        gameState = GAME_STATE.PLAY;
    }

    public void AddScore() {
        score ++;
        scoreText.text = score.ToString();
    }

}
