using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Money : MonoBehaviour {
    TurnManager turnManager;

    string activeTeam;

    float USMoney = 800;
    float USSRMoney = 800;

    Text moneyText;

	// Use this for initialization
	void Start () {
        turnManager = GameObject.Find("GameManager").GetComponent<TurnManager>();
        moneyText = GameObject.Find("MoneyText").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {

    }

	// true if money adjusted correctly, otherwise false.
    public bool SetUSMoney(float amount)
    {
		if ((USMoney + amount) >= 0)
		{
			USMoney = USMoney + amount;
			UpdateMoneyUI();
			return true;
		}
        return false;
    }

	// true if money adjusted correctly, otherwise false.
	public bool SetUSSRMoney(float amount)
	{
		if ((USSRMoney + amount) >= 0)
		{
			USSRMoney = USSRMoney + amount;
			UpdateMoneyUI();
			return true;
		}
		return false;
    }

    public float GetUSMoney()
    {
        return USMoney;
    }

    public float GetUSSRMoney()
    {
        return USSRMoney;
    }

	public void UpdateMoneyUI()
	{
		if (activeTeam == "USA")
		{
			moneyText.text = USMoney.ToString();
		}
		else
		{
			moneyText.text = USSRMoney.ToString();
		}
	}
}
