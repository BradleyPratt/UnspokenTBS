using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapUnitIcon : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// Make object become independant, apply scaling, then re-attach to parent.
		Transform parent = transform.parent;
		transform.parent = null;
		transform.localScale = new Vector3(8,1,8);
		transform.parent = parent;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
