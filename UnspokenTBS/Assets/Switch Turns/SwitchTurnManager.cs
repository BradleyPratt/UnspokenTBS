using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTurnManager : MonoBehaviour
{

    public int currentId;

    public void switchID()
    {
        if (currentId == 0)
            currentId = 1;
        else
            currentId = 0;
    }

}
