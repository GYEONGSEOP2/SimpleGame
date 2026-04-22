using Unity.Entities;
using UnityEngine;

public class ExpGemAuthoring : MonoBehaviour
{
    public float expValue = 1f;

    class Baker : Baker<ExpGemAuthoring>
    {
       public override void Bake(ExpGemAuthoring authoring)
       {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new ExpGemData
            {
                ExpValue = authoring.expValue,
                IsTargetingPlayer = false,
            });
       }
    }
}
