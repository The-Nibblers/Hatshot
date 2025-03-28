using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ShieldFunc : MonoBehaviour
{
    [Header("Shield Variables")]
    [SerializeField] private int shieldMaxHealth;
    [SerializeField] private float maxCoolDownTime;
    [SerializeField] private int Damage;
    private float shieldTime;
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

    private AudioSource shieldAudioSource;
    
    private UnityAction defending;
    public UnityAction shieldTakeDamage;
    private UnityAction ShieldBreaking;

    public bool isDefending = false;
    private bool isShieldActive = false;
    
    void Start()
    {
        defending+=Defend;
        ShieldBreaking+=TryShieldBreak;
        shieldTakeDamage+=TryDamageShield;
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
        if (isDefending || isShieldActive) return;

        shieldAnimator.SetTrigger("Defend");
        shieldHealth = shieldMaxHealth;
        isDefending = true;
    }

    private void TryDamageShield()
    {
        if (!isDefending) return;
        
        DamageShield(Damage);
    }

    private void DamageShield(int thisDamage)
    {
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
        isShieldActive = true;

        StartCoroutine(ResetShieldCooldown());
    }

    private IEnumerator ResetShieldCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        shieldTime = Time.time + maxCoolDownTime;
        isShieldActive = false;
    }
}
