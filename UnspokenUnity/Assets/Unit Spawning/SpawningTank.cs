using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningTank : MonoBehaviour {
    SpawnTank spawnTank;
    TurnManager turnManager;
    Vector3 movePos;

    ArrayList tankTeam;

    GameObject USBase;
    GameObject USSRBase;
    GameObject boundary;

    Vector3 boundaryRightPos;
    Vector3 boundaryLeftPos;
    Vector3 boundaryForwardPos;
    Vector3 boundaryBackPos;

    string currentTeam;

    bool far = false;

    // Use this for initialization
    void Start () {
        USBase = GameObject.Find("USBase");
        USSRBase = GameObject.Find("USSRBase");



        spawnTank = GameObject.Find("GameManager").GetComponent<SpawnTank>();
        turnManager = GameObject.Find("GameManager").GetComponent<TurnManager>();

        currentTeam = turnManager.GetActiveTeam();

        if (currentTeam == "USA") {
            boundary = USBase;
        } else {
            boundary = USSRBase;
        }

        Vector3 boundaryOffsetX = boundary.transform.right * (boundary.transform.localScale.x / 2f) * -1f;
        Vector3 boundaryOffsetY = boundary.transform.up * (boundary.transform.localScale.y / 2f) * -1f;
        Vector3 boundaryOffsetZ = boundary.transform.forward * (boundary.transform.localScale.z / 2f) * -1f;

        boundaryLeftPos = boundary.transform.position + boundaryOffsetX;
        boundaryRightPos = boundary.transform.position - boundaryOffsetX;
        boundaryBackPos = boundary.transform.position + boundaryOffsetZ;
        boundaryForwardPos = boundary.transform.position - boundaryOffsetZ;

    }

    // Update is called once per frame
    void Update () {
        if (transform.position.z > boundaryForwardPos.z) {
            far = true;
        } else if (transform.position.z < boundaryBackPos.z) {
            far = true;
        } else if (transform.position.x < boundaryLeftPos.x) {
            far = true;
        } else if (transform.position.x > boundaryRightPos.x) {
            far = true;
        } else {
            far = false;
        }

        if (far) {
            foreach (Renderer rend in GetComponentsInChildren<Renderer>()) {
                rend.material.color = Color.red;
            }
        } else {
            foreach (Renderer rend in GetComponentsInChildren<Renderer>()) {
                rend.material.color = Color.white;
            }
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            Vector3 lastPos = movePos;
            if (hit.collider.tag != "Unit" && hit.collider.tag != "SpawningTank" && hit.collider.tag != "MainCamera" &&hit.point.y > 12) {
                movePos = hit.point;
                lastPos = movePos;
            } else {
                movePos = lastPos;
            }
        }

        Vector3 activePos = new Vector3(movePos.x, movePos.y + 2, movePos.z);
        transform.position = activePos;

        if(Input.GetMouseButton(0) && !far) {
            spawnTank.SpawnTankAtPos(activePos);
        } 

        if (Input.GetMouseButton(1)) {
            spawnTank.CancelSpawn();
        }
    }
}
