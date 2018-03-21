using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinScene : MonoBehaviour {

	[SerializeField]
	Sprite USAFlag;

	[SerializeField]
	Sprite USSRFlag;

	Image backgroundImage;
	Text winnerText;

	// Use this for initialization
	void Start () {
		backgroundImage = GetComponentInChildren<Image>();
		winnerText = GetComponentInChildren<Text>();
		string winner = PlayerPrefs.GetString("winner");
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
