using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct AuraDamageSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if(!SystemAPI.TryGetSingletonEntity<PlayerTag>(out Entity playerEntity))
        {
            return;
        }
        if(!SystemAPI.TryGetSingleton<AuraWeaponData>(out var auraWeaponData))
        {
            return;
        }

        float3 playerPos = SystemAPI.GetComponent<LocalTransform>(playerEntity).Position;

        float auraRadius = auraWeaponData.AuraRadius;
        float dps = auraWeaponData.DPS;
        float deltatTime = SystemAPI.Time.DeltaTime;

        new AuraDamageJob
        {
            PlayerPos = playerPos.xy,
            AuraRadius = auraRadius,
            DamageThisFrame = deltatTime * dps,
            DeltaTime = deltatTime,
        }.ScheduleParallel();
    }
}

[BurstCompile]
public partial struct AuraDamageJob : IJobEntity
{
    public float2 PlayerPos;
    public float AuraRadius;
    public float DamageThisFrame;
    public float DeltaTime;

    public void Execute(in LocalTransform transform, ref HealthData health, ref EnemyData enemy)
    {
        float2 currentPos = transform.Position.xy;
        float dist = math.distance(PlayerPos, transform.Position.xy);

        if (dist <= AuraRadius)
        {
            health.CurrentHP -= DamageThisFrame;

            float2 pushDir = math.normalize(currentPos - PlayerPos);

            enemy.PushForce += pushDir * 10f * DeltaTime;
        }
    }
}