using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct MagicWandSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if(!SystemAPI.TryGetSingletonEntity<PlayerTag>(out Entity playerEntity))
        {
            return;
        }
        if(!SystemAPI.TryGetSingletonRW<MagicWandData>(out var magicWand))            
        {
            return;
        }
        if(!SystemAPI.TryGetSingleton<GamePrefabsData>(out var prefabs))
        {
            return;
        }

        float currentTime = (float)SystemAPI.Time.ElapsedTime;

        if (currentTime >= magicWand.ValueRO.NextFireTime)
        {
            float3 playerPos = SystemAPI.GetComponent<LocalTransform>(playerEntity).Position;
            Entity Target = Entity.Null;
            float minDist = float.MaxValue;

            foreach (var (transform, entity) in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<EnemyData>().WithEntityAccess())
            {
                float dist = math.distance(playerPos, transform.ValueRO.Position);

                if (dist < minDist)
                {
                    minDist = dist;
                    Target = entity;
                }
            }

            if (Target != Entity.Null)
            {
                var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

                float3 enemyPos = SystemAPI.GetComponent<LocalTransform>(Target).Position;
                float2 dir = math.normalize(enemyPos.xy - playerPos.xy);

                int projectileCount = magicWand.ValueRO.bulletCount;
                float spreadAngle = 15f;

                for (int i = 0; i < projectileCount; i++)
                {
                    float angleOffset = (i - (projectileCount - 1) * 0.5f) * spreadAngle;
                    float radOffset = math.radians(angleOffset);

                            float2 spreadDir = new float2(
                    dir.x * math.cos(radOffset) - dir.y * math.sin(radOffset),
                    dir.x * math.sin(radOffset) + dir.y * math.cos(radOffset)
                     );

                    Entity bullet = ecb.Instantiate(prefabs.MissilePrefab);

                    ecb.SetComponent(bullet, LocalTransform.FromPosition(playerPos).WithScale(0.1f));

                    ecb.AddComponent(bullet, new ProjectileData { Direction = spreadDir, Speed = magicWand.ValueRO.BulletSpeed, Damage = 10f });
                    ecb.AddComponent(bullet, new LifeTimeData { Value = 5 });
                }

                magicWand.ValueRW.NextFireTime = currentTime + magicWand.ValueRO.FireRate;
                ecb.Playback(state.EntityManager);
                ecb.Dispose();
            }
        }
    }
}
