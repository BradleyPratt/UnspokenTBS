using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
    public float speed = 1f;
    float cameraDistanceMax = 20f;
    float cameraDistanceMin = 5f;
    float cameraDistance = 10f;

    GameObject currentUnit;
    GameObject lastUnit;

    TurnManager turnManager;

    // Use this for initialization
    void Start() {
        turnManager = GameObject.Find("GameManager").GetComponent<TurnManager>();
    }

    // Update is called once per frame
    void Update() {
        Debug.Log(currentUnit);
        Debug.Log(turnManager.GetCurrentUnit());
        currentUnit = turnManager.GetCurrentUnit();

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) && transform.position.z <= 3500)
        {
            //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + speed);// * speed;
            //Camera.main.transform.Translate(new Vector3(transform.position.y, transform.position.y, Vector3.forward.z) * speed);
            transform.position = new Vector3(transform.position.x + transform.forward.x, transform.position.y, transform.position.z + transform.forward.z);
        }
        if (Input.GetKey( KeyCode.LeftArrow ) || Input.GetKey( KeyCode.A ) && transform.position.x >= -15000) {
            transform.position = new Vector3(transform.position.x - transform.right.x, transform.position.y, transform.position.z - transform.right.z);
        }
        if (Input.GetKey( KeyCode.DownArrow ) || Input.GetKey( KeyCode.S ) && transform.position.z >= -5000) {
            transform.position = new Vector3(transform.position.x - transform.forward.x, transform.position.y, transform.position.z - transform.forward.z);
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) && transform.position.x <= 24000)
        {
            transform.position = new Vector3(transform.position.x + transform.right.x, transform.position.y, transform.position.z + transform.right.z);
        }
        if (Input.GetAxis("Mouse ScrollWheel")>0 && transform.position.y > 2) {
            transform.position = new Vector3(transform.position.x, transform.position.y - speed*2, transform.position.z);
        }
        else if (Input.GetAxis("Mouse ScrollWheel")<0 && transform.position.y < 100) {
            transform.position = new Vector3(transform.position.x, transform.position.y + speed*2, transform.position.z);
        }

        if (Input.GetKey( KeyCode.Q))
        {
            transform.eulerAngles -= new Vector3(0, speed, 0);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.eulerAngles += new Vector3(0, speed, 0);
        }

        cameraDistance += Input.GetAxis("Mouse ScrollWheel") * speed;
        cameraDistance = Mathf.Clamp(cameraDistance, cameraDistanceMin, cameraDistanceMax);

        if (false)//(currentUnit != null && currentUnit != lastUnit)
        {
            Vector3 camPos = new Vector3 (currentUnit.transform.position.x, currentUnit.transform.position.y+3000, currentUnit.transform.position.z);
            transform.position = camPos;
        }

        lastUnit = currentUnit;
    }
}
