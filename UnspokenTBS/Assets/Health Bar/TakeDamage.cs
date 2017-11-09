using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    public float damage = 1;
    HealthBar healthBar;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (GameObject.FindWithTag("HealthBar"))
        {
            GameObject enemy = GameObject.FindGameObjectWithTag("HealthBar");
            if (Input.GetMouseButtonDown(0))
            {
                healthBar.TakeDamage(damage);
            }
        }

        if (GameObject.FindWithTag("HealthBar2"))
        {
            if (Input.GetMouseButtonDown(1))
            {
                healthBar.TakeDamage(damage);
            }
        }
    }
}
