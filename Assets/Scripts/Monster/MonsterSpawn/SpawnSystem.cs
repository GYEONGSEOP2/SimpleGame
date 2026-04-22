using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


[BurstCompile]
public partial struct SpawnSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.TryGetSingletonEntity<PlayerTag>(out var playerEntity))
        {
            return;
        }
        if (!SystemAPI.TryGetSingletonRW<SpawnerData>(out var spawner))
        {
            return;
        }

        if(!SystemAPI.TryGetSingleton<DifficultData>(out var difficult))
        {
            return;
        }


        double currentTime = SystemAPI.Time.ElapsedTime;
        uint randomSeed = (uint)(currentTime * 10000) + 1;
        var random = new Unity.Mathematics.Random(randomSeed);

        if (currentTime >= spawner.ValueRO.NextSpawnTime)
        {
            float scaledRate = math.max(0.2f, spawner.ValueRO.SpawnRate / difficult.DifficultyFactor);

            float3 playerPos = SystemAPI.GetComponent<LocalTransform>(playerEntity).Position;
            
            float randomAngle = random.NextFloat(0, math.PI * 2);

            float3 spawnPos = playerPos + new float3(math.cos(randomAngle) * spawner.ValueRO.SpawnRadius, math.sin(randomAngle) * spawner.ValueRO.SpawnRadius, 0);

            using var ecb = new EntityCommandBuffer(Allocator.Temp);

            Entity newEnemy = ecb.Instantiate(spawner.ValueRO.Enemy);
            ecb.SetComponent(newEnemy, LocalTransform.FromPosition(spawnPos));

            if (SystemAPI.HasComponent<HealthData>(spawner.ValueRO.Enemy))
            {
                var baseHealth = SystemAPI.GetComponent<HealthData>(spawner.ValueRO.Enemy);

                baseHealth.CurrentHP *= difficult.DifficultyFactor;

                baseHealth.MaxHP *= difficult.DifficultyFactor; 

                ecb.SetComponent(newEnemy, baseHealth);
            }

            spawner.ValueRW.NextSpawnTime = currentTime + scaledRate;

            ecb.Playback(state.EntityManager);
        }
    }
}