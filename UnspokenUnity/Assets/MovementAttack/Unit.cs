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
		attacked,
		dying
	};

	private float absAngle;
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
	float rotationSpeed = 10;

	[SerializeField]
	Vector2 attackRange = new Vector2(50, 150);

	[SerializeField]
	float attackRadius = 20;

	[SerializeField]
	float attackStrength = 20;

	[SerializeField]
	float rewardMoney = 0;

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
	private Vector3 movePosition;
	private float finalAngle;
	private Vector3 heightOffsetV;
	private Vector3 lengthOffsetV;

	private float animTimer = 0.0f;
	private float angleProg = 0.0f;
	private float angleAdjustment = 0.0f;
	private float angleChange;
	private bool unitHit;

	// Use this for initialization
	void Start()
	{
		rayCamera = Camera.main;
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

		heightOffsetV = new Vector3(0, combinedMesh.bounds.extents.y + heightOffset, 0);
		lengthOffsetV = new Vector3(combinedMesh.bounds.extents.x, 0, 0);


		int layer = 8; // Layer 8 is the terrain.
		int layermask = 1 << layer; // Turn the int into the layermask.

		RaycastHit hit;
		Physics.Raycast(transform.position, new Vector3(0, -1, 0), hitInfo: out hit, maxDistance: Mathf.Infinity, layerMask: layermask);
		transform.position = hit.point + heightOffsetV;
		
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

					if (hit.collider.gameObject.tag == "Terrain")
					{
						newPosition = ray.origin + ray.direction * hit.distance;
						//todo add offset

						Vector3 direction = (newPosition) - transform.position;
						RaycastHit obstacleFinder;
						Physics.Raycast(transform.position, direction, out obstacleFinder, Vector3.Distance(transform.position, newPosition));

						if (obstacleFinder.collider != null)
						{
							float temp = newPosition.y;
							newPosition = (transform.position) + (Vector3.Normalize(direction) * (obstacleFinder.distance -lengthOffsetV.x));
							newPosition.y = temp;
						}

						if (Vector3.Distance(newPosition, transform.position - heightOffsetV) <= moveRangeLimit)
						{
							angleProg = 0;
							Vector3 angleTarget = newPosition;
							angleTarget.x = angleTarget.x - transform.position.x;
							angleTarget.z = angleTarget.z - transform.position.z;
							finalAngle = Mathf.Atan2(angleTarget.z, angleTarget.x) * Mathf.Rad2Deg;
							angleChange = PositiveMod((PositiveMod(transform.rotation.eulerAngles.y, 360)) - (PositiveMod(finalAngle, 360)), 360);
							if(angleChange > 180)
							{
								angleChange -= 360;
							}
							unitTurnStatus = UnitTurnStatus.rotating;
						}
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
									collider.gameObject.GetComponent<Unit>().UnitHit();
								}
								if (collider.CompareTag("WatchTower"))
								{
									collider.gameObject.GetComponent<WatchTowerHealth>().WatchTowerTakeDamage(attackStrength);
									collider.gameObject.GetComponent<Unit>().UnitHit();
								}
							}
						}

						Destroy(currentProjector.gameObject);
						unitTurnStatus = UnitTurnStatus.attacked;
						foreach (MeshRenderer meshRenderer in this.GetComponentsInChildren<MeshRenderer>())
						{
							meshRenderer.material.color = Color.gray;
						}
						//this.GetComponentInChildren<MiniMapUnitIcon>().SetColor(Color.gray);
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
			if (angleProg < Mathf.Abs(angleChange))
			{
				float angleDelta = Time.deltaTime * rotationSpeed;
				angleProg += angleDelta;
				if (angleChange > 0)
				{
					angleDelta = -angleDelta;
				}
				transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, -angleDelta + transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
			} else
			{
				transform.rotation = Quaternion.Euler(new Vector3(-90, -finalAngle + angleOffset, 0));

				movePosition = transform.position;
				unitTurnStatus = UnitTurnStatus.moving;
			}
		}
		else if (unitTurnStatus == UnitTurnStatus.moving)
		{
			transform.position = movePosition;
			transform.position = Vector3.MoveTowards(transform.position, newPosition+ heightOffsetV, Time.deltaTime * moveSpeed);
			movePosition = transform.position;
			if (!(transform.position == newPosition + heightOffsetV))
			{
				//int layer = 8; // Layer 8 is the terrain.
				//int layermask = 1 << layer; // Turn the int into the layermask.

				//RaycastHit hit;
				//Physics.Raycast(transform.position, new Vector3(0, -1, 0), hitInfo: out hit, maxDistance: Mathf.Infinity, layerMask: layermask);
				//transform.position = hit.point + heightOffsetV;
			}
			else
			{
				unitTurnStatus = UnitTurnStatus.moved;
				if (unitSelected)
				{
					CreateAttackProjector();
				}
			}
		} else if (unitTurnStatus == UnitTurnStatus.dying)
		{
			if (animTimer > 1.0f) {
				Destroy(this.gameObject);
				System.Random rand = new System.Random();
				if (!(GameObject.FindGameObjectWithTag("GameManager").GetComponent<TurnManager>().GetActiveTeam() == team))
				{
					GameObject.FindGameObjectWithTag("GameManger").GetComponent<Money>().SetMoney((rewardMoney - 5) + rand.Next(0, 10), team);
				}
			}
			animTimer += Time.deltaTime;
		}

		if (unitHit)
		{
			System.Random rand = new System.Random();
			Vector3 adjustment = new Vector3(rand.Next(-5, 5)*(float)0.03, 0, rand.Next(-5, 5) * (float)0.03);
			transform.position+=adjustment;
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


		Vector3 angleTarget = newPosition;
		angleTarget.x = angleTarget.x - transform.position.x;
		angleTarget.z = angleTarget.z - transform.position.z;
		finalAngle = Mathf.Atan2(angleTarget.z, angleTarget.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(new Vector3(-90, -finalAngle + angleOffset, 0));
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
		if (unitTurnStatus == UnitTurnStatus.dying)
		{
			Destroy(this.gameObject);
		} else if (unitTurnStatus == UnitTurnStatus.moving)
		{
			transform.position = newPosition + heightOffsetV;
		} else if (unitTurnStatus == UnitTurnStatus.rotating)
		{
			transform.rotation = Quaternion.Euler(new Vector3(-90, -finalAngle + angleOffset, 0));
			transform.position = newPosition + heightOffsetV;
		} else if (unitTurnStatus == UnitTurnStatus.attacked)
		{
			foreach (MeshRenderer meshRenderer in this.GetComponentsInChildren<MeshRenderer>())
			{
				meshRenderer.material.color = Color.white;
			}
		}
		unitTurnStatus = UnitTurnStatus.idle;
	}

	public void UnitKilled()
	{
		animTimer = 0.0f;
		unitTurnStatus = UnitTurnStatus.dying;
		foreach (MeshRenderer meshRenderer in this.GetComponentsInChildren<MeshRenderer>())
		{
			meshRenderer.material.color = Color.red;
		}
	}

	IEnumerator ShakeUnit()
	{
		foreach (MeshRenderer meshRenderer in this.GetComponentsInChildren<MeshRenderer>())
		{
			meshRenderer.material.color = Color.red;
		}
		Vector3 oldPosition = transform.position;
		unitHit = true;
		float targetTime = 0.0f;
		while (targetTime < 1)
		{
			targetTime += Time.deltaTime;
			Debug.Log(targetTime);
			yield return null;
		}
		unitHit = false;
		transform.position = oldPosition;

		foreach (MeshRenderer meshRenderer in this.GetComponentsInChildren<MeshRenderer>())
		{
			meshRenderer.material.color = Color.white;
		}
	}

	public void UnitHit()
	{
		StartCoroutine("ShakeUnit");
	}

	private float PositiveMod(float number, float divisor)
	{
		// Get the standard C# % value, then add the divisor again to make sure it's +ve, then apply % again to make sure it's in the right range if the first result was positive.
		return ((number % divisor) + divisor) % divisor;
	}
}