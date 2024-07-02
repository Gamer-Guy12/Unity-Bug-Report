using Unity.Entities;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;

public partial struct DefenseRegenSystem : ISystem
{

    [BurstCompile]
    public void OnCreate (ref SystemState state)
    {

        state.RequireForUpdate<AttackTargetDataComponent> ();

    }

    [BurstCompile]
    public void OnUpdate (ref SystemState state)
    {

        JobHandle handle = new RegenJob
        {
            deltaTime = SystemAPI.Time.DeltaTime
        }.ScheduleParallel (state.Dependency);
        state.Dependency = handle;

    }

    [BurstCompile]
    public partial struct RegenJob : IJobEntity
    {

        [ReadOnly] public float deltaTime;

        [BurstCompile]
        public void Execute (ref AttackTargetDataComponent data)
        {

            data.health += data.regenRate * deltaTime;

            if (data.health > data.maxHealth) data.health = data.maxHealth;

        }

    }

}
