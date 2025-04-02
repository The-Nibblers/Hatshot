using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public abstract class Shield : MonoBehaviour, IShield
{
    [Header("Shield Variables")]
    [SerializeField] protected int shieldMaxHealth;
    [SerializeField] protected int damage;
    
    protected int shieldHealth;
    
    // Actions
    public UnityAction ShieldTakeDamage;
    public UnityAction TryShieldBreaking;
    
    // Bools
    protected bool tookDamage;
    
    // Functions
    public abstract void TryDamageShield();
    public abstract void DamageShield(int damageAmount);
    public abstract void TryShieldBreak();
    public abstract void ShieldBreak();
}