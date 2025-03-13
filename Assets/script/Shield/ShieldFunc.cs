using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ShieldFunc : MonoBehaviour
{
    [Header("Shield Variables")]
    [SerializeField] private float shieldTime;
    [SerializeField] private int shieldMaxHealth;
    private int shieldHealth;
    
    [Header("References")]
    [SerializeField] private GameObject shieldGameObject;
    [SerializeField] private Animator shieldAnimator;
    
    // events
    private UnityEvent defending;
    private UnityEvent shieldTakeDamage;
    private UnityEvent ShieldBreaking;
    
    
    //miscellaneous
    private bool isShieldActive;
    private Camera mainCam;
    
    void Start()
    {
        mainCam = Camera.main;
        
        defending = new UnityEvent();
        shieldTakeDamage = new UnityEvent();
        ShieldBreaking = new UnityEvent();
        
        defending.AddListener(Defend);
        shieldTakeDamage.AddListener(DamageShield);
        ShieldBreaking.AddListener(ShieldBreak);
    }

    
    void Update()
    {
        if (Input.getKeyDown(KeyCode.Mouse2))
        {
            
        }
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

    private void OnDestroy()
    {
        defending.RemoveAllListeners();
        shieldTakeDamage.RemoveAllListeners();
        ShieldBreaking.RemoveAllListeners();
    }
}
