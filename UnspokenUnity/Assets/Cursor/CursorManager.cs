using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour
{
	// Sprites for the cursor's current action
	[SerializeField]
	Texture2D idle, selecting, moving, attacking;

	TurnManager turnManager;

	// Start on the idle cursor
	CurrentAction currentAction = CurrentAction.idle;
	// Start on the move phase
	private string phase = "Move";

	// Enum for current cursor action
	enum CurrentAction
	{
		idle,
		selecting,
		moving,
		attacking
	}

	// Use this for initialization
	void Start()
	{
		// Set cursor to the idle sprite on game start
		Cursor.SetCursor(idle, new Vector2(4, 132), CursorMode.Auto);
		turnManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TurnManager>();
	}

	// Update is called once per frame
	void Update()
	{
		// If not paused
		if (Time.timeScale != 0)
		{
			// Check if we're over a UI item
			if (EventSystem.current.IsPointerOverGameObject())
			{
				SetAction(CurrentAction.idle);
			}
			else
			{
				// Check if there's currently a unit selected
				bool unitSet = false;
				if (turnManager.GetCurrentUnit() != null)
				{
					unitSet = true;
				}

				// Raycast from the cursor position into the scene
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				Physics.Raycast(ray.origin, ray.direction, hitInfo: out hit, maxDistance: Mathf.Infinity);

				// Check if we hit a unit
				bool colliderIsUnit = false;
				if (hit.collider.gameObject.GetComponent<Unit>() != null)
				{
					colliderIsUnit = true;
				}

				// If we hit a unit and it's on our team and can still perform the current action.
				if (colliderIsUnit && (turnManager.GetActiveTeam() == hit.collider.gameObject.GetComponent<Unit>().GetTeam()) && (!hit.collider.gameObject.GetComponent<Unit>().HasPerformedAction(phase)))
				{
					SetAction(CurrentAction.selecting);
				}
				// If there's a current unit, we hit the ground, the unit is on our team, and it's in the move phase, and in movement range.
				else if (unitSet && (((hit.collider.tag == "Terrain") && (turnManager.GetCurrentUnit().GetComponent<Unit>().GetTeam() == turnManager.GetActiveTeam())) && (turnManager.GetCurrentUnit().GetComponent<Unit>().GetPhase() == "Move") && (turnManager.GetCurrentUnit().GetComponent<Unit>().InMoveRange(hit.point))))
				{
					SetAction(CurrentAction.moving);
				}
				// If there's a current unit, and it's on our team, and it's in the attack phase, and in attack range.
				else if (unitSet && ((turnManager.GetCurrentUnit().GetComponent<Unit>().GetTeam() == turnManager.GetActiveTeam()) && (turnManager.GetCurrentUnit().GetComponent<Unit>().GetPhase() == "Attack") && (turnManager.GetCurrentUnit().GetComponent<Unit>().InAttackRange(hit.point))))
				{
					SetAction(CurrentAction.attacking);
				}
				// Otherwise, it's just the idle cursor.
				else
				{
					SetAction(CurrentAction.idle);
				}

				// If the user clicked, check what we're currently doing, then do that action
				if (Input.GetMouseButtonDown(0))
				{
					if (currentAction == CurrentAction.selecting)
					{
						if (hit.collider.tag == "Unit")
						{
							turnManager.SetCurrentUnit(hit.collider.gameObject, phase);
						}
					}
					else if (currentAction == CurrentAction.moving)
					{
						turnManager.GetCurrentUnit().GetComponent<Unit>().MoveTo(hit.point);
					}
					else if (currentAction == CurrentAction.attacking)
					{
						turnManager.GetCurrentUnit().GetComponent<Unit>().FireAt(hit.point);
					}
				}
			}
		}
	}

	// If newCurrentAction is different to the currentAction, change the action and cursor.
	private void SetAction(CurrentAction newCurrentAction)
	{
		if (currentAction != newCurrentAction)
		{
			if (newCurrentAction == CurrentAction.idle)
			{
				Cursor.SetCursor(idle, new Vector2(4, 132), CursorMode.Auto);
			}
			else if (newCurrentAction == CurrentAction.selecting)
			{
				Cursor.SetCursor(selecting, new Vector2(16, 140), CursorMode.Auto);
			}
			else if (newCurrentAction == CurrentAction.moving)
			{
				Cursor.SetCursor(moving, new Vector2(125, 125), CursorMode.Auto);
			}
			else if (newCurrentAction == CurrentAction.attacking)
			{
				Cursor.SetCursor(attacking, new Vector2(125, 125), CursorMode.Auto);
			}
			currentAction = newCurrentAction;
		}
	}

	// Set the current phase for units
	public void SetPhase(string newPhase)
	{
		if (newPhase == "Move" || newPhase == "Attack")
		{
			phase = newPhase;
		}
	}
}