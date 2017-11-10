using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Take damage method
*/
public class TakeDamage : MonoBehaviour
{
    public float damage = 1;
    //HealthBar healthBar;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown( 0 )) { // if left button pressed...
            Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
            RaycastHit hit;

            if (Physics.Raycast( ray, out hit )) {
                HealthBar healthBar = hit.collider.GetComponent<HealthBar>();
                if (hit.transform.gameObject.tag=="Unit") {
                    healthBar.TakeDamage( damage );
                }
            }
        }

        /*if (GameObject.FindWithTag("Unit"))
        {
            if (Input.GetMouseButtonDown(1))
            {
                healthBar.TakeDamage(damage);
            }
        }*/
    }
}
