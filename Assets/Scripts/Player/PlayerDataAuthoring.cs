using System.ComponentModel;
using Unity.Entities;
using UnityEngine;

public class PlayerDataAuthoring : MonoBehaviour
{
    class Baker : Baker<PlayerDataAuthoring> 
    {
        public override void Bake(PlayerDataAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent<PlayerTag>(entity);

            AddComponent(entity, new HealthData
            {
                MaxHP = 100,
                CurrentHP = 100,
            });

            AddComponent(entity, new PlayerCollisionData
            {
                HitRadius = 1.5f,
                InvincibiliyDuration = 0.5f,
            });

            #region Weapons
            AddComponent(entity, new PlayerExpData
            {
                CurrenctLevel = 1,
                CurrenctExp = 0,
                RequiredExp = 10,
            });

            AddComponent(entity, new AuraWeaponData
            {
                AuraRadius = 5,
                DPS = 5,
            });

            AddComponent(entity, new MagicWandData
            {
                BulletSpeed = 10,
                FireRate = 1,
                NextFireTime = 0,
                bulletCount = 1,
            });
            #endregion Weapons
        }
    }
}
