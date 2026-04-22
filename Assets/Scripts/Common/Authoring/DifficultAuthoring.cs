using Unity.Entities;
using UnityEngine;

public class DifficultAuthoring : MonoBehaviour
{
    public class Baker : Baker<DifficultAuthoring>
    {
        public override void Bake(DifficultAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new DifficultData
            {
                StartTime = 0,
                DifficultyFactor = 1.0f
            });
        }
    }
}
