using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
    public float speed = 2f;
    public float zoomSpeed = 4f;

    float cameraDistanceMax = 20f;
    float cameraDistanceMin = 5f;
    float cameraDistance = 10f;

    bool forwardBoundary = false;
    bool leftBoundary = false;
    bool downBoundary = false;
    bool rightBoundary = false;

    bool canMove = true;

    GameObject boundary;

    Vector3 focus;
    Vector3 boundaryRightPos;
    Vector3 boundaryLeftPos;
    Vector3 boundaryTopPos;
    Vector3 boundaryBottomPos;
    Vector3 boundaryForwardPos;
    Vector3 boundaryBackPos;

    TurnManager turnManager;

    // Use this for initialization
    void Start() {
        boundary = GameObject.Find("Boundary");
        turnManager = GameObject.Find("GameManager").GetComponent<TurnManager>();

        Vector3 boundaryOffsetX = boundary.transform.right * (boundary.transform.localScale.x / 2f) * -1f;
        Vector3 boundaryOffsetY = boundary.transform.up * (boundary.transform.localScale.y / 2f) * -1f;
        Vector3 boundaryOffsetZ = boundary.transform.forward * (boundary.transform.localScale.z / 2f) * -1f;

        boundaryLeftPos = boundary.transform.position + boundaryOffsetX;
        boundaryRightPos = boundary.transform.position - boundaryOffsetX;
        boundaryBottomPos = boundary.transform.position + boundaryOffsetY;
        boundaryTopPos = boundary.transform.position - boundaryOffsetY;
        boundaryBackPos = boundary.transform.position + boundaryOffsetZ;
        boundaryForwardPos = boundary.transform.position - boundaryOffsetZ;
    }

    // Update is called once per frame
    void Update() {
        if (transform.position.z > boundaryForwardPos.z)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, boundaryForwardPos.z);
        }
        if (transform.position.z < boundaryBackPos.z)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, boundaryBackPos.z);
        }

        if (transform.position.x < boundaryLeftPos.x)
        {
            transform.position = new Vector3(boundaryLeftPos.x, transform.position.y, transform.position.z);
        }
        if (transform.position.x > boundaryRightPos.x)
        {
            transform.position = new Vector3(boundaryRightPos.x, transform.position.y, transform.position.z);
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
            transform.position = new Vector3(transform.position.x + transform.forward.x, transform.position.y, transform.position.z + transform.forward.z);
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
            transform.position = new Vector3(transform.position.x - transform.right.x, transform.position.y, transform.position.z - transform.right.z);
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
            transform.position = new Vector3(transform.position.x - transform.forward.x, transform.position.y, transform.position.z - transform.forward.z);
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
            transform.position = new Vector3(transform.position.x + transform.right.x, transform.position.y, transform.position.z + transform.right.z);
        }

        if (Input.GetAxis("Mouse ScrollWheel")>0 && transform.position.y > boundaryBottomPos.y) {
            transform.position += transform.forward*zoomSpeed;
        }
        else if (Input.GetAxis("Mouse ScrollWheel")<0 && transform.position.y < boundaryTopPos.y) {
            transform.position -= transform.forward*zoomSpeed;
        }

        Vector3 target = new Vector3(transform.position.x + transform.forward.x*10, transform.position.y - 30f, transform.position.z + transform.forward.z*10);

        if (Input.GetKey( KeyCode.Q))
        {
            focus = new Vector3(transform.position.x + transform.forward.x*100, transform.position.y + transform.forward.y*100, transform.position.z + transform.forward.z*100);
            transform.RotateAround(focus, Vector3.up, speed);
        }
        if (Input.GetKey(KeyCode.E))
        {
            focus = new Vector3(transform.position.x + transform.forward.x * 100, transform.position.y + transform.forward.y * 100, transform.position.z + transform.forward.z * 100);
            transform.RotateAround(focus, -Vector3.up, speed);
        }

        cameraDistance += Input.GetAxis("Mouse ScrollWheel") * speed;
        cameraDistance = Mathf.Clamp(cameraDistance, cameraDistanceMin, cameraDistanceMax);
    }
}
