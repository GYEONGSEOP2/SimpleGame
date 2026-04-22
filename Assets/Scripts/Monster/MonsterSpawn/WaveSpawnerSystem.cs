using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct WaveSpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        if(!SystemAPI.TryGetSingletonEntity<PlayerTag>(out Entity playerEntity))
        {
            return;
        }

        float3 playerPos = SystemAPI.GetComponent<LocalTransform>(playerEntity).Position;

        using var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        foreach (var (waveState, waveBuffer) in SystemAPI.Query<RefRW<WaveStateData>, DynamicBuffer<WaveElement>>())
        {
            if (waveState.ValueRO.CurrentWaveIndex >= waveBuffer.Length) 
            { 
                continue; 
            }

            WaveElement currentWave = waveBuffer[waveState.ValueRO.CurrentWaveIndex];

            waveState.ValueRW.WaveTimer += deltaTime;
            waveState.ValueRW.SpawnTimer -= deltaTime;

            if (waveState.ValueRO.WaveTimer >= currentWave.WaveDuration)
            {
                waveState.ValueRW.CurrentWaveIndex++;
                waveState.ValueRW.WaveTimer = 0f;
                continue;
            }

            if (waveState.ValueRO.SpawnTimer <= 0f)
            {
                waveState.ValueRW.SpawnTimer = currentWave.SpawnInterval;

                for (int i = 0; i < currentWave.SpawnCountPerTick; i++)
                {
                    float angle = waveState.ValueRW.RandomState.NextFloat(0f, math.PI * 2);
                    float distance = 15f;
                    float3 spawnPos = playerPos + new float3(math.cos(angle), math.sin(angle), 0f) * distance;

                    Entity enemy = ecb.Instantiate(currentWave.EnemyPrefab);
                    ecb.SetComponent(enemy, LocalTransform.FromPosition(spawnPos));
                }
            }
        }

        ecb.Playback(state.EntityManager);
    }
}
