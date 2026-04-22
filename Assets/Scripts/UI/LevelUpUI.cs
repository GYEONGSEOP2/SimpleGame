using Unity.Entities;
using UnityEngine;

public class LevelUpUI : MonoBehaviour
{
    public GameObject LevelUpPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerLevelUpBridgeSystem.OnLevelUpTriggered += ShowLevelUpUI;
    }
    private void OnDestroy()
    {
        PlayerLevelUpBridgeSystem.OnLevelUpTriggered -= ShowLevelUpUI;
    }
    
    private void ShowLevelUpUI()
    {
        LevelUpPanel.SetActive(true);
    }

    public void OnClickBtn(int type)
    {
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        var playerQuery = entityManager.CreateEntityQuery((typeof(PlayerTag)));
        if (playerQuery.TryGetSingletonEntity<PlayerTag>(out Entity playerEntity))
        {
            entityManager.AddComponentData(playerEntity, new WeaponUpgradeRequestData
            {
                WeaponTypeValue = (WeaponType)type,
            });
        }

        LevelUpPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }
}
