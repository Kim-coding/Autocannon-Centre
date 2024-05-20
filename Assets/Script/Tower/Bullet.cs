using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;
    private float speed;
    private int damage;
    private float range;
    private Vector3 startPosition;
    public void Set(Transform target, float bulletSpeed, int bulletDamage, float range)
    {
        this.target = target;
        speed = bulletSpeed;
        damage = bulletDamage;
        this.range = range;
        startPosition = transform.position;
    }

    private void Update()
    {
        if(target == null || !target.gameObject.activeInHierarchy)
        {
            PoolManager.instance.ReturnObjectToPool(gameObject);
            return;
        }

        Move();

        if (Vector3.Distance(startPosition, transform.position) > range)
        {
            PoolManager.instance.ReturnObjectToPool(gameObject);
        }
    }

    private void Move()
    {
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            PoolManager.instance.ReturnObjectToPool(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        dir = dir.normalized;

        transform.position += dir * speed * Time.deltaTime;
        transform.LookAt(target);
        transform.Rotate(-90, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("monster"))
        {
            MonsterHealth monster = other.GetComponent<MonsterHealth>();
            if(monster != null) 
            {
                monster.OnDamage(damage);
                PoolManager.instance.ReturnObjectToPool(gameObject);
            }
        }
        else if (other.CompareTag("ground"))
        {
            PoolManager.instance.ReturnObjectToPool(gameObject);
        }
    }
}
