using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ShieldFunc : MonoBehaviour
{
    [Header("Shield Variables")]
    [SerializeField] private int shieldMaxHealth;
    [SerializeField] private float maxCoolDownTime;
    [SerializeField] private int Damage;
    private int shieldHealth;

    [Header("References")]
    [SerializeField] private GameObject shieldGameObject;
    [SerializeField] private Animator shieldAnimator;
    [SerializeField] private fpscontroller playerControllerScript;
    [SerializeField] private AudioSource[] shieldImpactSounds;

    [Header("Effects")] 
    [SerializeField] private Vector3 minImpactCoordinates;
    [SerializeField] private Vector3 maxImpactCoordinates;
    [SerializeField] private screenShake cameraShake;
    [SerializeField] private ParticleSystem[] shieldImpactParticles;
    
    [Header("ui")]
    [SerializeField] private Image shieldBorder;
    [SerializeField] private Image shieldBar;

    private AudioSource shieldAudioSource;
    
    //colors
    private Color brokenShieldColor = Color.red;
    private Color activeShieldColor = Color.white;
    private Color inactiveShieldColor = Color.gray;
    
    
    //delegates
    private UnityAction defending;
    public UnityAction shieldTakeDamage;
    private UnityAction ShieldBreaking;

    //bools
    [HideInInspector] public bool isDefending;
    private bool IsRessetingCooldown;
    private bool TookDamage;
    
    void Start()
    {
        defending+=Defend;
        ShieldBreaking+=TryShieldBreak;
        shieldTakeDamage+=TryDamageShield;
        
        SetShieldUiColor(inactiveShieldColor);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            defending.Invoke();
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            ShieldBreaking.Invoke();
        }
    }

    private void Defend()
    {
        if (isDefending || IsRessetingCooldown) return;

        shieldAnimator.SetTrigger("Defend");
        shieldHealth = shieldMaxHealth;
        SetShieldUiColor(activeShieldColor);
        UpdateShieldHealthUI();
        isDefending = true;
    }

    private void TryDamageShield()
    {
        if (!isDefending) return;
        
        DamageShield(Damage);
    }

    private void DamageShield(int thisDamage)
    {
        TookDamage = true;
        UpdateShieldHealthUI();
        shieldHealth -= thisDamage;
        shieldImpactSounds[Random.Range(0,shieldImpactSounds.Length)].Play();
        shieldImpactParticles[Random.Range(0, shieldImpactParticles.Length)].Play();
        StartCoroutine(cameraShake.Shake(0.2f, 0.3f));
        
        if (shieldHealth <= 0)
        {
            ShieldBreaking.Invoke();
        }
    }
    
    private void TryShieldBreak()
    {
        if (!isDefending) return;

        ShieldBreak();
    }
    private void ShieldBreak()
    {
        shieldAnimator.SetTrigger("Break");

        isDefending = false;

        SetShieldUiColor(inactiveShieldColor);
        
        if (TookDamage)
        {
            StartCoroutine(ResetShieldCooldown());   
        }
    }

    private void UpdateShieldHealthUI()
    {
        shieldBar.fillAmount = (float)shieldHealth / shieldMaxHealth;
    }

    private void SetShieldUiColor(Color thisColor)
    {
        shieldBorder.color = thisColor;
        thisColor.a = 0.39215686274f;
        shieldBar.color = thisColor;
        
    }

    private IEnumerator ResetShieldCooldown()
    {
        IsRessetingCooldown = true;

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
        TookDamage = false;
        IsRessetingCooldown = false;
    }
}
