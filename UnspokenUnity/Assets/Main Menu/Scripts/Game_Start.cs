using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game_Start : MonoBehaviour {
    public Button myButton;
    public Button myButton1;

    void Start()
    {
        Button btn = myButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        Debug.Log("It Works");
        LoadByIndex(1);
    }

    public void LoadByIndex(int sceneIndex)
    {
        SceneManager.LoadScene("Pause Test Scene");
    }

    public void BacktoMenu(int sceneIndex)
    {
        SceneManager.LoadScene("Main Menu Test");
    }

}
