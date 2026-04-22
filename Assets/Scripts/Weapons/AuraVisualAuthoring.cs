using Unity.Entities;
using UnityEngine;

public class AuraVisualAuthoring : MonoBehaviour
{
    class Baker : Baker<AuraVisualAuthoring>
    {
        public override void Bake(AuraVisualAuthoring authoring)
        {
            var Entity = GetEntity(TransformUsageFlags.Dynamic);
            
            AddComponent<AuraVisaulTag>(Entity);
        }
    }
}
