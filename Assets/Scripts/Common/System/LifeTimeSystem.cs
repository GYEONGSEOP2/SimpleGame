using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[BurstCompile]
public partial struct LifeTimeSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        using var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
        float deltaTime = SystemAPI.Time.DeltaTime;

        foreach(var (lifeTime, entity) in SystemAPI.Query<RefRW<LifeTimeData>>().WithEntityAccess())
        {
            lifeTime.ValueRW.Value -= deltaTime;

            if(lifeTime.ValueRW.Value <= 0)
            {
                ecb.DestroyEntity(entity);
            }
        }
        ecb.Playback(state.EntityManager);
    }
}
