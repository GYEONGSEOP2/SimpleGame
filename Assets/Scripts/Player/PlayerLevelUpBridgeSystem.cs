using System;
using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(PresentationSystemGroup))]
public partial class PlayerLevelUpBridgeSystem : SystemBase
{
    public static Action OnLevelUpTriggered;
    protected override void OnUpdate()
    {
        bool isLevelUp = false;

        using var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        foreach(var (tag,entity) in SystemAPI.Query<RefRO<PlayerLevelUpEventTag>>().WithEntityAccess())
        {
            isLevelUp = true;

            ecb.RemoveComponent<PlayerLevelUpEventTag>(entity);
        }
        

        if (isLevelUp) {
            UnityEngine.Time.timeScale = 0;
            OnLevelUpTriggered?.Invoke();
        }

        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}
