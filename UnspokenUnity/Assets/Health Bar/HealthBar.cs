using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public float turretHP;
    public float largeHP;
    public float mediumHP;
    public float smallHP;
    public float tinyHP;

    public float currentHp; 
    Slider healthSlider; //HealthSlider, needed for creating Health Sliders
    GameObject[] units;
    GameObject[] turrets;
    public Slider healthBar; //Only needed for instantiation
    static int numOfUnits;
    float health;

    // Use this for initialization
    void Start ()
    {
        turretHP = GameObject.Find("GameManager").GetComponent<HealthBar>().turretHP;
        largeHP = GameObject.Find("GameManager").GetComponent<HealthBar>().largeHP;
        mediumHP = GameObject.Find("GameManager").GetComponent<HealthBar>().mediumHP;
        tinyHP = GameObject.Find("GameManager").GetComponent<HealthBar>().tinyHP;
        smallHP = GameObject.Find("GameManager").GetComponent<HealthBar>().smallHP; // This and above to make sure difference instances maintain the same value

        health = 0;

        switch (name) {
            case "SmallTankUS":
            health = smallHP;
            break;

            case "MediumTankUS":
            health = mediumHP;
            break;

            case "LargeTankUS":
            health = largeHP;
            break;

            case "SmallTankUSSR":
            health = smallHP;
            break;

            case "MediumTankUSSR":
            health = mediumHP;
            break;

            case "LargeTankUSSR":
            health = largeHP;
            break;

            case "Turret":
            health = largeHP;
            break;

            case "Tiny":
                health = largeHP;
                break;

            default:
            break;
        }
        currentHp = health;

        //BuildUnits();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //BuildUnits();
        if (numOfUnits < GameObject.FindGameObjectsWithTag("Unit").Length + GameObject.FindGameObjectsWithTag("Turret").Length)
        {
            BuildUnits();
        }

        if (healthSlider != null)
        {
            healthSlider.transform.position = new Vector3(gameObject.transform.position.x+4f, gameObject.transform.position.y+5f, gameObject.transform.position.z);
            healthSlider.transform.rotation = Camera.main.transform.rotation;
        }
    }

    public void TakeDamage(float damage)
    {
        healthSlider.maxValue = health;
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
        units=GameObject.FindGameObjectsWithTag("Unit");
        turrets = GameObject.FindGameObjectsWithTag("Turret");

        for (int i = numOfUnits; i<units.Length; i++) {
            numOfUnits++;
            GameObject unit;
            unit=units[i];

            Vector3 position = new Vector3( unit.transform.position.x-1f, unit.transform.position.y, unit.transform.position.z );

            InstantiateHealthBar( unit, position );
        }

        for (int i = numOfUnits; i < turrets.Length; i++) {
            numOfUnits++;
            GameObject turret;
            turret = turrets[i];

            Vector3 position = new Vector3(turret.transform.position.x - 1f, turret.transform.position.y, turret.transform.position.z);

            InstantiateHealthBar(turret, position);
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