using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
	enum UnitStatus {
		selected,
		moving,
		moved,
		idle
	};

	[SerializeField]
	private string team;

	bool test = true;
	Vector3 newPosition;
    bool movingUnit = false;
	bool rotatingUnit = false;

    // Has the unit been selected by the turn counter? i.e. is it this unit's turn
    bool unitSelected = false;

    // Has the unit moved yet?
    bool hasMoved = false;
	float yAngle;
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

	// The attack strength
	[SerializeField]
	float attackStrength;

    [SerializeField]
    GameObject moveRangeProjector;
    [SerializeField]
    GameObject attackRangeProjector;

    private GameObject currentProjector;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (unitSelected && !hasMoved && !movingUnit && !rotatingUnit)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = rayCamera.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(ray.origin, ray.direction, out hit);
                newPosition = ray.origin + ray.direction * hit.distance;
                if ((Vector3.Distance(newPosition, new Vector3 (transform.position.x, transform.position.y - this.GetComponent<MeshFilter>().mesh.bounds.extents.y, transform.position.z)) <= moveRangeLimit) && (hit.collider.CompareTag("Terrain")))
                {
                    //transform.position = Vector3.MoveTowards(transform.position, new Vector3 (newPosition.x, newPosition.y+this.GetComponent<MeshFilter>().mesh.bounds.extents.y, newPosition.z), Time.deltaTime);
                    rotatingUnit = true;
				}
            }
        }
        else if (unitSelected && hasMoved)
        {
            // todo make attck projector follow cursor.
            PositionAttackProjector();
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = rayCamera.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(ray.origin, ray.direction, out hit);
                Vector3 target = ray.origin + ray.direction * hit.distance;
                if ((attackRange.x <= Vector3.Distance(target, transform.position)) && (Vector3.Distance(target, transform.position) <= attackRange.y))
                {
                    // Create array of colliders in attack range
                    Collider[] colliderArray = Physics.OverlapSphere(target, attackRadius);

                    foreach (Collider tempCollider in colliderArray)
                    {
						// check if colliders are units
						if (tempCollider.CompareTag("Unit"))
						{
							tempCollider.gameObject.GetComponent<HealthBar>().TakeDamage(attackStrength);
						}
						if (tempCollider.CompareTag("WatchTower"))
						{
							tempCollider.gameObject.GetComponent<WatchTowerHealth>().WatchTowerTakeDamage(attackStrength);
						}
					}
					Destroy(currentProjector.gameObject);
					SetUnitTurn(false);
					GameObject.FindGameObjectWithTag("GameManager").GetComponent<TurnManager>().SwitchUnit();
                }
            }
        }
    }

    void FixedUpdate()
	{

		if (rotatingUnit) {
			Vector3 angleTarget = newPosition;
			angleTarget.x = angleTarget.x - transform.position.x;
			angleTarget.z = angleTarget.z - transform.position.z;
			float angle = Mathf.Atan2(angleTarget.z, angleTarget.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(new Vector3(0, -angle+90, 0));
			rotatingUnit = false;
			movingUnit = true;
		} else if (movingUnit){
            if (!(transform.position == new Vector3(newPosition.x, newPosition.y + this.GetComponent<MeshFilter>().mesh.bounds.extents.y, newPosition.z)))
            {
                Debug.Log(Vector3.Distance(transform.position, new Vector3(newPosition.x, newPosition.y + this.GetComponent<MeshFilter>().mesh.bounds.extents.y, newPosition.z)));
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(newPosition.x, newPosition.y + this.GetComponent<MeshFilter>().mesh.bounds.extents.y, newPosition.z), Time.deltaTime);
            }
            else
            {
                movingUnit = false;
                hasMoved = true;
				Destroy(currentProjector.gameObject);
                currentProjector = Instantiate(attackRangeProjector);
				currentProjector.GetComponent<Projector>().orthographicSize = attackRadius;
                PositionAttackProjector();
            }
        }
    }

    // Tell the script it's this unit's turn
    public void SetUnitTurn(bool turnStatus)
    {
        if (turnStatus)
        { 
			if (moveRangeLimit <= 0)
			{
				currentProjector = Instantiate(attackRangeProjector);
				currentProjector.GetComponent<Projector>().orthographicSize = attackRadius;
				PositionAttackProjector();
				hasMoved = true;
			} else
			{
			currentProjector = Instantiate(moveRangeProjector);
            currentProjector.GetComponent<Projector>().orthographicSize = moveRangeLimit;
            currentProjector.transform.position = this.transform.position;
            hasMoved = false;

			}
        }
        unitSelected = turnStatus;
    }

    // returns if it is this unit's turn
    public bool GetUnitTurn()
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
	
    private void PositionAttackProjector()
    {
        RaycastHit hit;
        Ray ray = rayCamera.ScreenPointToRay(Input.mousePosition);
		int layer = 8; // Layer 8 is the terrain.
		int layermask = 1 << layer; // Turn the int into the layermask.
		Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, layermask);
        newPosition = ray.origin + ray.direction * hit.distance;
        currentProjector.transform.position = new Vector3(newPosition.x, this.transform.position.y, newPosition.z);
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
		} else
		{
			return false;
		}
	}
}
