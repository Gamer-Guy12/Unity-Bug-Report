using Unity.Entities;

public struct ZombieAttackDataComponent : IComponentData
{

    public float attackStrength;
    public float attackCooldown;
    public float attackRange;
    
}
