using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour {

	[SerializeField]
	Texture2D idle, selecting, moving, attacking;

	TurnManager turnManager;

	CurrentAction currentAction = CurrentAction.idle;
	private string phase = "Move";

	enum CurrentAction
	{
		idle,
		selecting,
		moving,
		attacking
	}

	// Use this for initialization
	void Start() {
		Cursor.SetCursor(idle, new Vector2(0, 0), CursorMode.Auto);
		turnManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TurnManager>();
	}

	// Update is called once per frame
	void Update() {
		if (Time.timeScale != 0)
		{
			
			bool unitSet = false;
			if (turnManager.GetCurrentUnit() != null)
			{
				unitSet = true;
			}

			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Physics.Raycast(ray.origin, ray.direction, hitInfo: out hit, maxDistance: Mathf.Infinity);

			bool colliderIsUnit = false;
			if (hit.collider.gameObject.GetComponent<Unit>() != null)
			{
				colliderIsUnit = true;
			}

			if (hit.collider.tag == "Unit" && colliderIsUnit && (turnManager.GetActiveTeam() == hit.collider.gameObject.GetComponent<Unit>().GetTeam()))
			{
				SetAction(CurrentAction.selecting);
			} else if (unitSet && (((hit.collider.tag == "Terrain") && (turnManager.GetCurrentUnit().GetComponent<Unit>().GetTeam() == turnManager.GetActiveTeam())) && (turnManager.GetCurrentUnit().GetComponent<Unit>().GetPhase() == "Move") && (turnManager.GetCurrentUnit().GetComponent<Unit>().InMoveRange(hit.point))))
			{
				SetAction(CurrentAction.moving);
			} else if (unitSet && ((turnManager.GetCurrentUnit().GetComponent<Unit>().GetTeam() == turnManager.GetActiveTeam()) && (turnManager.GetCurrentUnit().GetComponent<Unit>().GetPhase() == "Attack") && (turnManager.GetCurrentUnit().GetComponent<Unit>().InAttackRange(hit.point))))
			{
				SetAction(CurrentAction.attacking);
			} else
			{
				SetAction(CurrentAction.idle);
			}

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


	private void SetAction(CurrentAction newCurrentAction)
	{
		if (currentAction != newCurrentAction)
		{
			if(newCurrentAction == CurrentAction.idle)
			{
				Cursor.SetCursor(idle, Vector2.zero, CursorMode.Auto);
			}
			else if (newCurrentAction == CurrentAction.selecting)
			{
				Cursor.SetCursor(selecting, new Vector2(0, 0), CursorMode.Auto);
			} else if (newCurrentAction == CurrentAction.moving)
			{
				Cursor.SetCursor(moving, new Vector2(125, 125), CursorMode.Auto);
			} else if (newCurrentAction == CurrentAction.attacking)
			{
				Cursor.SetCursor(attacking, new Vector2(125, 125), CursorMode.Auto);
			}
			currentAction = newCurrentAction;
		}
	}

	public void SetPhase(string newPhase)
	{
		if (newPhase == "Move" || newPhase == "Attack")
		{
			phase = newPhase;
		}
	}
}