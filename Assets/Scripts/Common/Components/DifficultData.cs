using Unity.Entities;
using UnityEngine;

public struct DifficultData : IComponentData
{
    public double StartTime;
    public float DifficultyFactor;
}
