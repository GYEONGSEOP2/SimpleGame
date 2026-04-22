using Unity.Entities;
using UnityEngine;

public struct PlayerExpData : IComponentData
{
    public int CurrenctLevel;
    public float CurrenctExp;
    public float RequiredExp;
}
