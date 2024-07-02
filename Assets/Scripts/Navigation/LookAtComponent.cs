using Unity.Entities;
using Unity.Mathematics;

public struct LookAtComponent : IComponentData
{

    public float3 target;

    public float rotateSpeed;
    public float acceptableError;

}
