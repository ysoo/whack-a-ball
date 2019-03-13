using Leap.Unity.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    enum BallType
    {
        low,
        medium,
        high
    }

    GameController gameController;
    InteractionBehaviour interaction;
    MeshRenderer meshRenderer;

    [SerializeField] BallType type;

    // Use this for initialization
    void Awake ()
    {
        Physics.IgnoreLayerCollision(0, 8);
        interaction = GetComponent<InteractionBehaviour>();
        gameController = GetComponentInParent<GameController>();
        meshRenderer = GetComponent<MeshRenderer>();
	}

    void Start()
    {
        // gameObject.SetActive(false);
        // interaction.OnContactBegin += () => { DoHitBall(); };
        // Invoke("TimesUp", 1.2f);
    }

    Color GetColorForType(BallType type)
    {
        switch (type)
        {
            case BallType.low:
                return Color.red;

            case BallType.medium:
                return Color.blue;
          
            case BallType.high:
                return Color.yellow;

            default:
                return Color.black;
        }
    }

    int GetScoreForType(BallType type)
    {
        switch (type)
        {
            case BallType.low:
                return -300;
            case BallType.medium:
                return 300;
            case BallType.high:
                return 900;
            default:
                return 0;
        }
    }

    public void setActive()
    {
        // Debug.Log("In Balls");
        
        Array v = Enum.GetValues(typeof(BallType));
        System.Random random = new System.Random();
        BallType randomType = (BallType)v.GetValue(random.Next(v.Length));

        this.type = randomType;
        meshRenderer.material.color = GetColorForType(type);
        
        gameObject.SetActive(true);
    }

    void TimesUp()
    {
        gameObject.SetActive(false);
    }

    void DoHitBall()
    {
        gameController.UpdateScore(GetScoreForType(type));
        gameObject.SetActive(false);
    }
}
