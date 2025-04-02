using UnityEngine.Events;

public interface IShield
{
    void TryDamageShield();
    void DamageShield(int thisDamage);
    void TryShieldBreak();
    void ShieldBreak();
}

public interface IPhysicalShield
{
    void Defend();
}