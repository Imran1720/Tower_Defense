using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    private int attackPower;
    private Transform target;
    private float bulletSpeed;
    private Vector3 velocity = Vector3.zero;

    private void Update()
    {
        if (target == null)
        {
            velocity = Vector3.zero;
            return;
        }

        CalaculateVelocity();
    }

    private void FixedUpdate()
    {
        MoveBullet();
    }

    private void MoveBullet() => rb.linearVelocity = velocity;

    private void CalaculateVelocity()
    {
        Vector3 distance = (target.position - transform.position).normalized;
        velocity = distance * bulletSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent(out Enemy enemy);
        if (enemy != null)
        {
            target = null;
            enemy.TakeDamage(attackPower);
            Destroy(gameObject);
        }
    }

    public void InitializeData(int amount, Transform target, float speed)
    {
        bulletSpeed = speed;
        attackPower = amount;
        this.target = target;
    }
}
