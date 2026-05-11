using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial class PlayerInputSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float x = 0;
        float y = 0;
        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) y += 1f;
            if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) y -= 1f;
            if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) x += 1f;
            if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) x -= 1f;
        }

        float2 inputDir = new float2(x, y);
        if (math.lengthsq(inputDir) > 0)
        {
            inputDir = math.normalize(inputDir);
        }

        foreach (var input in SystemAPI.Query<RefRW<PlayerInputData>>().WithAll<PlayerTag>())
        {
            input.ValueRW.Direction = inputDir;
        }
    }
}
