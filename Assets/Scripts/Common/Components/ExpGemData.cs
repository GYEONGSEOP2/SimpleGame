using Unity.Entities;
using UnityEngine;

public struct ExpGemData : IComponentData
{
    public float ExpValue;
    public bool IsTargetingPlayer;
}
