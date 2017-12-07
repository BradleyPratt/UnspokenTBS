using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WatchTowerHealth : MonoBehaviour {
    public float watchtowerHealth;
    //public GameObject watchtower;
    public Slider slider;
    //GameObject[] watchTowers;
    float towerHealth;
    public float currentHp;

    // Use this for initialization
    void Start () {
        currentHp = watchtowerHealth;
        slider.maxValue = watchtowerHealth;
    }

    // Update is called once per frame
    void Update () {
        slider.value = currentHp;
    }

    public void WatchTowerTakeDamage(float damage)
    {
        currentHp -= damage;
        slider.value = currentHp;
    }

    /*private void BuildWatchtowers()
    {
        GameObject USSlider = GameObject.Find("USSlider");
        GameObject USSRSlider = GameObject.Find("USSRSlider");
        USSlider = (Slider) USSlider;
        watchtowerUS.GetComponent<WatchTowerHealth>().slider = USSlider;
        watchtowerUSSR.GetComponent<WatchTowerHealth>().slider = USSRSlider;
    }*/
}
