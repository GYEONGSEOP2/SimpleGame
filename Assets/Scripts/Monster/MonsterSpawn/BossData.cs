using Unity.Entities;
using UnityEngine;

public struct BossTag : IComponentData { }

public struct BossSpawnData : IComponentData 
{
    public Entity BossPrefab;
    public double SpawnTime;
    public bool IsSpawned;
}