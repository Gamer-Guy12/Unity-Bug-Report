using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;
using Unity.Jobs;

public partial struct ZombieAttackTargetingSystem : ISystem
{

    EntityQuery query;

    [BurstCompile]
    public void OnCreate (ref SystemState state)
    {

        state.RequireForUpdate<ZombieAttackTargetingComponent> ();
        state.RequireForUpdate<ZombieAttackDataComponent> ();
        state.RequireForUpdate<TargetComponent> ();

        query = new EntityQueryBuilder (Allocator.Temp)
            .WithAll<TargetDataComponent> ()
            .WithAll<LocalTransform> ()
            .Build (ref state);

    }

    [BurstCompile]
    public void OnUpdate (ref SystemState state)
    {

        NativeArray<TargetDataComponent> targets = query.ToComponentDataArray<TargetDataComponent> (Allocator.TempJob);
        NativeArray<LocalTransform> transforms = query.ToComponentDataArray<LocalTransform> (Allocator.TempJob);

        Random random = new Random ((uint) SystemAPI.Time.ElapsedTime * 1000 + 4);

        NativeArray<FloatInt> choices = new NativeArray<FloatInt> (targets.Length, Allocator.TempJob);

        JobHandle handle = new ZombieAttackTargetingJob
        {
            targets = targets,
            transforms = transforms,
            random = random,
            choices = choices
        }.ScheduleParallel (state.Dependency);

        JobHandle disposeHandle = targets.Dispose (handle);
        JobHandle disposeHandle2 = choices.Dispose (disposeHandle);
        state.Dependency = transforms.Dispose (disposeHandle2);

    }

    [BurstCompile]
    public partial struct ZombieAttackTargetingJob : IJobEntity
    {

        [ReadOnly] public NativeArray<TargetDataComponent> targets;
        [ReadOnly] public NativeArray<LocalTransform> transforms;
        [ReadOnly] public Random random;
        [NativeDisableParallelForRestriction] public NativeArray<FloatInt> choices;

        [BurstCompile]
        public void Execute (in ZombieAttackTargetingComponent data, in LocalTransform transform, in ZombieAttackDataComponent attackData, ref TargetComponent target, ref MoveToComponent moveTo)
        {

            if (targets.Length <= 0) return;

            for (int i = 0; i < targets.Length; i++)
            {

                float distance = math.distance (transforms[i].Position, transform.Position);

                choices[i] = new FloatInt
                {
                    val = distance,
                    index = i,
                };

            }

            int best = 0;

            for (int i = 0; i < choices.Length; i++)
            {

                if (i == 0) best = i;

                if (choices[i].val < choices[best].val)
                {

                    best = i;

                    UnityEngine.Debug.Log (choices[i].val + " " + choices[best].val);

                }

            }

            float3 val;

            if (math.distance (transforms[best].Position, transform.Position) <= data.sensoryRange)
            {

                target.target = transforms[best].Position;
                moveTo.accuracyRadius = targets[best].radius + attackData.attackRange;

            } 

        }

    }

}
