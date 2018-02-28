using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningTank : MonoBehaviour {
    SpawnTank spawnTank;
    TurnManager turnManager;
    Vector3 movePos;

    ArrayList tankTeam;

    string currentTeam;

    bool far = false;

    // Use this for initialization
    void Start () {
        spawnTank = GameObject.Find("GameManager").GetComponent<SpawnTank>();
        turnManager = GameObject.Find("GameManager").GetComponent<TurnManager>();

        currentTeam = turnManager.GetActiveTeam();

        GameObject[] nearTanks = GameObject.FindGameObjectsWithTag("Unit");
        tankTeam = new ArrayList();

        foreach (GameObject tank in nearTanks) {
            if (tank.GetComponent<Unit>().GetTeam() == currentTeam) {
                tankTeam.Add(tank);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        GameObject nearestTank = null;
        float minDist = 1000;
        Vector3 currentPos = transform.position;
        foreach (GameObject tank in tankTeam) {
            float dist = Vector3.Distance(tank.transform.position, currentPos);
            if (dist < minDist) {
                nearestTank = tank;
                minDist = dist;
            }
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        float distance = Vector3.Distance(nearestTank.transform.position, transform.position);
        if (distance > 50) {
            far = true;

            foreach (Renderer rend in GetComponentsInChildren<Renderer>()) {
                rend.material.color = Color.red;
            }
        } else {
            foreach (Renderer rend in GetComponentsInChildren<Renderer>()) {
                rend.material.color = Color.white;
            }
            far = false;
        }

        if (Physics.Raycast(ray, out hit)) {
            Vector3 lastPos = movePos;
            if (hit.collider.tag != "Unit" && hit.collider.tag != "SpawningTank" && hit.point.y > 10) {
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
