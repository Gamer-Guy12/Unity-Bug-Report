using Unity.Entities;

public struct AttackTargetDataComponent : IComponentData
{

    public float maxHealth;
    public float health;
    public float regenRate;

}
