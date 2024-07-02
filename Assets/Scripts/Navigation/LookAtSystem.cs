using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Collections;

public partial struct LookAtSystem : ISystem
{

    [BurstCompile]
    public void OnStart (ref SystemState state)
    {
        state.RequireForUpdate<LookAtComponent> ();
    }

    [BurstCompile]
    public void OnUpdate (ref SystemState state)
    {

        JobHandle handle = new LookAtJob
        {
            deltaTime = SystemAPI.Time.DeltaTime
        }.ScheduleParallel (state.Dependency);
        state.Dependency = handle;

    }

    [BurstCompile]
    public partial struct LookAtJob : IJobEntity
    {

        [ReadOnly] public float deltaTime;

        [BurstCompile]
        public void Execute (ref LocalTransform transform, in LookAtComponent data)
        {

            float3 flatTar = new float3 (data.target.x, transform.Position.y, data.target.z);
            float3 dir = math.normalize (flatTar - transform.Position);

            if (math.dot (dir, transform.Forward ()) > 1 - data.acceptableError) return;

            quaternion dest = quaternion.LookRotation (dir, math.up ());

            float change = math.angle (transform.Rotation, dest);

            transform = transform.RotateY (change * data.rotateSpeed / 10 * deltaTime);
        }

    }

}
