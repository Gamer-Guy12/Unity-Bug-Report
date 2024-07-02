using UnityEngine;
using Unity.Entities;

public class TargetAuthoring : MonoBehaviour
{

    [SerializeField] float radius;
    [SerializeField] bool infiniteRange;

    class Baker : Baker<TargetAuthoring>
    {
        public override void Bake (TargetAuthoring authoring)
        {
            
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent (entity, new TargetDataComponent
            {
                radius = authoring.radius,
                infiniteRange = authoring.infiniteRange
            });

        }
    }

}
