using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    public float damage = 1;
    HealthBar healthBar;
    GameObject[] units;

	// Use this for initialization
	void Start () {
        units=GameObject.FindGameObjectsWithTag( "Unit" );
        //healthBar=units.GetComponent<HealthBar>();

        for (int i = 0; i<units.Length; i++) {
            GameObject unit;
            unit = units[i];

            Debug.Log( "Player Number "+i+" is named "+units[i].name );

            healthBar=unit.GetComponent<HealthBar>();
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (GameObject.FindWithTag("Unit"))
        {
            if (Input.GetMouseButtonDown(0))
            {
                healthBar.TakeDamage(damage);
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
