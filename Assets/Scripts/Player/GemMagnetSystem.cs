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

        #region HardCoding
        // HardCoding
        //if (!SystemAPI.TryGetSingletonRW<AuraWeaponData>(out var auraWeapon))
        //{
        //    return;
        //}
        //if(!SystemAPI.TryGetSingletonRW<MagicWandData>(out var magicWand))
        //{
        //    return;
        //}
        //if(!SystemAPI.TryGetSingletonRW<OrbitalSpawnerData>(out var orbitalWeapon))
        //{
        //    return;
        //}
        #endregion

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
                    

                    //auraWeapon.ValueRW.AuraRadius += 0.5f;
                    //auraWeapon.ValueRW.DPS++;

                    //if(magicWand.ValueRO.bulletCount < 15)
                    //{
                    //    magicWand.ValueRW.bulletCount += 2;
                    //}
                    //if(orbitalWeapon.ValueRO.Count < 5)
                    //{
                    //    orbitalWeapon.ValueRW.Count++;
                    //    orbitalWeapon.ValueRW.Damage += 5;
                    //    orbitalWeapon.ValueRW.IsSpawned = false;
                    //}

                }

                ecb.DestroyEntity(entity);
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
