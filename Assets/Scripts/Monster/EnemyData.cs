using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct EnemyData : IComponentData
{
    public float2 Direction;
    public float Speed;
    public float2 PushForce;
}
