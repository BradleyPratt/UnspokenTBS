using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Manager : MonoBehaviour {

    public GameObject mainMenuHolder;
    public GameObject optionsMenuHolder;
    public Slider[] volumeSliders;
    public Toggle fullscreenToggle;
    public Dropdown resolutionDropdown;
    public Button applyButton;
    public Dropdown textureQualityDropdown;

    public Resolution[] resolution;
    public Option option;



    // Menu manager for options Menu


    // switch canvas for main menu and options menu
    public void OptionMenu()
    {
        mainMenuHolder.SetActive(false);
        optionsMenuHolder.SetActive(true);
    }

    public void MainMenu()
    {
        mainMenuHolder.SetActive(true);
        optionsMenuHolder.SetActive(false);
    }

    void OnEnable()
    {
        option = new Option();

       fullscreenToggle.onValueChanged.AddListener(delegate { OnFullscreenToggle(); });
       resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
       
        applyButton.onClick.AddListener(delegate { onApplyButtonClick(); });

        resolution = Screen.resolutions;
        foreach(Resolution resolution in resolution)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData(resolution.ToString()));
        }

        LoadSettings();
    }

    public void OnFullscreenToggle()
    {
      option.fullscreen =  Screen.fullScreen = fullscreenToggle.isOn;
    }

    public void OnResolutionChange()
    {
        Screen.SetResolution(resolution[resolutionDropdown.value].width, resolution[resolutionDropdown.value].height, Screen.fullScreen);
    }


    public void onApplyButtonClick()
    {
        SaveSettings();
        mainMenuHolder.SetActive(true);
        optionsMenuHolder.SetActive(false);
    }

    public void SaveSettings()
    {
        string jsonData = JsonUtility.ToJson(option, true);
        File.WriteAllText(Application.persistentDataPath + "/option.json", jsonData);

    }

    public void LoadSettings()
    {
        option = JsonUtility.FromJson<Option>(Application.persistentDataPath + "/option.json");

        resolutionDropdown.value = option.resolutionIndex;
        fullscreenToggle.isOn = option.fullscreen;
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
}
