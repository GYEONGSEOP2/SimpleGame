using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

public struct ProjectileData : IComponentData
{
    public float2 Direction;
    public float Speed;
    public float Damage;
}
