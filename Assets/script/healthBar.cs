using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{
    private const float MAX_HEALTH = 100f;

    public Text healthtext;

    public float currentHealth;

    private Image healthbar;
    // Start is called before the first frame update
    void Start()
    {
        healthbar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        healthtext.text = Convert.ToString(currentHealth);
        currentHealth = FindObjectOfType<fpscontroller>().health;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
        healthbar.fillAmount = currentHealth / MAX_HEALTH;
    }
}
