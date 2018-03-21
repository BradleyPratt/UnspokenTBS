using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
	// Enum for the Unit's current state; due to changes to how units work, only idle and dying are now used.
	enum UnitTurnStatus
	{
		idle,
		rotating,
		moving,
		moved,
		attacked,
		dying
	};

	// Used to pass angle to rotate to between functions
	private float absAngle;
	// Status of the unit; ultimatley, is is dying or not?
	private UnitTurnStatus unitTurnStatus = UnitTurnStatus.idle;

	// Team this unit is on
	[SerializeField]
	private string team = "USA";

	// Camera used for raycasts
	[SerializeField]
	private Camera rayCamera;

	// How far the unit can move
	[SerializeField]
	private float moveRangeLimit = 50;

	// minimum/maxium range the unit can fire at
	[SerializeField]
	Vector2 attackRange = new Vector2(50, 150);

	// impact range of the projectile
	[SerializeField]
	float attackRadius = 20;

	// How much damage the projectile does
	[SerializeField]
	float attackStrength = 20;

	// Money awarded for killing this unit
	[SerializeField]
	float rewardMoney = 0;

	// Angle to offset turret rotation by
	[SerializeField]
	float angleOffset = 90;

	// Projector showing move range
	[SerializeField]
	GameObject moveRangeProjector;
	// Projector showing attack location and size
	[SerializeField]
	GameObject attackRangeProjector;
	// Prefab containing the projectile to fire
	[SerializeField]
	GameObject projectile;
	// Prefab containing the location marker
	[SerializeField]
	GameObject locationMarker;

	// Red material for projector
	[SerializeField]
	Material redProjectorMaterial;
	// Green material for projector
	[SerializeField]
	Material greenProjectorMaterial;

	// Currently active projector
	private GameObject currentProjector;
	// Currently active location marker
	private GameObject currentMarker;

	private Vector3 newPosition;
	private Vector3 movePosition;
	private float finalAngle;
	private Vector3 lengthOffsetV;

	private float animTimer = 0.0f;
	private float angleProg = 0.0f;
	private float angleAdjustment = 0.0f;
	private float angleChange;
	private bool unitHit;
	private bool unitPhaseMoving, unitMoving, unitMoved, unitAttacking, unitAttacked, unitSelected = false;
	private NavMeshAgent navMeshAgent;
	private TurnManager turnManager;

	// Use this for initialization
	void Start()
	{
		turnManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TurnManager>();

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
		
		lengthOffsetV = new Vector3(combinedMesh.bounds.extents.x, 0, 0);
	}

	// Update is called once per frame
	void Update()
	{
		

		if (unitTurnStatus == UnitTurnStatus.dying)
		{
			if (animTimer > 1.0f) {
				turnManager.UnitKilled(team);
				System.Random rand = new System.Random();
				if (!(GameObject.FindGameObjectWithTag("GameManager").GetComponent<TurnManager>().GetActiveTeam() == team))
				{
					GameObject.FindGameObjectWithTag("GameManager").GetComponent<Money>().SetMoney((rewardMoney - 5) + rand.Next(0, 10), team);
				}
				Destroy(this.gameObject);
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
				if (currentMarker != null)
				{
					Destroy(currentMarker);
				}
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
		unitSelected = selectedStatus;
	}
	
	// Tell this unit if it's selected
	public void SetSelected(bool selectedStatus, string phase)
	{
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
		return (unitMoved && unitAttacked);
	}

	public bool HasMoved()
	{
		return (unitMoved);
	}

	public bool HasAttacked()
	{
		return (unitAttacked);
	}

	public bool HasPerformedAction(string action)
	{
		if(action == "Move")
		{
			return HasMoved();
		} else if (action == "Attack")
		{
			return HasAttacked();
		} else
		{
			return false;
		}
	}

	public void ResetUnitTurn()
	{
		if (unitTurnStatus == UnitTurnStatus.dying)
		{
			Destroy(this.gameObject);
		}

		foreach (MeshRenderer meshRenderer in this.GetComponentsInChildren<MeshRenderer>())
		{
			meshRenderer.material.color = Color.white;
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
			GameObject tempObject = Instantiate(projectile, transform.position, Quaternion.Euler(0, transform.parent.localRotation.eulerAngles.y + transform.Find("Turret").localRotation.eulerAngles.z, 0));
			Projectile projectileS = tempObject.GetComponent<Projectile>();
				projectileS.SetInfo(target, attackRadius, attackStrength, this.gameObject);
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