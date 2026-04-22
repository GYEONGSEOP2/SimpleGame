using Unity.Entities;
using UnityEngine;

public struct MagicWandData : IComponentData
{
    public float FireRate;
    public float NextFireTime;
    public float BulletSpeed;

    public int bulletCount;
}
