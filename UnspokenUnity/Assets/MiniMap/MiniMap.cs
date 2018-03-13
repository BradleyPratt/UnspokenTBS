using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour {

	// Use this for initialization
	void Start () {
		foreach(GameObject item in GameObject.FindGameObjectsWithTag("MiniMapItem"))
		{
			item.GetComponent<MeshRenderer>().enabled = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
