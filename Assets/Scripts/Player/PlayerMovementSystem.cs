using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[BurstCompile]
public partial struct PlayerMovementSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        foreach (var (transform, input, speed) in SystemAPI.Query<
                     RefRW<LocalTransform>,
                     RefRO<PlayerInputData>,
                     RefRO<PlayerMoveSpeed>>().WithAll<PlayerTag>())
        {
            if (input.ValueRO.Direction.Equals(float2.zero)) continue;

            float3 moveVector = new float3(input.ValueRO.Direction.x, input.ValueRO.Direction.y, 0f);

            transform.ValueRW.Position += moveVector * speed.ValueRO.Speed * deltaTime;
        }
    }
}
