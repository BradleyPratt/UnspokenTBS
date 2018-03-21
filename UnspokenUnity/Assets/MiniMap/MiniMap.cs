using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// Find all mini map icons, and enable them when the game starts
		foreach(GameObject item in GameObject.FindGameObjectsWithTag("MiniMapItem"))
		{
			MeshRenderer meshRenderer = item.GetComponent<MeshRenderer>();
			if(meshRenderer != null)
			{
				meshRenderer.enabled = true;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
