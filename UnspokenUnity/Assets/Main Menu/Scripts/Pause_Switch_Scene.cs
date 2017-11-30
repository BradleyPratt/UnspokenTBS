using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause_Switch_Scene : MonoBehaviour {

    public GameObject mainMenuHolder;
    public GameObject BackToGame;

    public void pauseMenu()
    {
        mainMenuHolder.SetActive(false);
        BackToGame.SetActive(true);
    }

    public void BackGame()
    {
        mainMenuHolder.SetActive(true);
        BackToGame.SetActive(false);
    }
}
