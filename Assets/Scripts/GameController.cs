using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Leap.Unity.Interaction;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{ 
    enum Difficulty
    {
        easy,
        medium,
        hard
    }

    public delegate void GameChangeHandler(GameController sender, bool gamePlaying);
    public event GameChangeHandler OnStartGame;

    [SerializeField] Ball[] ballList;
    int maxNum;
    [SerializeField] Difficulty type;

    public TextMesh countText;
    public TextMesh timerText;
    int score;
    float targetTime;
    bool gamePlaying;
    bool running;
    float waveWait;
    float spawnOver;



    private void Awake()
    {

    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (running)
        {
            targetTime -= Time.deltaTime;
            timerText.text = "Time left: " + targetTime;
            if (targetTime <= 0.0f)
            {
                TimerEnded();
            }
        }
    }

    public void StartGame()
    {
        waveWait = GetWaveWaitTime(type);
        spawnOver = GetSpawnTime(type);
        maxNum = GetMaxNum(type);
        targetTime = 30.0f;
        score = 0;
        UpdateScore(0);
        running = true;
        gamePlaying = true;
        Debug.Log("Start");
        OnStartGame(this, true);
        StartCoroutine(ballSetActive());
    }

    private void TimerEnded()
    {
        gamePlaying = false;
        targetTime = 0f;
        running = false;
        timerText.text = "GAME OVER";
        for (int i = 0; i < ballList.Length; i++)
        {
            ballList[i].LowerTarget();
        }
    }


    public void UpdateScore(int value)
    {
        score += value;
        countText.text = "Score: " + score.ToString();
    }

    public void EndGame()
    {
        Debug.Log("Quit Game");
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
    
    IEnumerator ballSetActive()
    {
        while(gamePlaying)
        {
            for(int i = 0; i < maxNum; i++)
            {
                Debug.Log("spawn Over: " + spawnOver + " waveWait: " + waveWait);

                int j = Random.Range(0, ballList.Length);
                if (ballList[j].isDown)
                {
                    ballList[j].MoveUpAndChangeColor();
                }
                yield return new WaitForSeconds(spawnOver);
                ballList[j].LowerTarget();
            }
            yield return new WaitForSeconds(waveWait);
        }
        
    }

    #region difficulty
    public void Hard()
    {
        type = Difficulty.hard;
    }

    public void Medium()
    {
        type = Difficulty.medium;
    }

    public void Easy()
    {
        type = Difficulty.easy;
    }

    int GetMaxNum(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.easy:
                return 3;
            case Difficulty.medium:
                return 4;
            case Difficulty.hard:
                return 5;
            default:
                return 3;
        }
    }

    float GetWaveWaitTime(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.easy:
                return 1.5f;
            case Difficulty.medium:
                return 1.0f;
            case Difficulty.hard:
                return 0.5f;
            default:
                return 1.5f;
        }
    }

    float GetSpawnTime(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.easy:
                return 1.0f;
            case Difficulty.medium:
                return 0.75f;
            case Difficulty.hard:
                return 0.5f;
            default:
                return 1.0f;
        }
    }

    #endregion

}
