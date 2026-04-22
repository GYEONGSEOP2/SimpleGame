using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;

[BurstCompile]
public partial struct OrbitalWeaponCollisionSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.TryGetSingletonEntity<PlayerTag>(out Entity player))
        {
            return;
        }
        float3 playerPos = SystemAPI.GetComponent<LocalTransform>(player).Position;

        float deltaTime = SystemAPI.Time.DeltaTime;

        foreach(var (weaponTransform, weaponData) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<OrbitalWeaponData>>())
        {
            double currentTime = SystemAPI.Time.ElapsedTime;

            float3 weaponPos = weaponTransform.ValueRO.Position;

            float hitRadiusSq = weaponData.ValueRO.HitRadius * weaponData.ValueRO.HitRadius;

            foreach (var (enemyTransform, enemyVelocity, enemyHp, enemyEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<PhysicsVelocity>, RefRW<HealthData>>().WithAll<EnemyData>().WithEntityAccess())
            {
                float distSq = math.distancesq(weaponPos, enemyTransform.ValueRO.Position);

                if(distSq < hitRadiusSq &&
                   currentTime >= enemyHp.ValueRO.LastHitTime + 0.5f)
                {
                    enemyHp.ValueRW.CurrentHP -= weaponData.ValueRO.Damage;
                    enemyHp.ValueRW.LastHitTime = currentTime;

                    float3 knockbackDir = math.normalize(enemyTransform.ValueRO.Position - playerPos);

                    enemyVelocity.ValueRW.Linear += knockbackDir * 50;
                }
            }
        }
    }
}
