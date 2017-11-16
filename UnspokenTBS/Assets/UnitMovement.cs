using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour {

	// Has the unit been selected by the turn counter? i.e. is it this unit's turn
	bool unitSelected = false;

	// Has the unit moved yet?
	bool hasMoved = false;

	// Should be set to be the camera used when the player moves units
	[SerializeField]
	Camera rayCamera;

	// The max distance the unit can move per turn. Set to 0 for no movement
	[SerializeField]
	float moveRangeLimit;

	// The min and max attack ranges for the unit
	[SerializeField]
	Vector2 attackRange;

	// The radius of the sphere in which units will take damage
	[SerializeField]
	float attackRadius;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (unitSelected && !hasMoved) {
			if (Input.GetMouseButtonDown (0)) {
				RaycastHit hit;
				Ray ray = rayCamera.ScreenPointToRay (Input.mousePosition);
				Physics.Raycast (ray.origin, ray.direction, out hit);
				Vector3 newPosition = ray.origin + ray.direction * hit.distance;
				if ((Vector3.Distance (newPosition, transform.position) <= moveRangeLimit) && (hit.collider.CompareTag("Terrain"))) {
					transform.position = newPosition;
					hasMoved = true;
				}
			}
		} else if (unitSelected && hasMoved) {
			// todo make another projecter which shows attack
			if (Input.GetMouseButtonDown (0)) {
				RaycastHit hit;
				Ray ray = rayCamera.ScreenPointToRay (Input.mousePosition);
				Physics.Raycast (ray.origin, ray.direction, out hit);
				Vector3 target = ray.origin + ray.direction * hit.distance;
				if (attackRange.x <= Vector3.Distance (target, transform.position) <= attackRange.y) {
					// Create array of colliders in attack range
					Collider[] colliderArray = Physics.OverlapSphere(target, attackRadius);

					foreach(Collider tempCollider in colliderArray){
						// check if colliders are units
						if (tempCollider.CompareTag ("Unit")) {
							//todo apply damage
						}
					}
				}
			}
		}
	}

	// Tell the script it's this unit's turn
	public void SetUnitTurn(bool turnStatus) {
		if (turnStatus) {
			// todo add projecter here to display range arround the unit with a circle
			hasMoved = false;
		}
		unitSelected = turnStatus;
	}

	// returns if it is this unit's turn
	public bool GetUnitTurn() {
		return unitSelected;
	}

	// Sets new movement range for unit; returns false if new range was below 0
	public bool SetMovementRange(float newRange){
		if (newRange >= 0) {
			moveRangeLimit = newRange;
			return true;
		} else {
			return false;
		}
	}

	// returns current movement range
	public float GetMovementRange() {
		return moveRangeLimit;
	}
}
