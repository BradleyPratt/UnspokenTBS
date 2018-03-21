using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinScene : MonoBehaviour {

	// Flags for the win screen's background
	[SerializeField]
	Sprite USAFlag;

	[SerializeField]
	Sprite USSRFlag;

	// Components we change
	Image backgroundImage;
	Text winnerText;

	// Use this for initialization
	void Start () {
		// Set the components
		backgroundImage = GetComponentInChildren<Image>();
		winnerText = GetComponentInChildren<Text>();

		// Find out who won from the game preferences.
		string winner = PlayerPrefs.GetString("winner");

		// Determine who won, place a message to this effect on screen, and set their flag as the background.
		if (winner == "USA")
		{
			backgroundImage.sprite = USAFlag;
			winnerText.text = "Winner: USA";
		} else if (winner == "USSR")
		{
			backgroundImage.sprite = USSRFlag;
			winnerText.text = "Winner: USSR";
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
