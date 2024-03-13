using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainController : MonoBehaviour
{
    [SerializeField] MusicController musicController;
    [SerializeField] HandGestureDetector handGestureDetector;
    [SerializeField] BlockController blockController;
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
    bool handGestureDetected;

    void Start()
    {
        
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            StartGame();
        }

        if (!handGestureDetected) {
            if (handGestureDetector.CurrentHandGesture == HandGesture.ThumbsUp) {
                blockController.AdjustGridPosition();
                handGestureDetected = true;
            } 
        } else {
            if (handGestureDetector.CurrentHandGesture == HandGesture.None) {
                handGestureDetected = false;
            }
        }
    }

    public void StartGame() {
        musicController.StartGame();
        gameState = GAME_STATE.PLAY;
    }

    public void AddScore() {
        score ++;
        scoreText.text = score.ToString();
    }

}
