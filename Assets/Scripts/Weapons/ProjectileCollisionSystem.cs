using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;

[BurstCompile]
public partial struct ProjectileCollisionSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        using var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        float collisionRadius = 1f * 1f;

        foreach(var (projectileTransform, projectile, projectileEntity) in SystemAPI.Query< RefRO<LocalTransform>,RefRO<ProjectileData>>().WithEntityAccess())
        {
            bool isHit = false;
            float3 projPos = projectileTransform.ValueRO.Position;

            foreach (var (enemyTransform, enemyHP) in SystemAPI.Query<RefRO<LocalTransform>, RefRW<HealthData>>().WithAll<EnemyData>())
            {
                float distSq = math.distancesq(projPos, enemyTransform.ValueRO.Position);

                if (distSq <= collisionRadius) 
                {
                    enemyHP.ValueRW.CurrentHP -= projectile.ValueRO.Damage;

                    isHit = true;
                    break;
                }
            }

            if (isHit)
            {
                ecb.DestroyEntity(projectileEntity);
            }
        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
