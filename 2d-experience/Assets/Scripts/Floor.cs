using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour {
    private float speed = .05f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(LevelManager.isGameOn)
        {
            transform.Translate(speed * Vector2.left);
        }
        
    }
}
