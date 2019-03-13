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
    MeshRenderer meshRenderer;
    ConfigurableJoint cj;
    Rigidbody rb;
    public bool isDown;

    [SerializeField] BallType type;

    // Use this for initialization
    void Awake ()
    {
        Physics.IgnoreLayerCollision(0, 8);
        rb = GetComponent<Rigidbody>();
        gameController = GetComponentInParent<GameController>();
        meshRenderer = GetComponent<MeshRenderer>();
        cj = GetComponent<ConfigurableJoint>();
	}

    void Start()
    {
        isDown = true;
        LowerTarget();
    }

    // On collision with the bottom of the platform 
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Collider")
        {
            isDown = true;
            cj.yDrive = new JointDrive
            {
                maximumForce = 0,
            };
        }
    }

    void LowerTarget()
    {
        rb.velocity = new Vector3(0, -0.08f, 0);
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
        Debug.Log("In Balls");
        /*
        Array v = Enum.GetValues(typeof(BallType));
        System.Random random = new System.Random();
        BallType randomType = (BallType)v.GetValue(random.Next(v.Length));

        this.type = randomType;
        meshRenderer.material.color = GetColorForType(type);
        
        */


        cj.yDrive = new JointDrive
        {
            maximumForce = 3.402823e+38f,
            positionSpring = 200.0f,
            positionDamper = 20f
        };
        isDown = false;
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
