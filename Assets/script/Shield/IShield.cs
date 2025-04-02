public interface IShield
{
    void TryDamageShield();
    void DamageShield(int damageAmount);
    void TryShieldBreak();
    void ShieldBreak();
}

public interface IPhysicalShield
{
    void Defend();
}