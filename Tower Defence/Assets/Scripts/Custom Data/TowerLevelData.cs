using System;
using UnityEngine;

[Serializable]
public class TowerLevelData
{
    [Header("Tower-Data")]
    public LevelType level;

    public float range;
    public float fireRate;

    [Header("Projectile-Data")]
    public int damage;
    public float damageAOE;
    public float projectileSpeed;

    [Header("Marketing-Data")]
    public int sellValue;
    public int updateCost;
    public int repairCost;
}
