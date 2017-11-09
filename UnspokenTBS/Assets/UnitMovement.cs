using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour {

	bool unitSelected = true;
	[SerializeField]
	Camera camera;
	[SerializeField]
	float moveRangeLimit;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (unitSelected) {
			if (Input.GetMouseButtonDown (0)) {
				RaycastHit hit;
				Ray ray = camera.ScreenPointToRay (Input.mousePosition);
				Physics.Raycast (ray.origin, ray.direction, out hit);
				Vector3 newPosition = ray.origin + ray.direction * hit.distance;
				if (Vector3.Distance (newPosition, transform.position) <= moveRangeLimit) {
					transform.position = newPosition;
				}
			}
		}
	}

	// Todo - replace with automatic selection from the turn system.
	void OnMouseDown() {
		//unitSelected = !unitSelected;
	}

}
