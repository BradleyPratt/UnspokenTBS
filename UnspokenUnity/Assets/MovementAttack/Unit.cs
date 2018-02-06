using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
	enum UnitTurnStatus
	{
		idle,
		rotating,
		moving,
		moved,
		attacked
	};

	private UnitTurnStatus unitTurnStatus = UnitTurnStatus.idle;
	private bool unitSelected = false;
	private bool lockInput = false;

	[SerializeField]
	private string team = "USA";

	[SerializeField]
	private Camera rayCamera;

	[SerializeField]
	private float moveRangeLimit = 50;

	[SerializeField]
	float moveSpeed = 10;

	[SerializeField]
	Vector2 attackRange = new Vector2(50, 150);

	[SerializeField]
	float attackRadius = 20;

	[SerializeField]
	float attackStrength = 20;

	[SerializeField]
	float angleOffset = 90;

	[SerializeField]
	float heightOffset = 0;

	[SerializeField]
	GameObject moveRangeProjector;
	[SerializeField]
	GameObject attackRangeProjector;

	[SerializeField]
	Material redProjectorMaterial;
	[SerializeField]
	Material greenProjectorMaterial;

	private GameObject currentProjector;

	private Vector3 newPosition;
	private float yAngle;
	private Vector3 heightOffsetV;
	private Vector3 lengthOffsetV;
	// Use this for initialization
	void Start()
	{
						MeshFilter[] meshFilters = this.GetComponentsInChildren<MeshFilter>();
						CombineInstance[] combine = new CombineInstance[meshFilters.Length];
						int i = 0;
						while (i < meshFilters.Length)
						{
							combine[i].mesh = meshFilters[i].sharedMesh;
							combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
							i++;
						}
						Mesh combinedMesh = new Mesh();
						combinedMesh.CombineMeshes(combine);

		//Debug.Log(combinedMesh.bounds.extents.z);
		//Debug.Log(combinedMesh.bounds.extents.x);
		heightOffsetV = new Vector3(0, combinedMesh.bounds.extents.y + heightOffset, 0);
		lengthOffsetV = new Vector3(combinedMesh.bounds.extents.x, 0, 0);
	}

	// Update is called once per frame
	void Update()
	{
		if (!lockInput && unitSelected && (Time.timeScale != 0))
		{
			if (unitTurnStatus == UnitTurnStatus.idle)
			{
				if (Input.GetMouseButtonDown(0))
				{
					int layer = 8; // Layer 8 is the terrain.
					int layermask = 1 << layer; // Turn the int into the layermask.

					RaycastHit hit;
					Ray ray = rayCamera.ScreenPointToRay(Input.mousePosition);
					Physics.Raycast(ray.origin, ray.direction, hitInfo: out hit, maxDistance: Mathf.Infinity, layerMask: layermask);
					newPosition = ray.origin + ray.direction * hit.distance;
					//todo add offset
					Debug.Log(newPosition);

//					Vector3 direction = transform.position - newPosition;
//					Debug.DrawLine(transform.position, -direction, Color.red, 60000);
//					RaycastHit obstacleFinder;
//					Physics.Raycast(transform.position, Vector3.) {
//
//					}
					if (Vector3.Distance(newPosition, transform.position - heightOffsetV) <= moveRangeLimit)
					{
						unitTurnStatus = UnitTurnStatus.rotating;
					}
				}
			} else if (unitTurnStatus == UnitTurnStatus.moved)
			{
				PositionAttackProjector();
				if (Input.GetMouseButtonDown(0))
				{
					RaycastHit hit;
					Ray ray = rayCamera.ScreenPointToRay(Input.mousePosition);
					Physics.Raycast(ray.origin, ray.direction, out hit);
					Vector3 target = ray.origin + ray.direction * hit.distance;

					if ((attackRange.x <= Vector3.Distance(target, transform.position)) && (Vector3.Distance(target, transform.position) <= attackRange.y))
					{
						Collider[] colliderArray = Physics.OverlapSphere(target, attackRadius);

						foreach (Collider collider in colliderArray)
						{
							if (collider.gameObject != this.gameObject)
							{
								if (collider.CompareTag("Unit"))
								{
									collider.gameObject.GetComponent<HealthBar>().TakeDamage(attackStrength);
								}
								if (collider.CompareTag("WatchTower"))
								{
									collider.gameObject.GetComponent<WatchTowerHealth>().WatchTowerTakeDamage(attackStrength);
								}
							}
						}

						Destroy(currentProjector.gameObject);
						unitTurnStatus = UnitTurnStatus.attacked;
					}
				}
			}
		} else if (lockInput) {
			if (Input.GetMouseButtonUp(0))
			{
				lockInput = false;
			}
		}


		if (unitTurnStatus == UnitTurnStatus.rotating)
		{
			Vector3 angleTarget = newPosition;
			angleTarget.x = angleTarget.x - transform.position.x;
			angleTarget.z = angleTarget.z - transform.position.z;
			float angle = Mathf.Atan2(angleTarget.z, angleTarget.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(new Vector3(-90, -angle + angleOffset, 0));

			unitTurnStatus = UnitTurnStatus.moving;
		}
		else if (unitTurnStatus == UnitTurnStatus.moving)
		{
			if (!(transform.position == newPosition + heightOffsetV))
			{
				transform.position = Vector3.MoveTowards(transform.position, newPosition+ heightOffsetV, Time.deltaTime * moveSpeed);
			}
			else
			{
				unitTurnStatus = UnitTurnStatus.moved;
				if (unitSelected)
				{
					CreateAttackProjector();
				}
			}
		}
	}

	private void CreateMoveProjector()
	{
		if (currentProjector != null)
		{
			Destroy(currentProjector.gameObject);
		}

		currentProjector = Instantiate(moveRangeProjector);
		currentProjector.GetComponent<Projector>().orthographicSize = moveRangeLimit;
		currentProjector.transform.position = this.transform.position;
	}

	private void CreateAttackProjector()
	{
		if (currentProjector != null)
		{
			Destroy(currentProjector.gameObject);
		}

		currentProjector = Instantiate(attackRangeProjector);
		currentProjector.GetComponent<Projector>().orthographicSize = attackRadius;
		PositionAttackProjector();
	}

	private void PositionAttackProjector()
	{
		RaycastHit hit;
		Ray ray = rayCamera.ScreenPointToRay(Input.mousePosition);
		int layer = 8; // Layer 8 is the terrain.
		int layermask = 1 << layer; // Turn the int into the layermask.
		Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, layermask);
		newPosition = ray.origin + ray.direction * hit.distance;
		currentProjector.transform.position = new Vector3(newPosition.x, this.transform.position.y, newPosition.z);
		if ((attackRange.x <= Vector3.Distance(currentProjector.transform.position, transform.position)) && (Vector3.Distance(currentProjector.transform.position, transform.position) <= attackRange.y))
		{
			currentProjector.GetComponent<Projector>().material = greenProjectorMaterial;
		} else
		{
			currentProjector.GetComponent<Projector>().material = redProjectorMaterial;
		}
	}

	// Tell this unit if it's selected
	public void SetSelected(bool selectedStatus)
	{
		if (selectedStatus)
		{
			if (unitTurnStatus == UnitTurnStatus.idle)
			{
				CreateMoveProjector();
			} else if (unitTurnStatus == UnitTurnStatus.moved)
			{
				CreateAttackProjector();
			}
		} else if (currentProjector != null) {
			Destroy(currentProjector.gameObject);
		}
		lockInput = selectedStatus;
		unitSelected = selectedStatus;
	}

	public bool IsSelected()
	{
		return unitSelected;
	}

	// Sets new movement range for unit; returns false if new range was below 0
	public bool SetMovementRange(float newRange)
	{
		if (newRange >= 0)
		{
			moveRangeLimit = newRange;
			return true;
		}
		else
		{
			return false;
		}
	}

	// returns current movement range
	public float GetMovementRange()
	{
		return moveRangeLimit;
	}

	public string GetTeam()
	{
		return team;
	}

	public bool SetTeam(string newTeam)
	{
		if (string.Equals(newTeam, "USA") || string.Equals(newTeam, "USSR"))
		{
			team = newTeam;
			return true;
		}
		else
		{
			return false;
		}
	}

	private void OnMouseDown()
	{
		GameObject.FindGameObjectWithTag("GameManager").GetComponent<TurnManager>().SetCurrentUnit(this.gameObject);
	}

	public bool HasFinishedTurn()
	{
		return (unitTurnStatus == UnitTurnStatus.attacked);
	}

	public void ResetUnitTurn()
	{
		unitTurnStatus = UnitTurnStatus.idle;
	}
}