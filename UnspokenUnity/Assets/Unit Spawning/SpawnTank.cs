using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnTank : MonoBehaviour {
    Button smallButton;
    Button mediumButton;
    Button largeButton;

    TurnManager turnManager;
    Money money;

    GameObject largeTankUS;
    GameObject mediumTankUS;
    GameObject smallTankUS;

    GameObject largeTankUSSR;
    GameObject mediumTankUSSR;
    GameObject smallTankUSSR;

    GameObject tempTank;
    GameObject spawningTank;

    Vector3 midPoint;
    Vector3 spawnPointUS;
    Vector3 spawnPointUSSR;
    Vector3 spawnLocation;

    bool isUS = true;
    bool spawning = false;
    bool spawnLocationFound = false;

    // Use this for initialization
    void Awake () {
        GetTanks();

        smallButton = GameObject.Find("SmallSpawnButton").GetComponent<Button>();
        mediumButton = GameObject.Find("MediumSpawnButton").GetComponent<Button>();
        largeButton = GameObject.Find("LargeSpawnButton").GetComponent<Button>();

        smallButton.onClick.AddListener(SmallTaskOnClick);
        mediumButton.onClick.AddListener(MediumTaskOnClick);
        largeButton.onClick.AddListener(LargeTaskOnClick);

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
        spawningTank = (GameObject)Resources.Load("SpawningTank");
    }

    private void SmallTaskOnClick() {
        float USMoney = money.GetUSMoney();
        float USSRMoney = money.GetUSSRMoney();

        if (turnManager.GetActiveTeam() == "USA") {
            isUS = true;
        } else {
            isUS = false;
        }

        if (smallButton.name == "SmallSpawnButton") {
            Debug.Log("Small Button Presssed");
            if (isUS && USMoney - 200 >= 0) {
                money.SetUSMoney(-200);
                PlaceTank(smallTankUS);
            } else if (!isUS && USSRMoney - 200 >= 0) {
                money.SetUSSRMoney(-200);
                PlaceTank(smallTankUSSR);
            }
        }
    }

    private void MediumTaskOnClick() {
        float USMoney = money.GetUSMoney();
        float USSRMoney = money.GetUSSRMoney();

        if (turnManager.GetActiveTeam() == "USA") {
            isUS = true;
        } else {
            isUS = false;
        }

        if (mediumButton.name == "MediumSpawnButton") {
            Debug.Log("Medium Button Presssed");
            if (isUS && USMoney - 400 >= 0) {
                money.SetUSMoney(-400);
                PlaceTank(mediumTankUS);
            } else if (!isUS && USSRMoney - 400 >= 0) {
                money.SetUSSRMoney(-400);
                PlaceTank(mediumTankUSSR);
            }
        }
    }

    private void LargeTaskOnClick() {
        if (!spawning) {
            float USMoney = money.GetUSMoney();
            float USSRMoney = money.GetUSSRMoney();

            if (turnManager.GetActiveTeam() == "USA") {
                isUS = true;
            } else {
                isUS = false;
            }

            Debug.Log("Large Button Presssed");
            if (isUS && USMoney - 800 >= 0) {
                money.SetUSMoney(-800);
                PlaceTank(largeTankUS);
            } else if (!isUS && USSRMoney - 800 >= 0) {
                money.SetUSSRMoney(-800);
                PlaceTank(largeTankUSSR);

            }
        }

    }

    void Update() {
        
    }

    void PlaceTank(GameObject tank) {
        tempTank = tank;
        spawning = true;
        turnManager.SetTankSpawning(spawning);

        spawningTank = GameObject.Instantiate(tank.transform.gameObject, new Vector3(), tank.transform.rotation);
        spawningTank.name = "SpawningTank";
        spawningTank.tag = "SpawningTank";
        Destroy(spawningTank.GetComponent<Unit>());
        spawningTank.AddComponent<SpawningTank>();
    }

    public void SpawnTankAtPos(Vector3 pos) {
        spawnLocation = pos;
        spawnLocationFound = true;

        spawning = false;
        turnManager.SetTankSpawning(spawning);

        Destroy(spawningTank.gameObject);

        GameObject.Instantiate(tempTank, pos, tempTank.transform.rotation);
        turnManager.GetComponent<TurnManager>().AddUnit(tempTank);
    }
}
