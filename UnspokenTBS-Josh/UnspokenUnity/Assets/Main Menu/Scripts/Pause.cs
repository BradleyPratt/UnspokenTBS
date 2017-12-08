using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {

	private bool paused;

	private static Pause instance;

	public static Pause Instance
	{
		get
		{ 
			if (instance == null)
			{
                instance = GameObject.FindObjectOfType<Pause>();
                Debug.Log("it works");
            }

            return Pause.instance;
		}
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			PauseGame ();
		}
		
	}

	public void PauseGame()
	{
		paused = !paused;
		if (paused)
		{
			Time.timeScale = 0;
		} else
		{
			Time.timeScale = 1;
		}
    }

}
