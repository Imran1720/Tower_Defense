using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TowerView : MonoBehaviour
{
    [SerializeField] private Transform towerBase;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float fireRate;

    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float predictionTime;

    [SerializeField] private List<Transform> enemiesInRangeList;

    private Transform target;
    private bool isEnemyInRange;

    private float timer;
    private Vector3 previousTragetPosition;


    private void Start()
    {
        enemiesInRangeList = new List<Transform>();
    }

    private void Update()
    {

        if (enemiesInRangeList.Count > 0)
        {
            timer -= Time.deltaTime;
            SetTarget();
            LookAtTarget();
            Shoot();
        }
        else
        {
            isEnemyInRange = false;
        }
    }

    private void Shoot()
    {
        Vector3 targetVelocity = (target.position - previousTragetPosition) / Time.deltaTime;
        previousTragetPosition = target.position;
        if (timer <= 0)
        {
            //shoot
            Vector3 predictedPosition = target.position + targetVelocity * predictionTime;

            Vector3 direction = (predictedPosition - shootPoint.position).normalized;
            Vector3 velocity = direction * bulletSpeed;
            Transform bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.AddForce(velocity, ForceMode.Impulse);

            ResetTimer();
        }
    }

    private void ResetTimer()
    {
        timer = fireRate;
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
            isEnemyInRange = true;
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
