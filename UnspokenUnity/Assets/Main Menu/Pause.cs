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
		if (Input.GetKeyDown (KeyCode.P)) 
		{
			PauseGame ();
		}
		
	}

	public void PauseGame()
	{
		paused = !paused;
        Debug.Log("it works");
    }

}
