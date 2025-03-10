using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{
    private const float MAX_HEALTH = 100f;

    [SerializeField] private Text healthtext;
    
    private float currentHealth;

    private Image healthbar;

    private fpscontroller playerscript;
    // Start is called before the first frame update
    void Start()
    {
        healthbar = GetComponent<Image>();
        playerscript = FindObjectOfType<fpscontroller>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = playerscript.returnHealth();
        healthtext.text = Convert.ToString(currentHealth);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
        healthbar.fillAmount = currentHealth / MAX_HEALTH;
    }
}
