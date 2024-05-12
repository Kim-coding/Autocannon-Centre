using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject bulletPrefab;
    private GameObject currentTarget;
    public GameObject soldier;

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
        id = int.Parse(name.Replace("(Clone)", ""));

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
        if(currentTarget == null || !currentTarget.activeInHierarchy || IsTargetOutOfRange(currentTarget))
        {
            //새로운 타겟 설정
            currentTarget = FindTarget();
        }

        if(soldier != null && currentTarget != null)
        {
            soldier.transform.LookAt(currentTarget.transform.position);
            soldier.transform.Rotate(0, 180, 0);
        }
        
        fireTime += Time.deltaTime;
        if (currentTarget != null && fireTime > 0.25f)
        {
            Shoot(currentTarget);
            fireTime = 0f;
        }
        

    }

    private bool IsTargetOutOfRange(GameObject target)  // 현재 타겟과의 거리 검사
    {
        return Vector3.Distance(transform.position, target.transform.position) > range;
    }

    private GameObject FindTarget()
    {
        GameObject nearestMonster = null;
        float shortDistance = 15f;
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
        if (bulletPrefab == null)
        {
            return;   
        }
        GameObject bulletGO = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        if(bullet != null ) 
        {
            bullet.Set(currentTarget.transform, speed, damage);
        }
    }
}
