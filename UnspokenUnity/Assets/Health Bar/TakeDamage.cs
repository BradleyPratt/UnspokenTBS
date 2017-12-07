using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Take damage method
*/
public class TakeDamage : MonoBehaviour
{
    public float damage = 1;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown( 0 )) { 
            Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
            RaycastHit hit;

            if (Physics.Raycast( ray, out hit )) {
                HealthBar healthBar = hit.collider.GetComponent<HealthBar>();
                WatchTowerHealth towerHealth = hit.collider.GetComponent<WatchTowerHealth>();
                if (hit.transform.gameObject.tag=="Unit") {
                    healthBar.TakeDamage( damage );
                }
                if (hit.transform.gameObject.tag=="WatchTower")
                {
                    towerHealth.WatchTowerTakeDamage(50f);
                }
            }
        }
    }
}
