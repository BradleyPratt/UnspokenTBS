using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {

	// A list each containing all units (and watchtowers) on each side.
	private List<GameObject> unitsUSA = new List<GameObject>();
	private List<GameObject> unitsUSSR = new List<GameObject>();

	// Unit which is currently active.
	private GameObject currentUnit;

	// The position of the next unit on each team.
	private int indexUSA = 1, indexUSSR = 0;

	// Use this for initialization
	void Start () {

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

		// Set the first unit to have it's turn.
		currentUnit = unitsUSA[0];
		currentUnit.GetComponent<Unit>().SetUnitTurn(true);
	}
	
	// Update is called once per frame
	void Update () {

	}

	// Switch to the next unit in
	public void SwitchUnit()
	{
		// Remember, if the current unit is on one team, the next should be from the opposing team.
		if (string.Equals(currentUnit.GetComponent<Unit>().GetTeam(), "USA"))
		{
			// if the next unit would be in a position in the list which doesn't exist, loop back to the start.
			if (indexUSSR < unitsUSSR.Count)
			{
				currentUnit = unitsUSSR[indexUSSR];
				indexUSSR++;
			} else
			{
				indexUSSR = 0;
				currentUnit = unitsUSSR[indexUSSR];
				indexUSSR++;
			}
		}
		else if (string.Equals(currentUnit.GetComponent<Unit>().GetTeam(), "USSR"))
		{
			if (indexUSA < unitsUSA.Count)
			{
				currentUnit = unitsUSA[indexUSA];
				indexUSA++;
			}
			else
			{
				indexUSA = 0;
				currentUnit = unitsUSA[indexUSA];
				indexUSA++;
			}
		}
		// Tell the new currentUnit to have it's turn now.
		currentUnit.GetComponent<Unit>().SetUnitTurn(true);
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
}
