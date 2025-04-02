using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Shield : MonoBehaviour, IShield
{
    [Header("Shield Variables")]
    [SerializeField] protected int shieldMaxHealth;
    [SerializeField] protected int Damage;
    protected int shieldHealth;
    
    //Actions
    public UnityAction shieldTakeDamage;
    protected UnityAction ShieldBreaking;
    
    //bools
    protected bool TookDamage;
    
    public abstract void TryDamageShield();
    public abstract void DamageShield(int thisDamage);
    public abstract void TryShieldBreak();
    public abstract void ShieldBreak();
}
