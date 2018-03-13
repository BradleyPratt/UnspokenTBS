using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
    public float rotateSpeed = 2f;
    public float zoomSpeed = 4f;
    public float speed = 2f;

    float maxSpeed;
    float minSpeed;

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

    Vector3 spawnPointUS;
    Vector3 spawnPointUSSR;

    TurnManager turnManager;

    bool cameraMovement = false;
    Vector3 cameraTarget;

    float lerpTime = 1f;
    float currentLerpTime;
    Quaternion initialRotation;

    Resolution res;

    // Use this for initialization
    void Start() {
        res = Screen.currentResolution;
        if (res.refreshRate == 60)
            QualitySettings.vSyncCount = 1;
        if (res.refreshRate == 120)
            QualitySettings.vSyncCount = 2;

        spawnPointUS = new Vector3(-300, 50, -170);
        spawnPointUSSR = new Vector3(300, 50, -100);

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

        maxSpeed = speed * 2;
        minSpeed = speed;

        initialRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update() {
        if (canMove) {
            if (transform.position.z > boundaryForwardPos.z) {
                transform.position = new Vector3(transform.position.x, transform.position.y, boundaryForwardPos.z);
            }
            if (transform.position.z < boundaryBackPos.z) {
                transform.position = new Vector3(transform.position.x, transform.position.y, boundaryBackPos.z);
            }

            if (transform.position.x < boundaryLeftPos.x) {
                transform.position = new Vector3(boundaryLeftPos.x, transform.position.y, transform.position.z);
            }
            if (transform.position.x > boundaryRightPos.x) {
                transform.position = new Vector3(boundaryRightPos.x, transform.position.y, transform.position.z);
            }

            if (Input.GetKey(KeyCode.LeftShift)) {
                if (speed <= maxSpeed) {
                    speed = maxSpeed;
                }
            } else {
                speed = minSpeed;
            }

            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
                transform.position += new Vector3(transform.forward.x * speed, 0, transform.forward.z * speed);
            }
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
                transform.position += new Vector3(-transform.right.x * speed, 0, -transform.right.z * speed);
            }
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
                transform.position += new Vector3(-transform.forward.x * speed, 0, -transform.forward.z * speed);
            }
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
                transform.position += new Vector3(transform.right.x * speed, 0, transform.right.z * speed);
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0 && transform.position.y > boundaryBottomPos.y) {
                transform.position += transform.forward * zoomSpeed;
            } else if (Input.GetAxis("Mouse ScrollWheel") < 0 && transform.position.y < boundaryTopPos.y) {
                transform.position -= transform.forward * zoomSpeed;
            }

            //Vector3 target = new Vector3(transform.position.x + transform.forward.x * 10, transform.position.y - 30f, transform.position.z + transform.forward.z * 10);

            float cameraValue = 100 * (transform.position.y / 100 );
            if (Input.GetKey(KeyCode.Q)) {
                focus = new Vector3(transform.position.x + transform.forward.x * cameraValue, transform.position.y + transform.forward.y * cameraValue, transform.position.z + transform.forward.z * cameraValue);
                transform.RotateAround(focus, -Vector3.up, rotateSpeed);
            }
            if (Input.GetKey(KeyCode.E)) {
                focus = new Vector3(transform.position.x + transform.forward.x * cameraValue, transform.position.y + transform.forward.y * cameraValue, transform.position.z + transform.forward.z * cameraValue);
                transform.RotateAround(focus, Vector3.up, rotateSpeed);
            }

            if (Input.GetMouseButton(2)) {
                Cursor.visible = false;
                float mouseX = Input.GetAxis("Mouse X");
                focus = new Vector3(transform.position.x + transform.forward.x * 100, transform.position.y + transform.forward.y * 100, transform.position.z + transform.forward.z * 100);
                transform.RotateAround(focus, Vector3.up, rotateSpeed*mouseX);
            } else {
                Cursor.visible = true;
            }
        }

        currentLerpTime += Time.deltaTime * 0.25f;
        if (currentLerpTime > lerpTime) {
            currentLerpTime = lerpTime;
        }

        if (cameraMovement) {
            canMove = false;
            float perc = currentLerpTime / lerpTime;
            transform.position = Vector3.Lerp(transform.position, cameraTarget, perc);
            transform.rotation = Quaternion.Lerp(transform.rotation, initialRotation, perc);
            if (cameraTarget == transform.position) {
                cameraMovement = false;
            }
        } else {
            canMove = true;
        }
    }

    // Camera Focus method for panning the camera to a specific point
    public void CameraFocus(Vector3 targetPos) {
        currentLerpTime = 0f;

        bool moving = true;
        cameraMovement = moving;
        GameObject[] nearTanks = GameObject.FindGameObjectsWithTag("Unit");

        if (targetPos == new Vector3(0,0,0)) {
            string team = turnManager.GetActiveTeam();

            if (team == "USA") {
                GameObject USTank = null;
                for (int i = 0; i < nearTanks.Length; i++) {
                    if (nearTanks[i].GetComponent<Unit>().GetTeam() == "USA") {
                        USTank = nearTanks[i];
                    } 
                }
                if (USTank == null) {
                    Debug.Log("No USA tanks, why this");
                }
                Vector3 USTankPos = USTank.transform.position;
                cameraTarget = new Vector3(USTankPos.x, USTankPos.y + 50, USTankPos.z - 50);
            } else {
                GameObject USSRTank = null;
                for (int i = 0; i < nearTanks.Length; i++) {
                    if (nearTanks[i].GetComponent<Unit>().GetTeam() == "USSR") {
                        USSRTank = nearTanks[i];
                    }
                }
                if (USSRTank == null) {
                    Debug.Log("No USSR tanks, why this");
                }
                Vector3 USSRTankPos = USSRTank.transform.position;
                cameraTarget = new Vector3(USSRTankPos.x, USSRTankPos.y + 50, USSRTankPos.z - 50);
            }
        } else {
            cameraTarget = new Vector3(targetPos.x, targetPos.y + 50, targetPos.z - 50);
        }
    }
}
