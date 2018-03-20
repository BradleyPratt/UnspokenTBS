using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticData : MonoBehaviour {

	static string winningTeam;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetWinningTeam(string winner)
	{
		winningTeam = winner;
	}

	public string GetWinningTeam()
	{
		return winningTeam;
	}
}
