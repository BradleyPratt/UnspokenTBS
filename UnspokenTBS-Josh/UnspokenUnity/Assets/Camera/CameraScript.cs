using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
    public float speed = 0.5f;
    float cameraDistanceMax = 20f;
    float cameraDistanceMin = 5f;
    float cameraDistance = 10f;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        //Vector3 limit = ;
        if (Input.GetKey( KeyCode.RightArrow  ) || Input.GetKey( KeyCode.D ) && transform.position.x <= 24000) {
            transform.position=new Vector3( transform.position.x+speed, transform.position.y, transform.position.z );
        }
        if (Input.GetKey( KeyCode.LeftArrow ) || Input.GetKey( KeyCode.A ) && transform.position.x >= -15000) {
            transform.position=new Vector3( transform.position.x-speed, transform.position.y, transform.position.z );
        }
        if (Input.GetKey( KeyCode.DownArrow ) || Input.GetKey( KeyCode.S ) && transform.position.z >= -5000) {
            transform.position=new Vector3( transform.position.x, transform.position.y, transform.position.z-speed );
        }
        if (Input.GetKey( KeyCode.UpArrow ) || Input.GetKey ( KeyCode.W) && transform.position.z <= 3500) {
            transform.position=new Vector3( transform.position.x, transform.position.y, transform.position.z+speed );
        }
        if (Input.GetAxis("Mouse ScrollWheel")>0 && transform.position.y > 500)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - speed*2, transform.position.z);
            Debug.Log("Ran");
        }
        else if (Input.GetAxis("Mouse ScrollWheel")<0 && transform.position.y < 3500)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + speed*2, transform.position.z);
        }
        cameraDistance += Input.GetAxis("Mouse ScrollWheel") * speed;
        cameraDistance = Mathf.Clamp(cameraDistance, cameraDistanceMin, cameraDistanceMax);
    }
}
