using System.Collections;
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

    private UnityEvent defending;
    [HideInInspector] public UnityEvent shieldTakeDamage;
    private UnityEvent ShieldBreaking;

    public bool isDefending = false;
    private bool isShieldActive = false;
    
    void Start()
    {
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

    private void DamageShield()
    {
        if (!isDefending) return;

        shieldHealth--;
        if (shieldHealth <= 0)
        {
            ShieldBreaking.Invoke();
        }
    }

    private void ShieldBreak()
    {
        if (!isDefending) return;

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

    private void OnDestroy()
    {
        defending.RemoveAllListeners();
        shieldTakeDamage.RemoveAllListeners();
        ShieldBreaking.RemoveAllListeners();
    }
}
