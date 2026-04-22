using Unity.Entities;
using UnityEngine;

public struct HealthData : IComponentData
{
    public float CurrentHP;
    public float MaxHP;

    public double LastHitTime;
}