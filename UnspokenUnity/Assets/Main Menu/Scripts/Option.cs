using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    public GameObject PauseHolder;
    public GameObject optionsMenuHolder;
    public Slider[] volumeSliders;
    public Toggle fullscreenToggles;
    int activeScreenResIndex;
    public bool optionsOpen;
    public bool fullscreen;
    public int resolutionIndex;
    public int textureQuality;

    public Resolution[] resolution;

   
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

    void OnEnable()
    {
        resolution = Screen.resolutions;
    }

    public void OnFullscreenToggle()
    {

    }
    public void OnResolutionChange()
    {

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
