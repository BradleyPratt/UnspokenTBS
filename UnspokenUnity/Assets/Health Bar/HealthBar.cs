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
        currentHp = totalHp;

        BuildUnits();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //BuildUnits();
        if (numOfUnits < GameObject.FindGameObjectsWithTag("Unit").Length)
        {
            BuildUnits();
            Debug.Log("Ran buildunits()");
        }

        if (healthSlider != null)
        {
            healthSlider.transform.position = new Vector3(gameObject.transform.position.x+4f, gameObject.transform.position.y+5f, gameObject.transform.position.z);
            healthSlider.transform.rotation = Camera.main.transform.rotation;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHp-=damage;
        healthSlider.value = currentHp;
		if (currentHp <= 0)
		{
			gameObject.tag = ("Untagged");
			numOfUnits--;
			GameObject.FindGameObjectWithTag("GameManager").GetComponent<TurnManager>().RemoveUnit(gameObject);
			gameObject.GetComponent<Unit>().UnitKilled();
		}
	}

    private void BuildUnits() {
        units=GameObject.FindGameObjectsWithTag( "Unit" );

        for (int i = numOfUnits; i<units.Length; i++) {
            numOfUnits++;
            GameObject unit;
            unit=units[i];

            Vector3 position = new Vector3( unit.transform.position.x-1f, unit.transform.position.y, unit.transform.position.z );

            InstantiateHealthBar( unit, position );
        }
    }

    private void InstantiateHealthBar( GameObject unit, Vector3 position ) {
        Slider gameObjHealthBar = Instantiate( healthBar, position, Camera.main.transform.rotation ) as Slider;
        unit.AddComponent<HealthBar>().healthSlider=gameObjHealthBar;
        unit.GetComponent<HealthBar>().healthBar=healthBar;
        gameObjHealthBar.GetComponent<HealthSlider>().slider = gameObjHealthBar;
        Canvas canvas = GameObject.FindGameObjectWithTag("CameraCanvas").GetComponent<Canvas>();
        gameObjHealthBar.transform.SetParent( canvas.transform );
    }
}
