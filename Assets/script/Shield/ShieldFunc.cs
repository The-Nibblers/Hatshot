using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldFunc : MonoBehaviour
{
    [Header("Shield Variables")]
    [SerializeField] private float shieldTime;
    [SerializeField] private int shieldMaxHealth;
    private int shieldHealth;
    
    [Header("References")]
    [SerializeField] private GameObject shieldGameObject;
    
    // events
    
    
    //miscellaneous
    private bool isShieldActive;
    private Camera mainCam;
    
    void Start()
    {
        mainCam = Camera.main;
    }

    
    void Update()
    {
        
    }

    private void Defend()
    {
        
    }

    private void DamageShield()
    {
        
    }
     
    private void ShieldBreak()
    {
        
    }
}
