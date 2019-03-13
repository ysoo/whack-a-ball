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

    private void Awake()
    {
        targetTime = 30.0f;
        score = 0;
        running = true;
        gamePlaying = true;
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

        // Debug.Log("In Ball Set Active and gamePlaying is " +  gamePlaying);
        while (gamePlaying)
        {
            // Debug.Log("I'm playing the game yay");
            for (int i = 0; i < maxNum; i++)
            {
                int j = Random.Range(0, ballList.Length);  
                if (!ballList[j].isActiveAndEnabled)
                {
                    ballList[j].setActive();
                }
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(3.0f);
        }

        
    }
}
