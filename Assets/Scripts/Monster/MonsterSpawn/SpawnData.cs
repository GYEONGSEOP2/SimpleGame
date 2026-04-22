using Unity.Entities;
using UnityEngine;

public struct SpawnerData : IComponentData
{
    public Entity Enemy;
    public Entity ExpGem;

    public float SpawnRate;
    public double NextSpawnTime;
    public float SpawnRadius;
}
