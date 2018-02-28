using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningTank : MonoBehaviour {
    SpawnTank spawnTank;
    Vector3 movePos;


    // Use this for initialization
    void Start () {
        spawnTank = GameObject.Find("GameManager").GetComponent<SpawnTank>();
	}
	
	// Update is called once per frame
	void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

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

        if(Input.GetMouseButton(0)) {
            spawnTank.SpawnTankAtPos(activePos);
        }
    }
}
