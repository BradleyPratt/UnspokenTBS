using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cube : MonoBehaviour
{
    public int iD;
    public GameObject gameMaster;
    public float moveSpeed = 1;
    public bool movement = false;

    // Use this for initialization
    void Start()
    {
        moveSpeed = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        checkID();
        if (movement)
        {
            transform.Translate(moveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime, 0f, moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime);
        }
    }

    void checkID()
    {
        if (gameMaster.GetComponent<TurnManager>().currentId == iD)
            movement = true;
        else
            movement = false;

    }
}

/*public class cube : MonoBehaviour {

    public float moveSpeed = 1;

	// Use this for initialization
	void Start ()
    {
        moveSpeed = 1f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.Translate(moveSpeed*Input.GetAxis("Horizontal")*Time.deltaTime,0f, moveSpeed*Input.GetAxis("Vertical") * Time.deltaTime);
	}
}*/
