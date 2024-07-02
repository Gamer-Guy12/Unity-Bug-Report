using Unity.Burst;
using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;

public partial struct ZombieSpawnSystem : ISystem
{

    [BurstCompile]
    public void OnCreate (ref SystemState state)
    {
        state.RequireForUpdate<ZombieSpawnDataBuffer> ();
    }

    [BurstCompile]
    public void OnUpdate (ref SystemState state)
    {

        EntityManager manager = state.EntityManager;
        EntityCommandBuffer ecb = new EntityCommandBuffer (Allocator.Temp);

        DynamicBuffer<ZombieSpawnDataBuffer> buffer = SystemAPI.GetSingletonBuffer<ZombieSpawnDataBuffer> ();
        ZombieSpawnDataComponent data = SystemAPI.GetSingleton<ZombieSpawnDataComponent> ();
        Random random = new Random ((uint) SystemAPI.Time.ElapsedTime * 10 + 4);

        foreach (ZombieSpawnDataBuffer item in buffer)
        {

            // Spawn Zombies
            Entity entity = ecb.Instantiate (data.prefab);
            LocalTransform transform = manager.GetComponentData<LocalTransform> (data.prefab);
            transform.Position = item.pos;
            transform.Rotation = item.rot;
            ecb.SetComponent (entity, transform);

            // Get Random Data
            RandomZombieData zombieData = CalculateRandomData (ref random, data.difficulty);

            MoveToComponent moveTo = manager.GetComponentData<MoveToComponent> (data.prefab);
            moveTo.moveSpeed = zombieData.moveSpeed;
            ecb.SetComponent (entity, moveTo);

            LookAtComponent lookAt = manager.GetComponentData<LookAtComponent> (data.prefab);
            lookAt.rotateSpeed = zombieData.rotationSpeed;
            ecb.SetComponent (entity, lookAt);

            ZombieAttackDataComponent attackData = manager.GetComponentData<ZombieAttackDataComponent> (data.prefab);
            attackData.attackStrength = zombieData.attackStrength;
            attackData.attackCooldown = zombieData.attackCooldown;
            attackData.attackRange = zombieData.attackRange;
            ecb.SetComponent (entity, attackData);

            ZombieAttackTargetingComponent targetingData = manager.GetComponentData<ZombieAttackTargetingComponent> (data.prefab);
            targetingData.sensoryRange = zombieData.attackSensoryRange;
            ecb.SetComponent (entity, targetingData);

        }

        // Clear Data
        EntityQuery query = new EntityQueryBuilder (Allocator.Temp).WithAll<ZombieSpawnDataBuffer> ().Build (state.EntityManager);
        NativeArray<Entity> entities = query.ToEntityArray (Allocator.Temp);

        foreach (Entity entity in entities)
        {

            manager.RemoveComponent (entity, typeof (ZombieSpawnDataBuffer));
            
        }

        ecb.Playback (manager);

        ecb.Dispose ();
        query.Dispose ();
        entities.Dispose ();

    }

    RandomZombieData CalculateRandomData (ref Random random, float difficulty)
    {

        float shiftedDifficulty;

        if (difficulty > 7) shiftedDifficulty = difficulty + random.NextFloat (-(difficulty / 10), difficulty / 10);
        else shiftedDifficulty = difficulty + random.NextFloat (-0.5f, 0.3f);

        float moveSpeed = 3 + (shiftedDifficulty / 50) * 17;
        float rotationSpeed = 3 + (shiftedDifficulty / 50) * 27;

        float attackStrength = 5 + (shiftedDifficulty / 50) * 10;
        float attackCooldown = 1 - (shiftedDifficulty / 50) * 0.9f;
        float attackRange = 3 + (shiftedDifficulty / 50) * 7;

        float attackSensoryRange = 20 + (shiftedDifficulty / 50) * 10;

        return new RandomZombieData
        {
            moveSpeed = moveSpeed,
            rotationSpeed = rotationSpeed,
            attackStrength = attackStrength,
            attackCooldown = attackCooldown,
            attackRange = attackRange,
            attackSensoryRange = attackSensoryRange,
        };

    }

}

public struct RandomZombieData
{

    public float moveSpeed;
    public float rotationSpeed;

    public float attackStrength;
    public float attackCooldown;
    public float attackRange;

    public float attackSensoryRange;

}
