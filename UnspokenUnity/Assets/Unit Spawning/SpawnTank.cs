using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnTank : MonoBehaviour {
    Button button;

    TurnManager turnManager;
    Money money;

    GameObject largeTankUS;
    GameObject mediumTankUS;
    GameObject smallTankUS;

    GameObject largeTankUSSR;
    GameObject mediumTankUSSR;
    GameObject smallTankUSSR;

    Vector3 midPoint;

    // Use this for initialization
    void Awake () {
        button = GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);

        midPoint = new Vector3(-10, 16, -112);

        turnManager = GameObject.Find("GameManager").GetComponent<TurnManager>();
        money = GameObject.Find("GameManager").GetComponent<Money>();

        GetTanks();
    }

    private void GetTanks()
    {
        GameObject[] tanks = Resources.FindObjectsOfTypeAll<GameObject>();

        for(int i = 0; i < tanks.Length; i++) {
            if (tanks[i].name=="LargeTankUSPrefab")
            {
                largeTankUS = tanks[i];
            }
            if (tanks[i].name == "MediumTankUSPrefab")
            {
                mediumTankUS = tanks[i];
            }
            if (tanks[i].name == "SmallTankUSPrefab")
            {
                smallTankUS = tanks[i];
            }
            if (tanks[i].name == "LargeTankUSSRPrefab")
            {
                largeTankUSSR = tanks[i];
            }
            if (tanks[i].name == "MediumTankUSSRPrefab")
            {
                mediumTankUSSR = tanks[i];
            }
            if (tanks[i].name == "SmallTankUSSRPrefab")
            {
                smallTankUSSR = tanks[i];
            }
        }
    }

    private void TaskOnClick()
    {
        bool isUS;
        float USMoney = money.GetUSMoney();
        float USSRMoney = money.GetUSSRMoney();

        if (turnManager.GetActiveTeam() == "USA") {
            isUS = true;
        } else {
            isUS = false;
        }

        if (button.name == "LargeSpawnButton") {
            Debug.Log("Large Button Presssed");
            if (isUS && USMoney-800 >= 0) {
                money.SetUSMoney(-800);
                GameObject.Instantiate(largeTankUS, midPoint, largeTankUS.transform.rotation);
            } else if (!isUS && USSRMoney - 800 >= 0) {
                money.SetUSSRMoney(-800);
                GameObject.Instantiate(largeTankUSSR, midPoint, largeTankUSSR.transform.rotation);
            }

        } else if (button.name == "MediumSpawnButton") {
            Debug.Log("Medium Button Presssed");
            if (isUS && USMoney - 400 >= 0) {
                money.SetUSMoney(-400);
                GameObject.Instantiate(mediumTankUS, midPoint, mediumTankUS.transform.rotation);
            } else if (!isUS && USSRMoney - 400 >= 0) {
                money.SetUSSRMoney(-400);
                GameObject.Instantiate(mediumTankUSSR, midPoint, mediumTankUSSR.transform.rotation);
            }
        }
        else if (button.name == "SmallSpawnButton") {
            Debug.Log("Small Button Presssed");
            if (isUS && USMoney - 200 >= 0) {
                money.SetUSMoney(-200);
                GameObject.Instantiate(smallTankUS, midPoint, smallTankUS.transform.rotation);
            } else if(!isUS && USSRMoney - 200 >= 0) {
                money.SetUSSRMoney(-200);
                GameObject.Instantiate(smallTankUSSR, midPoint, smallTankUSSR.transform.rotation);
            }
        }
        else {
            Debug.Log("Button doesnt have correct name");
        }
    }
}
