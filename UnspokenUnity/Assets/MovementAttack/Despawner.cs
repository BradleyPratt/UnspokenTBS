using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawner : MonoBehaviour {


	// Time after creation to destroy the object at
	[SerializeField]
	float killTime = 5.0f;

	// How much time has passed
	float currentTime = 0.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// Add elapsed time to current time
		currentTime += Time.deltaTime;

		// If we've reached the kill time, destroy the object
		if (currentTime >= killTime)
		{
			Destroy(this.gameObject);
		}
	}
}
