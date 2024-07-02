using Unity.Entities;

public struct ZombieSpawnDataComponent : IComponentData
{

    public Entity prefab;
    public int difficulty;
    
}
