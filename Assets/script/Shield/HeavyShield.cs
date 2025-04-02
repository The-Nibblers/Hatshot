using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HeavyShield : Shield, IPhysicalShield
{
    [Header("Shield Variables Instance")]
    [SerializeField] private float maxCoolDownTime;
    
    [Header("References")]
    [SerializeField] private GameObject shieldGameObject;
    [SerializeField] private Animator shieldAnimator;
    [SerializeField] private SFX_Manager shieldSFXManager;

    [Header("Effects")] 
    [SerializeField] private Vector3 minImpactCoordinates;
    [SerializeField] private Vector3 maxImpactCoordinates;
    [SerializeField] private screenShake cameraShake;
    [SerializeField] private ParticleSystem[] shieldImpactParticles;
    
    [Header("UI")]
    [SerializeField] private Image shieldBorder;
    [SerializeField] private Image shieldBar;
    
    // Colors
    private Color brokenShieldColor = Color.red;
    private Color activeShieldColor = Color.white;
    private Color inactiveShieldColor = Color.gray;
    
    // Actions
    public UnityAction Defending;

    // Bools
    [HideInInspector] public bool isDefending;
    private bool isResettingCooldown;
    
    // Arrays
    [SerializeField] private string[] shieldImpactSounds;
    
    private void Start()
    {
        Defending += Defend;
        TryShieldBreaking += TryShieldBreak;
        ShieldTakeDamage += TryDamageShield;
        
        SetShieldUiColor(inactiveShieldColor);
    }

    public void Defend()
    {
        if (isDefending || isResettingCooldown) return;

        shieldAnimator.SetTrigger("Defend");
        shieldHealth = shieldMaxHealth;
        SetShieldUiColor(activeShieldColor);
        UpdateShieldHealthUI();
        isDefending = true;
    }

    public override void TryDamageShield()
    {
        if (!isDefending) return;
        
        DamageShield(damage);
    }

    public override void DamageShield(int damageAmount)
    {
        tookDamage = true;
        UpdateShieldHealthUI();
        shieldHealth -= damageAmount;
        shieldSFXManager.PlayRandomSound(shieldImpactSounds, 1.0f, false);
        shieldImpactParticles[Random.Range(0, shieldImpactParticles.Length)].Play();
        StartCoroutine(cameraShake.Shake(0.2f, 0.3f));
        
        if (shieldHealth <= 0)
        {
            TryShieldBreaking.Invoke();
        }
    }

    public override void TryShieldBreak()
    {
        if (!isDefending) return;
        
        ShieldBreak();
    }

    public override void ShieldBreak()
    {
        shieldAnimator.SetTrigger("Break");
        isDefending = false;
        SetShieldUiColor(inactiveShieldColor);
        
        if (tookDamage)
        {
            StartCoroutine(ResetShieldCooldown());   
        }
    }

    private void UpdateShieldHealthUI()
    {
        shieldBar.fillAmount = (float)shieldHealth / shieldMaxHealth;
    }

    private void SetShieldUiColor(Color color)
    {
        shieldBorder.color = color;
        color.a = 0.392f;
        shieldBar.color = color;
    }

    private IEnumerator ResetShieldCooldown()
    {
        isResettingCooldown = true;
        SetShieldUiColor(brokenShieldColor);

        float elapsedTime = 0f;
        float regenDuration = maxCoolDownTime;
        float shieldStart = shieldHealth;
        float shieldTarget = shieldMaxHealth;

        while (elapsedTime < regenDuration)
        {
            elapsedTime += Time.deltaTime;
            shieldHealth = (int)Mathf.Lerp(shieldStart, shieldTarget, elapsedTime / regenDuration);
            UpdateShieldHealthUI();
            yield return null;
        }

        shieldHealth = shieldMaxHealth;
        UpdateShieldHealthUI();
        SetShieldUiColor(inactiveShieldColor);
        tookDamage = false;
        isResettingCooldown = false;
    }
}
