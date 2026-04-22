using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct EnemyMovementSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.TryGetSingletonEntity<PlayerTag>(out Entity playerEntity))
        {
            return;
        }

        float3 playerPos = SystemAPI.GetComponent<LocalTransform>(playerEntity).Position;
        float deltaTime = SystemAPI.Time.DeltaTime;


        new EnemyMoveJob
        {
            DeltaTime = deltaTime,
            TargetPos = playerPos.xy
        }.ScheduleParallel();
    }
}

[BurstCompile]
public partial struct EnemyMoveJob : IJobEntity
{
    public float DeltaTime;
    public float2 TargetPos;

    public void Execute(ref LocalTransform transform, ref PhysicsVelocity velocity, ref EnemyData enemy)
    {
        float2 currentPos = transform.Position.xy;
        float dist = math.distance(TargetPos, currentPos);

        float2 baseVelocity = float2.zero;

        if (dist > 2.0f)
        {
            float2 directionToTarget = math.normalize(TargetPos - currentPos);
            enemy.Direction = math.lerp(enemy.Direction, directionToTarget, DeltaTime * 5f);

            baseVelocity = enemy.Direction * enemy.Speed;
        }

        float2 finalVelocity = baseVelocity + enemy.PushForce;

        enemy.PushForce = math.lerp(enemy.PushForce, float2.zero, DeltaTime * 5f);

        

        //velocity.Linear = new float3(finalVelocity.x, finalVelocity.y, 0);

        float2 currentPhysicsVelocity = velocity.Linear.xy;

        float2 blendedVelocity = math.lerp(currentPhysicsVelocity, finalVelocity, DeltaTime * 10f);

        velocity.Linear = new float3(blendedVelocity.x, blendedVelocity.y, 0);
    }
}