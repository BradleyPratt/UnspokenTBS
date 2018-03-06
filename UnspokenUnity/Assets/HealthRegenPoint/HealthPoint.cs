using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPoint : MonoBehaviour
{

    public float maxHealth = 100;
    public float regeneration = 5;
    float health = 100;

    void Start()
    {
        health = maxHealth;
    }


    void Update()
    {
        if (health < maxHealth)
        {
            health += regeneration * Time.deltaTime;
            if (health > maxHealth)
            {
                health = maxHealth;
            }
        }
    }


    void OnGUI()
    {
        if (health < maxHealth)
        {
            Color tempColour = GUI.color;
            GUI.color = Color.red;
            GUI.Label(new Rect((Screen.width / 2) - 70, (Screen.height / 2) - 20, 140, 40), "Taking Damage");
            GUI.color = tempColour;
        }
    }
}
