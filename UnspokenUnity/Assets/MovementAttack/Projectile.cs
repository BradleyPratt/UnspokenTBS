using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	Vector3 target;
	float speed = 10;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position != target)
		{
			Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
		} else
		{

		}
	}
}
