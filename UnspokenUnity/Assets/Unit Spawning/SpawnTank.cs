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
    Vector3 spawnPointUS;
    Vector3 spawnPointUSSR;

    // Use this for initialization
    void Awake () {
        GetTanks();

        button = GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);

        //midPoint = new Vector3(-10, 16, -112);
        Vector3 checkPointMid = GameObject.Find("CheckpointMid").transform.position;
        midPoint = new Vector3(checkPointMid.x-5, checkPointMid.y+1, checkPointMid.z);

        turnManager = GameObject.Find("GameManager").GetComponent<TurnManager>();
        money = GameObject.Find("GameManager").GetComponent<Money>();

        //Temporary locations
        spawnPointUS = new Vector3(-300, 16.94f, -120);
        spawnPointUSSR = new Vector3(300, 17, -100);
    }

    private void GetTanks()
    {
        GameObject[] tanks = Resources.FindObjectsOfTypeAll<GameObject>();

        smallTankUS = (GameObject)Resources.Load("SmallTankUSPrefab");
        mediumTankUS = (GameObject)Resources.Load("MediumTankUSPrefab");
        largeTankUS = (GameObject)Resources.Load("LargeTankUSPrefab");
        smallTankUSSR = (GameObject)Resources.Load("SmallTankUSSRPrefab");
        mediumTankUSSR = (GameObject)Resources.Load("MediumTankUSSRPrefab");
        largeTankUSSR = (GameObject)Resources.Load("LargeTankUSSRPrefab");
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
                GameObject newTank = GameObject.Instantiate(largeTankUS, spawnPointUS, largeTankUS.transform.rotation);
                turnManager.GetComponent<TurnManager>().AddUnit(newTank);
            } else if (!isUS && USSRMoney - 800 >= 0) {
                money.SetUSSRMoney(-800);
                GameObject newTank = GameObject.Instantiate(largeTankUSSR, spawnPointUSSR, largeTankUSSR.transform.rotation);
                turnManager.GetComponent<TurnManager>().AddUnit(newTank);
            }

        } else if (button.name == "MediumSpawnButton") {
            Debug.Log("Medium Button Presssed");
            if (isUS && USMoney - 400 >= 0) {
                money.SetUSMoney(-400);
                GameObject newTank = GameObject.Instantiate(mediumTankUS, spawnPointUS, mediumTankUS.transform.rotation);
                turnManager.GetComponent<TurnManager>().AddUnit(newTank);
            } else if (!isUS && USSRMoney - 400 >= 0) {
                money.SetUSSRMoney(-400);
                GameObject newTank = GameObject.Instantiate(mediumTankUSSR, spawnPointUSSR, mediumTankUSSR.transform.rotation);
                turnManager.GetComponent<TurnManager>().AddUnit(newTank);
            }
        }
        else if (button.name == "SmallSpawnButton") {
            Debug.Log("Small Button Presssed");
            if (isUS && USMoney - 200 >= 0) {
                money.SetUSMoney(-200);
                GameObject newTank = GameObject.Instantiate(smallTankUS, spawnPointUS, smallTankUS.transform.rotation);
                turnManager.GetComponent<TurnManager>().AddUnit(newTank);
            } else if(!isUS && USSRMoney - 200 >= 0) {
                money.SetUSSRMoney(-200);
                GameObject newTank = GameObject.Instantiate(smallTankUSSR, spawnPointUSSR, smallTankUSSR.transform.rotation);
                turnManager.GetComponent<TurnManager>().AddUnit(newTank);
            }
        }
        else {
            Debug.Log("Button doesnt have correct name");
        }
    }
}
