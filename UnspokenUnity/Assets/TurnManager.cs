﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{

	// A list each containing all units (and watchtowers) on each side.
	private List<GameObject> unitsUSA = new List<GameObject>();
	private List<GameObject> unitsUSSR = new List<GameObject>();

	// Unit which is currently active.
	private GameObject currentUnit;

	// The current active team. USA or USSR
	private string currentTeam = "USA";

	// Number of current turn.
	private int turnCounter = 1;

	// Use this for initialization
	void Start()
	{

        // Find all units and add them to their team's list.
        GameObject[] tempUnits = GameObject.FindGameObjectsWithTag("Unit");
		foreach (GameObject tempUnit in tempUnits)
		{
			if (string.Equals(tempUnit.GetComponent<Unit>().GetTeam(), "USA"))
			{
				unitsUSA.Add(tempUnit);
				tempUnit.GetComponentInChildren<MiniMapUnitIcon>().SetColor(Color.blue);
			}
			else if (string.Equals(tempUnit.GetComponent<Unit>().GetTeam(), "USSR"))
			{
				unitsUSSR.Add(tempUnit);
				tempUnit.GetComponentInChildren<MiniMapUnitIcon>().SetColor(Color.red);
			}
		}

		// Find all watchtowers and add them to their team's list.
		GameObject[] tempWatchTowers = GameObject.FindGameObjectsWithTag("WatchTower");
		foreach (GameObject tempWatchTower in tempWatchTowers)
		{
			if (string.Equals(tempWatchTower.GetComponent<Unit>().GetTeam(), "USA"))
			{
				unitsUSA.Add(tempWatchTower);
			}
			else if (string.Equals(tempWatchTower.GetComponent<Unit>().GetTeam(), "USSR"))
			{
				unitsUSSR.Add(tempWatchTower);
			}
		}
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void EndTurn()
	{
		Debug.Log("TurnEnded");
		if (currentUnit != null)
		{
			currentUnit.GetComponent<Unit>().SetSelected(false);
			currentUnit = null;
		}
		if (currentTeam == "USA")
		{
			currentTeam = "USSR";
			foreach (GameObject unit in unitsUSSR)
			{
				if (unit == null)
				{
					RemoveUnit(unit);
				} else
				{
					unit.GetComponentInChildren<MiniMapUnitIcon>().SetColor(Color.blue);
				}
			}
			foreach (GameObject unit in unitsUSA)
			{
				if (unit == null)
				{
					RemoveUnit(unit);
				}
				else
				{
				unit.GetComponent<Unit>().ResetUnitTurn();
				unit.GetComponentInChildren<MiniMapUnitIcon>().SetColor(Color.red);
				}
			}
		}
		else
		{
			currentTeam = "USA";
			foreach (GameObject unit in unitsUSA)
			{
				if (unit == null)
				{
					RemoveUnit(unit);
				}
				else
				{
				unit.GetComponentInChildren<MiniMapUnitIcon>().SetColor(Color.blue);
				}
			}
			foreach (GameObject unit in unitsUSSR)
			{
				if (unit == null)
				{
					RemoveUnit(unit);
				}
				else
				{
				unit.GetComponent<Unit>().ResetUnitTurn();
				unit.GetComponentInChildren<MiniMapUnitIcon>().SetColor(Color.red);
				}
			}
		}
		turnCounter++;

        RunCheckpoints();
		gameObject.GetComponent<Money>().UpdateMoneyUI();
		UpdateTeamIndicator();

        Camera.main.GetComponent<CameraScript>().RunEndTurnSwitch(currentTeam, true);
	}

	public void AutoEndTurn()
	{
		bool finished = true;

		if (currentTeam == "USA")
		{
			foreach (GameObject unit in unitsUSA)
			{
				if (!unit.GetComponent<Unit>().HasFinishedTurn())
				{
					finished = false;
					break;
				}
			}
		}
		else
		{
			foreach (GameObject unit in unitsUSSR)
			{
				if (!unit.GetComponent<Unit>().HasFinishedTurn())
				{
					finished = false;
					break;
				}
			}
		}

		if (finished)
		{
			EndTurn();
		}
	}

    // Remove a unit from the turn manager.
    public void RemoveUnit(GameObject deadUnit) {
        if (string.Equals(deadUnit.GetComponent<Unit>().GetTeam(), "USA")) {
            unitsUSA.Remove(deadUnit);
        } else if (string.Equals(deadUnit.GetComponent<Unit>().GetTeam(), "USSR")) {
            unitsUSSR.Remove(deadUnit);
        }
    }  
    
    // add a unit to the turn manager.
    public void AddUnit(GameObject newUnit) {
        if (string.Equals(newUnit.GetComponent<Unit>().GetTeam(), "USA")) {
            unitsUSA.Add(newUnit);
        } else if (string.Equals(newUnit.GetComponent<Unit>().GetTeam(), "USSR")) {
            unitsUSSR.Add(newUnit);
        }
    }

    public void SetCurrentUnit(GameObject unit)
	{
		if ((unit.GetComponent<Unit>().GetTeam() == currentTeam) && !(unit.GetComponent<Unit>().HasFinishedTurn()) && (currentUnit != unit))
		{
			if (currentUnit != null)
			{
				currentUnit.GetComponent<Unit>().SetSelected(false);
			}
			currentUnit = unit;
			currentUnit.GetComponent<Unit>().SetSelected(true);
		}
	}

	public GameObject GetCurrentUnit()
	{
		return currentUnit;
	}

	// Returns the number of units remaining on the specified team, or -1 if a non-existant team is specified.
	public int UnitsRemaining(string team)
	{
		if (team == "USA")
		{
			return unitsUSA.Count;
		}
		else if (team == "USSR")
		{
			return unitsUSSR.Count;
		}
		else
		{
			return -1;
		}
	}

	public int GetTurnCount()
	{
		return turnCounter;
	}

	public string GetActiveTeam()
	{
		return currentTeam;
	}

	public void UpdateTeamIndicator()
	{
		GameObject.FindGameObjectWithTag("TeamIndicator").GetComponent<Text>().text = string.Concat("Current Team: ", currentTeam);
	}

    // Finds checkpoints and adds money accordingly, using the EndTurn function
    private void RunCheckpoints()
    {
        GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        foreach (GameObject checkpoint in checkpoints)
        {
            string owner = checkpoint.GetComponent<Checkpoint>().GetCheckpointOwner();
			Debug.Log(owner);
            if (owner == "USA" && currentTeam == "USSR")
            {
                gameObject.GetComponent<Money>().SetUSMoney(100);
            } else if (owner == "USSR" && currentTeam == "USA")
            {
                gameObject.GetComponent<Money>().SetUSSRMoney(100);
            }
        }
    }
}
