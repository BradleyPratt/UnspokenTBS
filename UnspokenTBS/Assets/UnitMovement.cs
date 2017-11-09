using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour {

	bool unitSelected = true;
	[SerializeField]
	Camera rayCamera;
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
				Ray ray = rayCamera.ScreenPointToRay (Input.mousePosition);
				Physics.Raycast (ray.origin, ray.direction, out hit);
				Vector3 newPosition = ray.origin + ray.direction * hit.distance;
				if (Vector3.Distance (newPosition, transform.position) <= moveRangeLimit) {
					transform.position = newPosition;
				}
			}
		}
	}

	public void SetUnitTurn(bool turnStatus) {
		unitSelected = turnStatus;
	}

	public bool GetUnitTurn() {
		return unitSelected;
	}

	public bool SetMovementRange(float newRange){
		if (newRange >= 0) {
			moveRangeLimit = newRange;
			return true;
		} else {
			return false;
		}
	}

	public float GetMovementRange() {
		return moveRangeLimit;
	}
}
