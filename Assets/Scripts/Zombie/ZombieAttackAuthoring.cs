using UnityEngine;
using Unity.Entities;

public class ZombieAttackAuthoring : MonoBehaviour
{

    [Header ("Attack")]
    [SerializeField] float attackStrength;
    [SerializeField] float attackCooldown;
    [SerializeField] float attackRange;

    [Header ("Sensing")]
    [SerializeField] float sensoryRange;
    [SerializeField] int considerationChoiceCount;

    class Baker : Baker<ZombieAttackAuthoring>
    {
        public override void Bake (ZombieAttackAuthoring authoring)
        {

            Entity entity = GetEntity (TransformUsageFlags.Dynamic);

            AddComponent (entity, new ZombieAttackDataComponent
            {
                attackStrength = authoring.attackStrength,
                attackCooldown = authoring.attackCooldown,
                attackRange = authoring.attackRange,
            });

            AddComponent (entity, new ZombieAttackTargetingComponent
            {
                sensoryRange = authoring.sensoryRange,
            });

        }
    }

}
