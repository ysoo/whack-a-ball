﻿using Leap.Unity.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
    MeshRenderer[] meshRenderer;
    ConfigurableJoint cj;
    InteractionBehaviour interaction;
    Rigidbody rb;
    public bool isDown;
    AudioSource audio;
    public Material fur;

    bool lowering;


    [SerializeField] BallType type;

    // Use this for initialization
    void Awake ()
    {
        Physics.IgnoreLayerCollision(0, 8);
        rb = GetComponent<Rigidbody>();
        interaction = GetComponent<InteractionBehaviour>();
        gameController = GetComponentInParent<GameController>();
        meshRenderer = GetComponentsInChildren<MeshRenderer>();
        cj = GetComponent<ConfigurableJoint>();
        audio = GetComponent<AudioSource>();
	}

    void Start()
    {
        LowerTarget();
        interaction.OnContactBegin += DoSound;
        gameController.OnStartGame += GameState_Changed;
    }

    void GameState_Changed(GameController sender, bool isPlaying)
    {
        LowerTarget();
    }

    void DoSound()
    {
        audio.Play();
    }

    private void FixedUpdate()
    {
        if (isDown)
        {
            cj.yDrive = new JointDrive
            {
                maximumForce = 0.0f,
                positionSpring = 0.0f,
                positionDamper = 0.0f
            };
            rb.AddForce(transform.up * -10);
        }
        else
        {
            cj.yDrive = new JointDrive
            {
                maximumForce = 3.402823e+38f,
                positionSpring = 200.0f,
                positionDamper = 20f
            };
            rb.AddForce(transform.up * 10);
        }
    }

    // On collision with the bottom of the platform 
    // We freeze it's movement and then update scores
    private void OnTriggerEnter(Collider collider) 
    {
        if (collider.gameObject.name == "Collider" && !lowering)
        {
            isDown = true;
            lowering = true;
            gameController.UpdateScore(GetScoreForType(type));
            Debug.Log("Got Hit");
        }

    }


    /*
     * Lower the current balls and resets it's color and isDown 
     */
    public void LowerTarget()
    {
        lowering = true;
        isDown = true;
        for (int i = 0; i < meshRenderer.Length; i++)
        {
            if (meshRenderer[i].tag == "changeMaterial")
                meshRenderer[i].material = fur;
        }
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

    public void MoveUpAndChangeColor()
    {
        Debug.Log("In Balls");
       
       
        Array v = Enum.GetValues(typeof(BallType));
        System.Random random = new System.Random();
        BallType randomType = (BallType)v.GetValue(random.Next(v.Length));
        isDown = false;
        lowering = false;
        this.type = randomType;

        for(int i = 0; i < meshRenderer.Length; i++)
        {
            if(meshRenderer[i].tag == "changeMaterial")
                meshRenderer[i].material.color = GetColorForType(type);
        }
        
    }
}
