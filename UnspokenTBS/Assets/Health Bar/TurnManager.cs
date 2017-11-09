using UnityEngine;
using System.Collections;

public class TurnManager : MonoBehaviour
{
    public int turnStatus = 0;
    public int turnCount = 1;


    // Use this for initialization
    void Start()
    {

    }

    void CurrentTurn()
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

        turnCount++;

    }

    public int GetTurnCount()
    {
        return turnCount;
    }

}