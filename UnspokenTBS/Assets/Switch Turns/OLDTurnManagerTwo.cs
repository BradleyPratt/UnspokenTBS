using UnityEngine;
using System.Collections;

public class OLDTurnManagerTwo : MonoBehaviour
{
    public int turnStatus = 0;

    public GameObject Cube;
    public GameObject Cube2;

    // Use this for initialization
    void Start()
    {
        Cube = GameObject.Find("Cube");
        Cube2 = GameObject.Find("Cube2");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))   
        {
            if (Cube.GetComponent<cube>().movement)   
            {
                Cube.GetComponent<Rigidbody>().isKinematic= true;
                Cube2.GetComponent<Rigidbody>().isKinematic = false;
            }
            if (Input.GetKeyDown(KeyCode.P)) 
            {
                if (Cube.GetComponent<BoxCollider>().enabled == false)  
                {
                    Cube.GetComponent<Rigidbody>().isKinematic = false;
                    Cube2.GetComponent<Rigidbody>().isKinematic = true;
                }
            }

        }
    }      
     
    /*void CurrentTurn()
    {
        if (turnStatus == 0)
        {
            playerTurn();
        }
        else if (turnStatus == 1)
        {
            playerTwoTurn();
        }
    }


    void playerTurn()
    {
        Debug.Log("Player's turn");
    }

    void playerTwoTurn()
    {
        Debug.Log("Player Two's turn");
    }

    public void TurnChange()
    {
        if (turnStatus == 0)
        {
            turnStatus = 1;
        }
        else if (turnStatus == 1)
        {
            turnStatus = 0;
        }
    }*/
}