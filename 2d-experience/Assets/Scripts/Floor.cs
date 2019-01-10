using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour {
    public static float speed;
    private static float maxSpeed = 0.2f;
    private static float speedStep = 0.01f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!LevelManager.IsGamePaused())
        {
            transform.Translate(speed * Vector2.left);
        }
        
    }

    public static void IncreaseFloorSpeed()
    {
        if (speed < maxSpeed)
        {
            speed += speedStep;
        }
    }

    public static void SetupInitialFloorSpeed()
    {
        speed = 0.08f;
    }
}
