using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct PlayerCollisionSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if(!SystemAPI.TryGetSingletonEntity<PlayerTag>(out Entity playerEntity))
        {
            return;
        }
        if(SystemAPI.HasSingleton<PlayerDeathTag>())
        {
            return;
        }
        using var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        var playerTransform = SystemAPI.GetComponent<LocalTransform>(playerEntity);
        var playerHealth = SystemAPI.GetComponentRW<HealthData>(playerEntity);
        var collisionData = SystemAPI.GetComponent<PlayerCollisionData>(playerEntity);

        double currentTime = SystemAPI.Time.ElapsedTime;

        if(currentTime < playerHealth.ValueRO.LastHitTime + collisionData.InvincibiliyDuration)
        {
            return;
        }

        float radiusSq = collisionData.HitRadius * collisionData.HitRadius;
        bool isHit = false;

        foreach(var enemyTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<EnemyData>())
        {
            if (math.distancesq(playerTransform.Position, enemyTransform.ValueRO.Position) < radiusSq)
            {
                playerHealth.ValueRW.CurrentHP -= 0;
                isHit = true;
                break;
            }
        }

        if(isHit)
        {
            playerHealth.ValueRW.LastHitTime = currentTime;
            if(playerHealth.ValueRW.CurrentHP <= 0)
            {
                ecb.AddComponent<PlayerDeathTag>(playerEntity);
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
