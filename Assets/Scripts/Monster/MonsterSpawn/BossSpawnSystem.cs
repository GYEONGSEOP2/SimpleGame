using Unity.Burst;
using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct BossSpawnSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        double currentTime = SystemAPI.Time.ElapsedTime;

        using var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
        foreach (var (spawner, entity) in SystemAPI.Query<RefRW<BossSpawnData>>().WithEntityAccess())
        {
            if (!spawner.ValueRO.IsSpawned && currentTime >= spawner.ValueRO.SpawnTime)
            {
                if (!SystemAPI.TryGetSingletonEntity<PlayerTag>(out Entity playerEntity))
                {
                    return;
                }
                float3 playerPos = SystemAPI.GetComponent<LocalTransform>(playerEntity).Position;

                float3 spawnPos = playerPos + new float3(15f, 0f, 0f);

                
                Entity boss = ecb.Instantiate(spawner.ValueRO.BossPrefab);

                ecb.SetComponent(boss, LocalTransform.FromPosition(spawnPos).WithScale(3.0f));

                ecb.AddComponent<BossTag>(boss);

                spawner.ValueRW.IsSpawned = true; 
            }
        }
        ecb.Playback(state.EntityManager);
    }
}
