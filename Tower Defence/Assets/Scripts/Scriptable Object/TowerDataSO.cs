using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/TowerDataSO", fileName = "TowerDataSO")]
public class TowerDataSO : ScriptableObject
{
    [Header("Prefab-Data")]
    public Bullet bulletPrefab;
    public TowerView towerPrefab;

    [Header("Core-Data")]
    public string towerDescription;
    public float bulletSpeed;

    public TowerID towerID;
    public EnemyType immuneEnemyType;

    [Header("Level-Data")]
    public List<TowerLevelData> levelDataList;
}
