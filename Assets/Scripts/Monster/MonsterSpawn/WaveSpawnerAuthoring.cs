using System;
using Unity.Entities;
using UnityEngine;

public class WaveSpawnerAuthoring : MonoBehaviour
{
    [Serializable]
    public struct WaveConfig
    {
        public GameObject EnemyPrefab;
        public float WaveDuration;
        public float SpawnInterval;
        public int SpawnCountPerTick;
    }

    public WaveConfig[] Waves;

    class Baker : Baker<WaveSpawnerAuthoring>
    {
        public override void Bake(WaveSpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new WaveStateData
            {
                CurrentWaveIndex = 0,
                WaveTimer = 0,
                SpawnTimer = 0,
                RandomState = Unity.Mathematics.Random.CreateFromIndex((uint)entity.Index),
            });

            DynamicBuffer<WaveElement> waveBuffer = AddBuffer<WaveElement>(entity);

            foreach (var wave in authoring.Waves)
            {
                waveBuffer.Add(new WaveElement
                {
                    EnemyPrefab = GetEntity(wave.EnemyPrefab, TransformUsageFlags.Dynamic),
                    WaveDuration = wave.WaveDuration,
                    SpawnInterval = wave.SpawnInterval,
                    SpawnCountPerTick = wave.SpawnCountPerTick,
                });
            }
        }
    }
}
