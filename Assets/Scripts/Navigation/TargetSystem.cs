using Unity.Entities;
using Unity.Burst;
using Unity.Jobs;

public partial struct TargetSystem : ISystem
{

    [BurstCompile]
    public void OnStart (ref SystemState state)
    {
        state.RequireForUpdate<TargetComponent> ();
    }

    [BurstCompile]
    public void OnUpdate (ref SystemState state)
    {

        JobHandle handle = new TargetJob
        {

        }.ScheduleParallel (state.Dependency);
        state.Dependency = handle;

    }

    [BurstCompile]
    public partial struct TargetJob : IJobEntity
    {

        [BurstCompile]
        public void Execute (in TargetComponent data, ref MoveToComponent moveTo, ref LookAtComponent lookAt)
        {

            moveTo.target = data.target;
            lookAt.target = data.target;

        }

    }

}
