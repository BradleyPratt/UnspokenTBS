using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public float totalHp = 100;
    public float currentHp; 
    public Slider healthSlider; //HealthSlider, needed for creating Health Sliders
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
        /* var wantedPos = Camera.main.WorldToScreenPoint( position );
         transform.position=wantedPos;*/
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
        units=GameObject.FindGameObjectsWithTag( "Unit" );
        Debug.Log( "num of units= " + numOfUnits );
        for (int i = numOfUnits; i<units.Length; i++) {
            numOfUnits++;
            GameObject unit;
            unit=units[i];
            Debug.Log( "i= " + i );
            Debug.Log("units length= " + units.Length );

            Vector3 position = new Vector3(unit.transform.position.x, unit.transform.position.y+0.75f, unit.transform.position.z);

            Debug.Log( "Player Number "+i+" is named "+units[i].name );

            InstantiateHealthBar(unit, position);
            Debug.Log( "done done" );
        }
    }

    private void InstantiateHealthBar( GameObject unit, Vector3 position ) {
        Slider gameObjHealthBar = Instantiate( healthBar, position, Camera.main.transform.rotation ) as Slider;
        unit.AddComponent<HealthBar>().healthSlider=gameObjHealthBar;
        Canvas canvas = FindObjectOfType<Canvas>();
        gameObjHealthBar.transform.SetParent( canvas.transform );

        Debug.Log( "Done" );
    }
}
