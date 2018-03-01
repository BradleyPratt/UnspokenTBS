using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

			if (hit.collider.tag == "Unit")
			{
				SetAction(CurrentAction.selecting);
			} else if (unitSet && (((hit.collider.tag == "Terrain") && (turnManager.GetCurrentUnit().GetComponent<Unit>().GetTeam() == turnManager.GetActiveTeam())) && (turnManager.GetCurrentUnit().GetComponent<Unit>().InMoveRange(hit.point))))
			{
				SetAction(CurrentAction.moving);
			} else if (unitSet && ((turnManager.GetCurrentUnit().GetComponent<Unit>().GetTeam() == turnManager.GetActiveTeam()) && (turnManager.GetCurrentUnit().GetComponent<Unit>().InAttackRange(hit.point))))
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
						turnManager.SetCurrentUnit(hit.collider.gameObject);
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
		currentAction = newCurrentAction;
	}

	public void SetPhase(string newPhase)
	{
		if (newPhase == "Move" || newPhase == "Attack")
		{
			phase = newPhase;
		}
	}
}