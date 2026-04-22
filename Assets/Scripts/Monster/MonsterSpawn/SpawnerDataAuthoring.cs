using Unity.Entities;
using UnityEngine;

public class SpawnerDataAuthoring : MonoBehaviour
{
    public GameObject enemy;
    public GameObject expGem;
    public int count;

    public float spawnRate = 1.5f;
    public float spawnRadius = 15.0f;

    class Baker : Baker<SpawnerDataAuthoring>
    {
        public override void Bake(SpawnerDataAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new SpawnerData
            {
                Enemy = GetEntity(authoring.enemy, TransformUsageFlags.Dynamic),
                ExpGem = GetEntity(authoring.expGem, TransformUsageFlags.Dynamic),

                NextSpawnTime = 0,
                SpawnRadius = authoring.spawnRadius,
                SpawnRate = authoring.spawnRate,
            });
        }
    }
}
