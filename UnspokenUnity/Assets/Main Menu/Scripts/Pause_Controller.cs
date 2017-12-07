using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause_Controller : MonoBehaviour
{
    public GameObject Pausemenu;
    public GameObject optionmenu;
    //bool isPressed = false;

    public Transform canvas;
    Option optionController;


    void Start()
    {
        optionController = GameObject.Find("Option Controller").GetComponent<Option>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !optionController.optionsOpen)
        {
            Pause();
        }
    }
    public void Pause()
    {
            if (canvas.gameObject.activeInHierarchy == false)
            {
                canvas.gameObject.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                canvas.gameObject.SetActive(false);
                Time.timeScale = 1;
                Debug.Log("it works");
            }
        }

    }

