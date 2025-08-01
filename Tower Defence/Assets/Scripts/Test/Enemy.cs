using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    private List<Transform> checkpointList;

    private Transform target;

    private bool destinatioReached = false;

    private void Start()
    {
        SetCheckpoints();
        AssignTarget();
    }

    private void SetCheckpoints()
    {
        checkpointList = new List<Transform>();
        List<Transform> waypointList = GameService.instance.GetWaypoints();

        foreach (Transform point in waypointList)
        {
            checkpointList.Add(point);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log(Vector3.Distance(transform.position, target.position));
        }

        if (!destinatioReached && Vector3.Distance(transform.position, target.position) <= .7f)
        {
            AssignTarget();
        }

        if (!destinatioReached)
        {
            Move();
        }
    }

    private void Move()
    {
        Vector3 direction = (target.position - transform.position).normalized;

        Vector3 velocity = direction * moveSpeed;
        velocity.y = 0f;

        rb.MovePosition(transform.position + velocity * Time.deltaTime);

        Rotate();
    }

    private void Rotate()
    {
        Vector3 direction = (target.position - transform.position).normalized;


        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Quaternion flatRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, flatRotation, rotationSpeed * Time.deltaTime);
            rb.MoveRotation(rotation);
        }
    }

    private void AssignTarget()
    {
        if (checkpointList.Count == 0)
        {
            Debug.Log("Enemy reached castle");
            destinatioReached = true;
            return;
        }
        target = checkpointList[0];
        checkpointList.RemoveAt(0);
    }
}
