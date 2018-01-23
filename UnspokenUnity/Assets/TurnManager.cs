using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {

	private List<GameObject> unitsUSA = new List<GameObject>();
	private List<GameObject> unitsUSSR = new List<GameObject>();
	private GameObject currentUnit;
	private int indexUSA = 1, indexUSSR = 0;

	// Use this for initialization
	void Start () {
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

		currentUnit = unitsUSA[0];
		currentUnit.GetComponent<Unit>().SetUnitTurn(true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SwitchUnit()
	{
		if (string.Equals(currentUnit.GetComponent<Unit>().GetTeam(), "USA"))
		{
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
		currentUnit.GetComponent<Unit>().SetUnitTurn(true);
	}

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
