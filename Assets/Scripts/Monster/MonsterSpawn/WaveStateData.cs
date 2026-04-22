using Unity.Entities;
using UnityEngine;

[InternalBufferCapacity(8)]
public struct WaveElement : IBufferElementData
{
    public Entity EnemyPrefab;
    public float WaveDuration;
    public float SpawnInterval;
    public int SpawnCountPerTick;
}

public struct WaveStateData : IComponentData
{
    public int CurrentWaveIndex;
    public float WaveTimer;
    public float SpawnTimer;
    public Unity.Mathematics.Random RandomState;
}
