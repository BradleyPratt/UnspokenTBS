using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cube : MonoBehaviour
{
    public int iD = 1;
    public GameObject gameMaster;
    public float moveSpeed = 1;
    public bool movement = false;
    GameObject[] units;
    public static int numOfUnits;

    // Use this for initialization
    void Start()
    {
        moveSpeed = 1f;
        /*units = GameObject.FindGameObjectsWithTag("Unit");
        BuildUnits();*/
    }

    // Update is called once per frame
    void Update()
    {
        units = GameObject.FindGameObjectsWithTag("Unit");
        if (numOfUnits<units.Length)
        {
            BuildUnits();
        }
        if (movement)
        {
            transform.Translate(moveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime, 0f, moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime);
        }

        checkID();
    }

    void checkID()
    {
        if (gameMaster.GetComponent<SwitchTurnManager>().currentId == iD)
            movement = true;
        else
            movement = false;
    }

    private void BuildUnits()
    {
        for (int i = numOfUnits; i < units.Length; i++)
        {
            GameObject unit = units[i];
            Debug.Log(unit.ToString());
            if (unit.GetComponent<cube>()==null)
            {
                unit.AddComponent<cube>().gameMaster = gameMaster;
            }
            unit.GetComponent<cube>().iD = i+1;
            numOfUnits++;
        }
        Debug.Log("NumOfUnits = " + numOfUnits + " Units Length = " + units.Length);
    }

}