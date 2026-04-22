using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct EnemyDeathStstem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        if(!SystemAPI.TryGetSingleton<GamePrefabsData>(out var prefabsData))
        {
            return;
        }

        foreach( var (hp, transform, entity) in SystemAPI.Query<RefRO<HealthData>, RefRO<LocalTransform>>().WithAll<EnemyData>().WithEntityAccess())
        {
            if(hp.ValueRO.CurrentHP <= 0)
            {
                Entity ExpGemEntity = ecb.Instantiate(prefabsData.ExpGem);

                LocalTransform gemTransform = SystemAPI.GetComponent<LocalTransform>(prefabsData.ExpGem);
                gemTransform.Position = transform.ValueRO.Position;

                ecb.SetComponent(ExpGemEntity, gemTransform);

                ecb.DestroyEntity(entity);
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}