using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cube : MonoBehaviour {

    public float moveSpeed = 1;

	// Use this for initialization
	void Start ()
    {
        moveSpeed = 1f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.Translate(moveSpeed*Input.GetAxis("Horizontal")*Time.deltaTime,0f, moveSpeed*Input.GetAxis("Vertical") * Time.deltaTime);
	}
}
