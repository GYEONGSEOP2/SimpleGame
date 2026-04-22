using Unity.Entities;
using UnityEngine;


[UpdateAfter(typeof(PlayerCollisionData))]
public partial class GameOverSystem : SystemBase
{
    protected override void OnUpdate()
    {
        if(SystemAPI.HasSingleton<PlayerDeathTag>())
        {
            Debug.Log("Game Over");

            UnityEngine.Time.timeScale = 0;

            this.Enabled = false;
        }
    }
}
