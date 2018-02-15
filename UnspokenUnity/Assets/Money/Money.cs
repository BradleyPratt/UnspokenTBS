﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Money : MonoBehaviour {
    TurnManager turnManager;

    string activeTeam;

    float USMoney = 0;
    float USSRMoney = 0;

    Text moneyText;

	// Use this for initialization
	void Start () {
        turnManager = GameObject.Find("GameManager").GetComponent<TurnManager>();
        moneyText = GameObject.Find("MoneyText").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {

    }

    public float SetUSMoney(float amount)
    {
        USMoney = USMoney + amount;
		UpdateMoneyUI();
        return USMoney;
    }

    public float SetUSSRMoney(float amount)
    {
        USSRMoney = USSRMoney + amount;
		UpdateMoneyUI();
		return USSRMoney;
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
