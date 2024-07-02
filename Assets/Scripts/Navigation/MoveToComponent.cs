using Unity.Entities;
using Unity.Mathematics;

public struct MoveToComponent : IComponentData
{

    public float3 target;

    public float moveSpeed;

    public float accuracyRadius;

}
