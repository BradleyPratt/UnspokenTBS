using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    // Invasion of Josh's code to add class objects that will be used by the money system
    GameObject gameManager;
    Money money;

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
        // Finding the class' in GameObjects within the world
        GameObject gameManager = GameObject.Find("GameManager");
        money = gameManager.GetComponent<Money>();

        // Find all units and add them to their team's list.
        GameObject[] tempUnits = GameObject.FindGameObjectsWithTag("Unit");
		foreach (GameObject tempUnit in tempUnits)
		{
			if (string.Equals(tempUnit.GetComponent<Unit>().GetTeam(), "USA"))
			{
				unitsUSA.Add(tempUnit);
			}
			else if (string.Equals(tempUnit.GetComponent<Unit>().GetTeam(), "USSR"))
			{
				unitsUSSR.Add(tempUnit);
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
				unit.GetComponent<Unit>().ResetUnitTurn();
			}
		}
		else
		{
			currentTeam = "USA";
			foreach (GameObject unit in unitsUSA)
			{
				unit.GetComponent<Unit>().ResetUnitTurn();
			}
		}
		turnCounter++;

        RunCheckpoints();
		UpdateTeamIndicator();
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
	public void RemoveUnit(GameObject deadUnit)
	{
		if (string.Equals(deadUnit.GetComponent<Unit>().GetTeam(), "USA"))
		{
			unitsUSA.Remove(deadUnit);
		}
		else if (string.Equals(deadUnit.GetComponent<Unit>().GetTeam(), "USSR"))
		{
			unitsUSSR.Remove(deadUnit);
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
    void RunCheckpoints()
    {
        GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        foreach (GameObject checkpoint in checkpoints)
        {
            string owner = checkpoint.GetComponent<Checkpoint>().GetCheckpointOwner();
            if (owner == "USA")
            {
                money.SetUSMoney(100);
            }
            if (owner == "USSR")
            {
                money.SetUSSRMoney(100);
            }
        }
    }
}
