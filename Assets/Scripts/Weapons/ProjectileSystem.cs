using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;

[BurstCompile]
public partial struct ProjectileSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        foreach(var (transform, projectile) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<ProjectileData>>())
        {
            transform.ValueRW.Position += new float3(projectile.ValueRO.Direction * projectile.ValueRO.Speed * deltaTime, 0);
        }
    }
}
