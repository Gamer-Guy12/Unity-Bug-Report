using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Collections;

public partial struct MoveToSystem : ISystem
{

    [BurstCompile]
    public void OnStart (ref SystemState state)
    {
        state.RequireForUpdate<MoveToComponent> ();
    }

    [BurstCompile]
    public void OnUpdate (ref SystemState state)
    { 

        JobHandle handle = new MoveToJob
        {
            deltaTime = SystemAPI.Time.DeltaTime,
        }.ScheduleParallel (state.Dependency);

        state.Dependency = handle;

    }

    [BurstCompile]
    public partial struct MoveToJob : IJobEntity
    {

        [ReadOnly] public float deltaTime;

        [BurstCompile]
        public void Execute (ref LocalTransform transform, in MoveToComponent data)
        {

            float3 flatTar = new float3 (data.target.x, transform.Position.y, data.target.z);

            if (math.distance (flatTar, transform.Position) < data.accuracyRadius) return;

            float3 dir = math.normalize (flatTar - transform.Position);
            float3 change = dir * data.moveSpeed * deltaTime;

            transform = transform.Translate (change);

        }

    }

}
