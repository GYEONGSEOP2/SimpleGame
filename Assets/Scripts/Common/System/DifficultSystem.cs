using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[BurstCompile]
public partial struct DifficultSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if(!SystemAPI.TryGetSingletonRW<DifficultData>(out var difficult))
        {
            return;
        }

        double elapsedTime = SystemAPI.Time.ElapsedTime - difficult.ValueRO.StartTime;
        difficult.ValueRW.DifficultyFactor = 1.0f + (float)(elapsedTime / 60f);
    }
}
