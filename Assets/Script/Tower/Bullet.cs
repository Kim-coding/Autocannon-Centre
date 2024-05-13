using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;
    private float speed;
    private int damage;

    public void Set(Transform target, float bulletSpeed, int bulletDamage)
    {
        this.target = target;
        speed = bulletSpeed;
        damage = bulletDamage;
    }

    private void Awake()
    {
        
    }

    private void Update()
    {
        if(target == null)
        {
            Destroy(gameObject);
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
                Destroy(gameObject);
            }
        }
        else if (other.CompareTag("ground"))
        {
            Destroy(gameObject);
        }
    }
}
