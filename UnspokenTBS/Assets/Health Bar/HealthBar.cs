using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public float totalHp = 100;
    public float currentHp; 
    Slider healthSlider; //HealthSlider, needed for creating Health Sliders
    GameObject[] units; 
    public Slider healthBar; //Only needed for instantiation
    static int numOfUnits;

    // Use this for initialization
    void Start ()
    {
        Debug.Log( "started again" );
        currentHp = totalHp;

        BuildUnits();
        Debug.Log( "done done done" );
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (currentHp<=0) {
            Destroy( gameObject );
        }
    }

    public void TakeDamage(float damage)
    {
        currentHp-=damage;
        healthSlider.value=currentHp;
    }

    private void BuildUnits() {
        units=(GameObject.FindGameObjectsWithTag( "Unit"));
        for (int i = numOfUnits; i<units.Length; i++) {
            numOfUnits++;
            GameObject unit = units[i];

            Vector3 position = new Vector3(unit.transform.position.x, unit.transform.position.y+0.75f, unit.transform.position.z);

            InstantiateHealthBar(unit, position);
        }
    }

    private void InstantiateHealthBar( GameObject unit, Vector3 position ) {
        Slider gameObjHealthBar = Instantiate( healthBar, position, Camera.main.transform.rotation ) as Slider;
        unit.AddComponent<HealthBar>().healthSlider=gameObjHealthBar;
        Canvas canvas = FindObjectOfType<Canvas>();
        gameObjHealthBar.transform.SetParent( canvas.transform );
    }
}
