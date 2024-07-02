using UnityEngine;
using Unity.Entities;

public class AttackTargetDataAuthoring : MonoBehaviour
{

    [SerializeField] float maxHealth;
    [SerializeField] float regenRate;

    class Baker : Baker<AttackTargetDataAuthoring>
    {
        public override void Bake (AttackTargetDataAuthoring authoring)
        {
            
            Entity entity = GetEntity(TransformUsageFlags.Renderable);

            AddComponent (entity, new AttackTargetDataComponent
            {
                maxHealth = authoring.maxHealth,
                health = authoring.maxHealth,
                regenRate = authoring.regenRate
            });

        }
    }

}
