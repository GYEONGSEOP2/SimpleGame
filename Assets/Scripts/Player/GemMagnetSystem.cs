using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct GemMagnetSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state )
    {
        if(!SystemAPI.TryGetSingletonEntity<PlayerTag>(out Entity playerEntity))
        {
            return;
        }
        if(!SystemAPI.TryGetSingletonRW<PlayerExpData>(out var playerExpData))
        {
            return;
        }

        float3 playerPos = SystemAPI.GetComponent<LocalTransform>(playerEntity).Position;
        float deltaTime = SystemAPI.Time.DeltaTime;
        float pickupDistance = 10f;
        float collectDistance = 0.2f;


        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        foreach( var (gem, transform, entity) in SystemAPI.Query<RefRO<ExpGemData>, RefRW<LocalTransform>>().WithEntityAccess())
        {
            float dist = math.distance(playerPos, transform.ValueRO.Position);
               
            if( dist < pickupDistance)
            {
                float3 direction = math.normalize(playerPos - transform.ValueRO.Position);
                transform.ValueRW.Position += direction * 10f * deltaTime;
            }

            if (dist < collectDistance)
            {
                playerExpData.ValueRW.CurrenctExp += gem.ValueRO.ExpValue;

                if(playerExpData.ValueRW.CurrenctExp >= playerExpData.ValueRW.RequiredExp)
                {
                    playerExpData.ValueRW.CurrenctLevel++;
                    playerExpData.ValueRW.CurrenctExp = playerExpData.ValueRW.CurrenctExp - playerExpData.ValueRW.RequiredExp;
                    playerExpData.ValueRW.RequiredExp += 5;
                    
                    ecb.AddComponent<PlayerLevelUpEventTag>(playerEntity);
                }

                ecb.DestroyEntity(entity);
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
