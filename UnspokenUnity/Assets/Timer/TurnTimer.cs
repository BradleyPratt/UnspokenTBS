using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTimer : MonoBehaviour {

    float turnTime = 0.0f;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        turnTime += Time.deltaTime;
    }

    void timerTurn()
    {
        turnTime = 0;
    }

    void setText()
    {
        countText.text = "Time: " + turnTime.ToString();
        if (turnTime >= 10)
        {

        }
    }
}
