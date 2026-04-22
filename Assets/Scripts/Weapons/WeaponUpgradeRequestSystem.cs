using Unity.Entities;
using UnityEngine;

public enum WeaponType
{
    Aura = 0,
    MagicWand = 1,
    Orbital = 2,
}
public partial struct WeaponUpgradeRequestSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        foreach(var (request, entity) in SystemAPI.Query<WeaponUpgradeRequestData>().WithEntityAccess())
        {
            if (request.WeaponTypeValue == WeaponType.Aura)
            {
                if (SystemAPI.TryGetSingletonRW<AuraWeaponData>(out var auraWeapon))
                {
                    auraWeapon.ValueRW.AuraRadius += 0.5f;
                    auraWeapon.ValueRW.DPS++;
                }
            }
            else if (request.WeaponTypeValue == WeaponType.MagicWand)
            {
                if (SystemAPI.TryGetSingletonRW<MagicWandData>(out var magicWand))
                {
                    if (magicWand.ValueRO.bulletCount < 15)
                    {
                        magicWand.ValueRW.bulletCount += 2;
                    }
                }
            }
            else if (request.WeaponTypeValue == WeaponType.Orbital)
            {
                if (SystemAPI.TryGetSingletonRW<OrbitalSpawnerData>(out var orbitalWeapon))
                {
                    if (orbitalWeapon.ValueRO.Count < 5)
                    {
                        orbitalWeapon.ValueRW.Count++;
                        orbitalWeapon.ValueRW.Damage += 5;
                        orbitalWeapon.ValueRW.IsSpawned = false;
                    }
                    else if(orbitalWeapon.ValueRO.Count >= 5)
                    {
                        orbitalWeapon.ValueRW.Damage += 5;
                    }
                }
            }
            ecb.RemoveComponent<WeaponUpgradeRequestData>(entity);
        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}

// HardCoding
//if (!SystemAPI.TryGetSingletonRW<AuraWeaponData>(out var auraWeapon))
//{
//    return;
//}
//if(!SystemAPI.TryGetSingletonRW<MagicWandData>(out var magicWand))
//{
//    return;
//}
//if(!SystemAPI.TryGetSingletonRW<OrbitalSpawnerData>(out var orbitalWeapon))
//{
//    return;
//}

//auraWeapon.ValueRW.AuraRadius += 0.5f;
//auraWeapon.ValueRW.DPS++;

//if(magicWand.ValueRO.bulletCount < 15)
//{
//    magicWand.ValueRW.bulletCount += 2;
//}
//if(orbitalWeapon.ValueRO.Count < 5)
//{
//    orbitalWeapon.ValueRW.Count++;
//    orbitalWeapon.ValueRW.Damage += 5;
//    orbitalWeapon.ValueRW.IsSpawned = false;
//}