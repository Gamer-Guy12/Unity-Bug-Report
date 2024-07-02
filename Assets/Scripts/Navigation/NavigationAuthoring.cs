using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;

public class NavigationAuthoring : MonoBehaviour
{

    [Header ("Shared")]
    [SerializeField] float3 target;

    [Header ("Move To")]
    [SerializeField] float moveSpeed;
    [SerializeField] float accuracyRadius;

    [Header ("Look At")]
    [SerializeField] float rotateSpeed;
    [SerializeField] float acceptableError;

    class Baker : Baker<NavigationAuthoring>
    {
        public override void Bake (NavigationAuthoring authoring)
        {
            Entity entity = GetEntity (TransformUsageFlags.Dynamic);

            AddComponent (entity, new MoveToComponent
            {
                target = authoring.target,
                moveSpeed = authoring.moveSpeed,
                accuracyRadius = authoring.accuracyRadius
            });

            AddComponent (entity, new LookAtComponent
            {
                target = authoring.target,
                rotateSpeed = authoring.rotateSpeed,
                acceptableError = authoring.acceptableError
            });

            AddComponent (entity, new TargetComponent
            {
                target = authoring.target
            });
        }
    }

}
