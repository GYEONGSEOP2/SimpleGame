using Unity.Entities;
using UnityEngine;

public struct OrbitalWeaponData : IComponentData
{
    public float CurrenctAngle;
    public float RoatationSpeed;
    public float Radius;

    public float Damage;
    public float HitRadius;

    public float KnockbackForce;
}

public struct OrbitalSpawnerData : IComponentData
{
    public Entity WeaponPrefab;
    public int Count;
    public float Radius;
    public float RotationSpeed;
    public bool IsSpawned;

    public float Damage;
    public float HitRadius;

    public float KnockbackForce;
}
