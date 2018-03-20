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

	StaticData staticData;

	// Use this for initialization
	void Start () {
		backgroundImage = GetComponentInChildren<Image>();
		winnerText = GetComponentInChildren<Text>();
		staticData = GameObject.FindGameObjectWithTag("GameManager").GetComponent<StaticData>();

		if (staticData.GetWinningTeam() == "USA")
		{
			backgroundImage.sprite = USAFlag;
			winnerText.text = "Winner: USA";
		} else if (staticData.GetWinningTeam() == "USSR")
		{
			backgroundImage.sprite = USSRFlag;
			winnerText.text = "Winner: USA";
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
