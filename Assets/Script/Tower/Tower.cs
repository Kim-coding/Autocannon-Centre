using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject bulletPrefab;
    private GameObject currentTarget;

    private Collider[] hitCollider;
    private int maxCollider = 10;

    public int damage;
    public float range;
    public float speed;
    public float fireRate = 0.5f;
    public float fireTime;

    private int id;
    private void Awake()
    {
        hitCollider = new Collider[maxCollider];

    }

    private void Start()
    {
        id = int.Parse(gameObject.name);

        var towerTable = DataTableMgr.Get<TowerTable>(DataTableIds.tower);
        if (towerTable != null)
        {
            var data = towerTable.GetID(id);
            speed = data.atkSpeed;
            range = data.atkRange;
            damage = data.damage;       
        }
    }

    private void Update()
    {
        if(currentTarget == null || !currentTarget.activeInHierarchy)
        {
            //Å¸°Ù ¼³Á¤
            currentTarget = FindTarget();
        }
        
        fireTime += Time.deltaTime;
        if (currentTarget != null && fireTime > 0.25f)
        {
            Shoot(currentTarget);
            fireTime = 0f;
        }
    }

    private GameObject FindTarget()
    {
        GameObject nearestMonster = null;
        float shortDistance = 10f;
        int colliders = Physics.OverlapSphereNonAlloc(transform.position, range, hitCollider);
        for (int i = 0; i < colliders; i++)
        {
            Collider collider = hitCollider[i];
            if(collider != null && collider.gameObject.CompareTag("monster"))
            {
                float distance = Vector3.Distance(transform.position, collider.gameObject.transform.position);
                if(distance < shortDistance)
                {
                    shortDistance = distance;
                    nearestMonster = collider.gameObject;
                }
            }
        }

        return nearestMonster;
    }

    private void Shoot(GameObject target)
    {
        GameObject bulletGO = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        if(bullet != null ) 
        {
            bullet.Set(currentTarget.transform, speed, damage);
        }
    }
}
