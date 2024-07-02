using Unity.Entities;
using Unity.Burst;
using Unity.Collections;

public partial struct CoreCountSystem : ISystem, ISystemStartStop
{

    [BurstCompile]
    public void OnCreate (ref SystemState state)
    {

        state.RequireForUpdate<CoreCountComponent> ();

    }

    [BurstCompile]
    public void OnStartRunning (ref SystemState state)
    {

        EntityQuery query = new EntityQueryBuilder (Allocator.Temp)
            .WithAll<CoreTag> ()
            .Build (ref state);

        NativeArray<CoreTag> tags = query.ToComponentDataArray<CoreTag> (Allocator.Temp);

        RefRW<CoreCountComponent> coreCount = SystemAPI.GetSingletonRW<CoreCountComponent> ();
        coreCount.ValueRW.count = (uint) tags.Length;

    }

    public void OnStopRunning (ref SystemState state)
    {

        return;

    }
}
