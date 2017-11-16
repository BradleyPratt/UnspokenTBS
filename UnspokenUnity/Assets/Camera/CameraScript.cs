using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
    public float speed = 0.5f;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKey( KeyCode.RightArrow  ) || Input.GetKey( KeyCode.D )) {
            transform.position=new Vector3( transform.position.x+speed, transform.position.y, transform.position.z );
        }
        if (Input.GetKey( KeyCode.LeftArrow ) || Input.GetKey( KeyCode.A )) {
            transform.position=new Vector3( transform.position.x-speed, transform.position.y, transform.position.z );
        }
        if (Input.GetKey( KeyCode.DownArrow ) || Input.GetKey( KeyCode.S )) {
            transform.position=new Vector3( transform.position.x, transform.position.y, transform.position.z-speed );
        }
        if (Input.GetKey( KeyCode.UpArrow ) || Input.GetKey ( KeyCode.W)) {
            transform.position=new Vector3( transform.position.x, transform.position.y, transform.position.z+speed );
        }
    }
}
