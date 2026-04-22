using Unity.Entities;
using UnityEngine;

public class PrefabsAuthoring : MonoBehaviour
{
    public GameObject missilePrefab;
    public GameObject explosionPrefab;
    public GameObject expGem;

    public class Baker : Baker<PrefabsAuthoring>
    {
        public override void Bake(PrefabsAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new GamePrefabsData
            {
                MissilePrefab = GetEntity(authoring.missilePrefab, TransformUsageFlags.Dynamic),
                ExpGem = GetEntity(authoring.expGem, TransformUsageFlags.Dynamic),
            });
        }
    }
}
