using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;

[BurstCompile]
public partial struct OrbitalWeaponSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if(!SystemAPI.TryGetSingletonEntity<PlayerTag>(out Entity playerEntity))
        {
            return;
        }

        float3 playerPos = SystemAPI.GetComponent<LocalTransform>(playerEntity).Position;
        float deltaTime = SystemAPI.Time.DeltaTime;

        using var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        foreach (var (spawner, entity) in SystemAPI.Query<RefRW<OrbitalSpawnerData>>().WithEntityAccess())
        {
            if (!spawner.ValueRO.IsSpawned)
            {
                foreach(var (weaponData, weaponEntity) in SystemAPI.Query<RefRO<OrbitalWeaponData>>().WithEntityAccess())
                {
                    ecb.DestroyEntity(weaponEntity);
                }

                int count = spawner.ValueRO.Count;

                float angleStep = math.radians(360 / count);
                for (int i = 0; i < count; i++)
                {
                    Entity weapon = ecb.Instantiate(spawner.ValueRO.WeaponPrefab);
                    ecb.AddComponent(weapon, new OrbitalWeaponData
                    {
                        CurrenctAngle = angleStep * i,
                        Radius = spawner.ValueRO.Radius,
                        RoatationSpeed = spawner.ValueRO.RotationSpeed,
                        HitRadius = spawner.ValueRO.HitRadius,
                        KnockbackForce = spawner.ValueRO.KnockbackForce,
                        Damage = spawner.ValueRO.Damage,
                    });
                }
                spawner.ValueRW.IsSpawned = true;
            }
        }

        foreach (var (transform, weapon) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<OrbitalWeaponData>>())
        {
            weapon.ValueRW.CurrenctAngle += weapon.ValueRO.RoatationSpeed * deltaTime;

            float x = playerPos.x + math.cos(weapon.ValueRO.CurrenctAngle) * weapon.ValueRO.Radius;
            float y = playerPos.y + math.sin(weapon.ValueRO.CurrenctAngle) * weapon.ValueRO.Radius;

            transform.ValueRW.Position = new float3(x, y, playerPos.z);
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
