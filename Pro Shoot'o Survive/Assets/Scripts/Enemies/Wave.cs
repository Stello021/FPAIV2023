using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Wave : ScriptableObject
{
    public int StandardEnemiesToSpawn;
    public int RangedEnemiesToSpawn;
    public float EnemiesSpeedMultiplier;
    public float EnemiesHpMultiplier;
}
