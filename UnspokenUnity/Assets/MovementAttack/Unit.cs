using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
	GameObject projectile;
	[SerializeField]
	GameObject locationMarker;

	[SerializeField]
	Material redProjectorMaterial;
	[SerializeField]
	Material greenProjectorMaterial;

	private GameObject currentProjector;
	private GameObject currentMarker;

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
	private bool unitPhaseMoving, unitMoving, unitMoved, unitAttacking, unitAttacked, unitSelected = false;
	private NavMeshAgent navMeshAgent;

	// Use this for initialization
	void Start()
	{
		if (projectile == null)
		{
			projectile = Resources.Load<GameObject>("Projectile");
		}
		if (locationMarker == null)
		{
			locationMarker = Resources.Load<GameObject>("LocationMarker");
		}
		rayCamera = Camera.main;
		navMeshAgent = GetComponentInParent<NavMeshAgent>();
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
	}

	// Update is called once per frame
	void Update()
	{
		

		if (unitTurnStatus == UnitTurnStatus.dying)
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

	void LateUpdate()
	{
		if (unitMoving)
		{
			if ((navMeshAgent.remainingDistance < 0.0002) && (navMeshAgent.pathPending == false))
			{
				unitMoving = false;
				unitMoved = true;
				HasActionsLeft();
				if (currentProjector != null)
				{
					Destroy(currentProjector);
				}
				Destroy(currentMarker);
				GameObject.FindGameObjectWithTag("GameManager").GetComponent<TurnManager>().RunTurrets();
			}
		}

		if (unitAttacking)
		{
			PositionAttackProjector();
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

		Transform turret = transform.Find("Turret");

		if (turret != null)
		{
			transform.Find("Turret").rotation = Quaternion.Euler(new Vector3(-90, -finalAngle + angleOffset, 0));
		} else
		{
			Debug.Log("Cannot turn turret; turning full tank.");
			transform.rotation = Quaternion.Euler(new Vector3(-90, -finalAngle + angleOffset, 0));
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
			}
			else if (unitTurnStatus == UnitTurnStatus.moved)
			{
				CreateAttackProjector();
			}
		}
		else if (currentProjector != null)
		{
			Destroy(currentProjector.gameObject);
		}
		lockInput = selectedStatus;
		unitSelected = selectedStatus;
	}
	
	// Tell this unit if it's selected
	public void SetSelected(bool selectedStatus, string phase)
	{
		Debug.Log(phase);
		if (selectedStatus)
		{
			if (phase == "Move" && !unitMoved)
			{
				CreateMoveProjector();
				unitPhaseMoving = true;
				unitAttacking = false;
			}
			else if (phase == "Attack" && !unitAttacked)
			{
				unitPhaseMoving = false;
				unitAttacking = true;
				CreateAttackProjector();
			}
		}
		else if (currentProjector != null)
		{
			Destroy(currentProjector.gameObject);
		}
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
		unitPhaseMoving = unitMoving = unitMoved = unitAttacking = unitAttacked = unitSelected = false;
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

	public void MoveTo(Vector3 target)
	{
		if ((!unitMoved && unitPhaseMoving) && InMoveRange(target))
		{
			currentMarker = Instantiate(locationMarker, target, new Quaternion());
			navMeshAgent.destination = target;
			unitMoving = true;
		}
	}

	public void FireAt(Vector3 target)
	{
		if ((unitAttacking) && (InAttackRange(target)))
		{
			GameObject tempObject = Instantiate(projectile, transform.position, transform.rotation);
			tempObject.GetComponent<Projectile>().SetTarget(target);
			tempObject.GetComponent<Projectile>().SetAttackRadius(attackRadius);
			tempObject.GetComponent<Projectile>().SetAttackStrength(attackStrength);
			Destroy(currentProjector.gameObject);
			unitAttacking = false;
			unitAttacked = true;
			HasActionsLeft();
		}
	}

	public bool InMoveRange(Vector3 target)
	{
		if (Vector3.Distance(transform.position, target) <= moveRangeLimit)
		{
			return true;
		}
		return false;
	}

	public bool InAttackRange(Vector3 target)
	{
		if ((Vector3.Distance(transform.position, target) >= attackRange.x) && (Vector3.Distance(transform.position, target) <= attackRange.y))
		{
			return true;
		}
		return false;
	}

	public string GetPhase()
	{
		if(unitPhaseMoving && !unitMoved && !unitMoving)
		{
			return "Move";
		} else if (unitAttacking)
		{
			return "Attack";
		}
		return null;
	}

	private bool HasActionsLeft()
	{
		if (!unitMoved || !unitAttacked)
		{
			return true;
		} else
		{
			foreach (MeshRenderer meshRenderer in this.GetComponentsInChildren<MeshRenderer>())
			{
				meshRenderer.material.color = Color.gray;
			}

			return false;
		}
	}
}