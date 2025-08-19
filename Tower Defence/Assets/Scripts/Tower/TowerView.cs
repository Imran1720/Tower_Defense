using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TowerView : MonoBehaviour
{
    [Header("Tower-Data")]
    [SerializeField] private TowerDataSO towerDataSO;
    [SerializeField] private Transform towerBase;
    [SerializeField] private float rotationSpeed;
    //[SerializeField] private int attackPower;
    //[SerializeField] private float range;
    //[SerializeField] private float fireRate;

    [Header("Tower-Level-Data")]
    [SerializeField] private Transform level2Body;
    [SerializeField] private Transform level3Body;
    [SerializeField] private Transform level4Body;
    [SerializeField] private Transform level5Body;
    [SerializeField] private Transform rangeIndicator;

    [Header("Bullet-Data")]
    [SerializeField] private Transform shootPoint;
    //[SerializeField] private Bullet bulletPrefab;
    //[SerializeField] private float bulletSpeed;
    //[SerializeField] private float predictionTime;


    private List<Transform> enemiesInRangeList;
    [SerializeField] private CapsuleCollider capsuleCollider;

    private Transform target;

    private float timer;
    private Vector3 previousTragetPosition;
    private LevelType currentLevel;
    private TowerLevelData towerCurrentData;
    private bool canShoot = false;

    private void Start()
    {
        UpdateLevel(currentLevel);
        enemiesInRangeList = new List<Transform>();
        ToggleBodyParts();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.T))
        {
            currentLevel++;
            if ((int)currentLevel >= 5)
            {
                currentLevel = LevelType.LEVEL_5;
            }
            UpdateLevel(currentLevel);
        }
        if (enemiesInRangeList.Count > 0)
        {
            timer -= Time.deltaTime;
            SetTarget();
            LookAtTarget();
            Shoot();
        }
    }

    public void UpdateLevel(LevelType levelType)
    {
        currentLevel = levelType;
        towerCurrentData = GetTowerData(levelType);

        SetRange(towerCurrentData.range);

        ToggleBodyParts();
    }

    private void SetRange(float rangeValue)
    {
        capsuleCollider.radius = rangeValue;
        float scale = capsuleCollider.radius / 5;
        rangeIndicator.transform.localScale = new Vector3(scale, 1, scale);
    }

    public TowerLevelData GetTowerData(LevelType level)
    {
        TowerLevelData data = towerDataSO.levelDataList.Find(tower => tower.level == level);
        if (data == null)
            return null;

        return data;
    }

    private void ToggleBodyParts()
    {
        switch (currentLevel)
        {
            case LevelType.LEVEL_1:
            default:
                HideAllParts();
                break;
            case LevelType.LEVEL_2:
                ShowBodyPart(level2Body);
                break;
            case LevelType.LEVEL_3:
                ShowBodyPart(level3Body);
                break;
            case LevelType.LEVEL_4:
                ShowBodyPart(level4Body);
                break;
            case LevelType.LEVEL_5:
                ShowBodyPart(level5Body);
                break;
        }
    }

    private void ShowBodyPart(Transform part)
    {
        part.gameObject.SetActive(true);
    }
    private void HideAllParts()
    {
        level2Body.gameObject.SetActive(false);
        level3Body.gameObject.SetActive(false);
        level4Body.gameObject.SetActive(false);
        level5Body.gameObject.SetActive(false);
    }

    private void Shoot()
    {
        Vector3 targetVelocity = (target.position - previousTragetPosition) / Time.deltaTime;
        previousTragetPosition = target.position;
        if (timer <= 0 && canShoot)
        {
            Bullet bullet = Instantiate(towerDataSO.bulletPrefab, shootPoint.position, Quaternion.identity);
            bullet.InitializeData(towerCurrentData.damage, target, towerDataSO.bulletSpeed);

            canShoot = false;
            ResetTimer();
        }
    }

    private void ResetTimer()
    {
        timer = towerCurrentData.fireRate;
    }

    private void LookAtTarget()
    {
        Vector3 direction = (target.position - towerBase.position).normalized;
        direction.y = 0f;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            towerBase.rotation = Quaternion.Slerp(towerBase.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            if (IsFacingTarget(targetRotation))
            {
                canShoot = true;
            }
        }
    }

    private bool IsFacingTarget(Quaternion targetRotation)
    {
        float angle = Quaternion.Angle(towerBase.rotation, targetRotation);
        return angle <= 15;
    }

    private void SetTarget()
    {
        target = enemiesInRangeList[0];
    }

    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent<Enemy>(out Enemy enemy);
        if (enemy != null && !enemy.IsEnemy(towerDataSO.immuneEnemyType))
        {
            enemiesInRangeList.Add(enemy.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        other.TryGetComponent<Enemy>(out Enemy enemy);
        if (enemy != null)
        {
            enemiesInRangeList.Remove(enemy.transform);
        }
    }
}
