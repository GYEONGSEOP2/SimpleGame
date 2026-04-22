using Unity.Entities;
using UnityEngine;

public struct PlayerCollisionData : IComponentData
{
    public float HitRadius;
    public float InvincibiliyDuration;
}
