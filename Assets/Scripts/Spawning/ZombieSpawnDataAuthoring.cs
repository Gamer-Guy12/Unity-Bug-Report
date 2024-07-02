using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

public class ZombieSpawnDataAuthoring : MonoBehaviour
{

    [Header ("Spawning")]
    [SerializeField] float spawnRadius;
    [SerializeField] int unitCount;
    [SerializeField] GameObject prefab;

    [Header ("Attack")]
    [SerializeField] int difficulty;

    class Baker : Baker<ZombieSpawnDataAuthoring>
    {
        public override void Bake (ZombieSpawnDataAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            Entity prefab = GetEntity (authoring.prefab, TransformUsageFlags.Dynamic);
            AddComponent (entity, new ZombieSpawnDataComponent
            {
                prefab = prefab,
                difficulty = authoring.difficulty
            });
            DynamicBuffer<ZombieSpawnDataBuffer> buffer = AddBuffer<ZombieSpawnDataBuffer> (entity);

            for (int i = 0; i < authoring.unitCount; i++)
            {
                float angle = ((float) i / (float) authoring.unitCount) * math.TAU;
                float3 pos = new float3 (math.sin (angle) * authoring.spawnRadius, 1f, math.cos (angle) * authoring.spawnRadius);

                float3 dir = math.normalize (new float3 (0f, 1f, 0f) - pos);
                quaternion rot = quaternion.LookRotation (dir, math.up ());

                buffer.Add (new ZombieSpawnDataBuffer
                {
                    pos = pos,
                    rot = rot,
                });
            }
        }
    }
}
