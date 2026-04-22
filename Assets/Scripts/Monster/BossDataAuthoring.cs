using Unity.Entities;
using UnityEngine;

public class BossDataAuthoring : MonoBehaviour
{
    public GameObject bossPrefab;
    public float spawnTime = 30;

    class Baker : Baker<BossDataAuthoring>
    {
        public override void Bake(BossDataAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new BossSpawnData
            {
                BossPrefab = GetEntity(authoring.bossPrefab),
                SpawnTime = authoring.spawnTime,
                IsSpawned = false,
            });

        }
    }
}
