using Unity.Entities;
using UnityEngine;

public class OrbitalWeaponAuthoring : MonoBehaviour
{
    public GameObject weaponPrefab;
    public int weaponCount = 3;
    public float radius = 11;
    public float rotationSpeed = 3;

    class Baker : Baker<OrbitalWeaponAuthoring>
    {
        public override void Bake(OrbitalWeaponAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new OrbitalSpawnerData
            {
                WeaponPrefab = GetEntity(authoring.weaponPrefab, TransformUsageFlags.Dynamic),
                Count = authoring.weaponCount,
                Radius = authoring.radius,
                RotationSpeed = authoring.rotationSpeed,
                Damage = 5,
                HitRadius = 1f,
                KnockbackForce = 1f,
                IsSpawned = false
            });
        }
    }
}
