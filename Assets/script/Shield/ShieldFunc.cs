using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ShieldFunc : MonoBehaviour
{
    [Header("Shield Variables")]
    [SerializeField] private int shieldMaxHealth;
    [SerializeField] private float maxCoolDownTime;
    private float shieldTime;
    private int shieldHealth;
    
    [Header("References")]
    [SerializeField] private GameObject shieldGameObject;
    [SerializeField] private Animator shieldAnimator;
    [SerializeField] private fpscontroller playerControllerScript;
    
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
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            defending.Invoke();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            ShieldBreaking.Invoke();
        }
    }

    private void Defend()
    {
        if (isShieldActive)
            return;

        if (Time.time > shieldTime)
        {
            shieldHealth = shieldMaxHealth;
            isShieldActive = true; 
            Debug.Log("Shield Active");   
        }
        
        
    }

    private void DamageShield()
    {
        
    }
     
    private void ShieldBreak()
    {
        if (!isShieldActive)
            return;
        
        Debug.Log("Shield Breaking");
        isShieldActive = false;
        shieldTime = Time.time + maxCoolDownTime;
    }

    private void OnDestroy()
    {
        defending.RemoveAllListeners();
        shieldTakeDamage.RemoveAllListeners();
        ShieldBreaking.RemoveAllListeners();
    }
}
