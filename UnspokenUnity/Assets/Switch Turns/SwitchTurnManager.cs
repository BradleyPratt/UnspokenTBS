using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTurnManager : MonoBehaviour
{
    int units;
    public int currentId = 1;

    public void switchID()
    {
        units = cube.numOfUnits;
        if (currentId < units)
            currentId++;
        else
        {
            currentId = 1;
        }
        Debug.Log("CurrentID = " + currentId);
        Debug.Log("Units = " + units);
    }
}
