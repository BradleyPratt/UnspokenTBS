using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Option : MonoBehaviour
{

    void Start()
    {
        activeScreenResIndex = PlayerPrefs.GetInt("screen res index");
        bool isFullscreen = (PlayerPrefs.GetInt("fullscreen") == 1) ? true : false;

        for (int i = 0; i < resolutionToggles.Length; i++)
        {
            resolutionToggles[i].isOn = i == activeScreenResIndex;
        }

        SetFullscreen(isFullscreen);
    }

    // Menu manager for options Menu

    public GameObject PauseHolder;
    public GameObject optionsMenuHolder;
    public Slider[] volumeSliders;
    public Toggle[] resolutionToggles;
    public int[] screenWidths;
    int activeScreenResIndex;
    public bool optionsOpen;

    // switch canvas for main menu and options menu
    public void OptionMenu()
    {
        PauseHolder.SetActive(false);
        optionsMenuHolder.SetActive(true);
        optionsOpen = true;
    }

    public void MainMenu()
    {
        PauseHolder.SetActive(true);
        optionsMenuHolder.SetActive(false);
        optionsOpen = false;
    }

    // setting ingame resolution 
    public void SetScreenResolution(int i)
    {
        if (resolutionToggles[i].isOn)
        {
            activeScreenResIndex = i;
            float aspectRatio = 16 / 9f;
            Screen.SetResolution(screenWidths[i], (int)screenWidths[i] / (int)aspectRatio, false);
        }
    }


    // in fullscreen it will disable all other resolution options to prevent bug
    public void SetFullscreen(bool isFullscreen)
    {
        for (int i = 0; i < resolutionToggles.Length; i++)
        {
            resolutionToggles[i].interactable = !isFullscreen;
        }
        if (isFullscreen)
        {
            Resolution[] allResolutions = Screen.resolutions;
            Resolution maxResolution = allResolutions[allResolutions.Length - 1];
            Screen.SetResolution(maxResolution.width, maxResolution.height, true);
        }
        else
        {
            SetScreenResolution(activeScreenResIndex);
        }
    }

    //When the ingame Audio is finished, please add the audio manager to here in order to adjust the volume in the future
    public void SetMasterVolume(float value)
    {

    }

    public void SetMusicVolume(float value)
    {

    }

    public void SetSfxVolume(float value)
    {

    }

    public void LoadByIndex(int sceneIndex)
    {
        SceneManager.LoadScene("Main Menu Test");
    }
}
