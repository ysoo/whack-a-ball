using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Leap.Unity.Interaction;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] Ball[] ballList;
    [SerializeField] float maxNum;
    public TextMesh countText;
    public TextMesh timerText;
    int score;
    float targetTime;
    [SerializeField] bool gamePlaying;
    bool running;
    public float waveWait;
    public float spawnOver;

    private void Awake()
    {
        targetTime = 30.0f;
        score = 0;
        running = true;
        gamePlaying = true;
        waveWait = 1.5f;
        spawnOver = 1.0f;
    }

    private void Start()
    {
        StartCoroutine(ballSetActive());
        UpdateScore(0);
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

    public void Restart()
    {
        Debug.Log("RESTART");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    
    IEnumerator ballSetActive()
    {
        while(gamePlaying)
        {
            for(int i = 0; i < maxNum; i++)
            {
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

    
}
