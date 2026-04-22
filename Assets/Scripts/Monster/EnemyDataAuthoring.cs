using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class EnemyDataAuthoring : MonoBehaviour
{
    public float speed;
    public float HP;

    class Baker : Baker<EnemyDataAuthoring> 
    {
        public override void Bake(EnemyDataAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EnemyData 
            { 
                Speed = authoring .speed ,
                Direction = new float2(1,0),
                PushForce = new float2(0,0),
            });

            AddComponent(entity, new HealthData
            {
                MaxHP = authoring.HP,
                CurrentHP = authoring.HP,
            });
        }
    }
}
