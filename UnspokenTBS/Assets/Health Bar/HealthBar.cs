using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public static float totalHp = 100;
    public float currentHp;
    public Slider healthSlider;

    // Use this for initialization
    void Start ()
    {
        currentHp = totalHp;
	}
	
	// Update is called once per frame
	void Update ()
    {
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        healthSlider.value = currentHp;
    }
   
}
