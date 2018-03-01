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
                PlaceTank(smallTankUS);
            } else if (!isUS && USSRMoney - 200 >= 0) {
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
                PlaceTank(mediumTankUS);
            } else if (!isUS && USSRMoney - 400 >= 0) {
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
                PlaceTank(largeTankUS);
            } else if (!isUS && USSRMoney - 800 >= 0) {
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

        GameObject[] tanks = GameObject.FindGameObjectsWithTag("Unit");
        GameObject USTank = null;
        GameObject USSRTank = null;


        if (isUS) {
            foreach (GameObject t in tanks) {
                if (t.GetComponent<Unit>().GetTeam() == "USA") {
                    USTank = t;
                }
            }
            Debug.Log(USTank.name);
            spawningTank = GameObject.Instantiate(tank.transform.gameObject, USTank.transform.position, tank.transform.rotation);
        } else {
            foreach (GameObject t in tanks) {
                if (t.GetComponent<Unit>().GetTeam() == "USSR") {
                    USSRTank = t;
                }
            }
            spawningTank = GameObject.Instantiate(tank.transform.gameObject, USSRTank.transform.position, tank.transform.rotation);
        }

        spawningTank.name = "SpawningTank";
        spawningTank.tag = "SpawningTank";
        Destroy(spawningTank.GetComponent<Unit>());
        Destroy(spawningTank.GetComponent<HealthBar>());
        spawningTank.AddComponent<SpawningTank>();
    }

    public void SpawnTankAtPos(Vector3 pos) {
        float USMoney = money.GetUSMoney();
        float USSRMoney = money.GetUSSRMoney();

        spawnLocation = pos;
        spawnLocationFound = true;

        spawning = false;
        turnManager.SetTankSpawning(spawning);

        Destroy(spawningTank.gameObject);

		// We create a temporary variable and store the instantiated object there.
		GameObject newTank = GameObject.Instantiate(tempTank, pos, tempTank.transform.rotation);
		// Then pass it to the turn manager. (If we pass the TurnManager tempTank, it gets a reference to the prefab instead of the new tank.)
        turnManager.GetComponent<TurnManager>().AddUnit(newTank);

        Debug.Log(tempTank.name);

        switch (tempTank.name) {
            case "SmallTankUSPrefab":
            money.SetUSMoney(-200);
            break;

            case "MediumTankUSPrefab":
            money.SetUSMoney(-400);
            break;

            case "LargeTankUSPrefab":
            money.SetUSMoney(-800);
            break;

            case "SmallTankUSSRPrefab":
            money.SetUSSRMoney(-200);
            break;

            case "MediumTankUSSRPrefab":
            money.SetUSSRMoney(-400);
            break;

            case "LargeTankUSSRPrefab":
            money.SetUSSRMoney(-800);
            break;

            default:
            break;

        }
    }

    public void CancelSpawn() {
        Destroy(spawningTank.gameObject);

        spawnLocationFound = false;
        spawning = false;
    }
}
