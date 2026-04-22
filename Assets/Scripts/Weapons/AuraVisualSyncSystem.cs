using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct AuraVisualSyncSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if(!SystemAPI.TryGetSingleton<AuraWeaponData>(out var auraData))
        {
            return;
        }

        foreach(var (transform, visualTag) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<AuraVisaulTag>>())
        {
            transform.ValueRW.Scale = auraData.AuraRadius * 2.0f;
        }

    }
}
