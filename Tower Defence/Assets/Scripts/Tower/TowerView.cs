using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerView : MonoBehaviour
{
    [SerializeField] private Transform towerBase;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float fireRate;

    [SerializeField] private List<Transform> enemiesInRangeList;

    private Transform target;

    private void Start()
    {
        enemiesInRangeList = new List<Transform>();
    }

    private void Update()
    {
        if (enemiesInRangeList.Count > 0)
        {
            SetTarget();
            LookAtTarget();
        }
    }

    private void LookAtTarget()
    {
        Vector3 direction = (target.position - towerBase.position).normalized;
        direction.y = 0f;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            towerBase.rotation = Quaternion.Slerp(towerBase.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void SetTarget()
    {
        target = enemiesInRangeList[0];
    }

    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent<Enemy>(out Enemy enemy);
        if (enemy != null)
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
