using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    Vector3 newPosition;
    bool movingUnit = false;

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
        // todo: remove when we have turn manager
        SetUnitTurn(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (unitSelected && !hasMoved)
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
                    movingUnit = true;
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
                    }
                }
            }
        }
    }

    void FixedUpdate() {
        if ((movingUnit)){
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
                PositionAttackProjector();
            }
        }
    }

    // Tell the script it's this unit's turn
    public void SetUnitTurn(bool turnStatus)
    {
        if (turnStatus)
        {
            // todo add projecter here to display range arround the unit with a circle
            currentProjector = Instantiate(moveRangeProjector);
            currentProjector.GetComponent<Projector>().orthographicSize = moveRangeLimit;
            currentProjector.transform.position = this.transform.position;
            hasMoved = false;
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
		int layer = 8;
		int layermask = 1 << layer;
		Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, layermask);
        newPosition = ray.origin + ray.direction * hit.distance;
        Debug.Log("Octagon");
//        rayCamera.ScreenToWorldPoint(Input.mousePosition);
        currentProjector.transform.position = new Vector3(newPosition.x, this.transform.position.y, newPosition.z);
        Debug.Log(new Vector3(rayCamera.ScreenToWorldPoint(Input.mousePosition).x, rayCamera.ScreenToWorldPoint(Input.mousePosition).y, this.transform.position.z));
        Debug.Log(Input.mousePosition);
        Debug.Log(rayCamera.ViewportToWorldPoint(Input.mousePosition));
        Debug.Log(newPosition);
    }
}
