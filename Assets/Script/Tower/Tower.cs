using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject bulletPrefab;
    private GameObject currentTarget;
    public GameObject soldier;

    public int damage;
    public float range;
    public float speed;
    public int percent;
    public TowerTable towerTable;
    public float fireRate = 0.05f;
    public float fireTime;

    private int id;

    private TowerSpawner towerSpawner;


    private void Start()
    {
        towerSpawner = GetComponentInParent<TowerSpawner>();
        id = int.Parse(name.Replace("(Clone)", ""));

        towerTable = DataTableMgr.Get<TowerTable>(DataTableIds.tower);
        if (towerTable != null)
        {
            var data = towerTable.GetID(id);
            speed = data.atkSpeed;
            range = data.atkRange;
            damage = data.damage;
            percent = data.percent;
        }

        PoolManager.instance.CreatePool(bulletPrefab, 1);
    }

    private void Update()
    {
        UpdateCurrentTarget();

        if (soldier != null && currentTarget != null)
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

    private void UpdateCurrentTarget()
    {
        if (currentTarget == null || IsTargetOutOfRange(currentTarget))
        {
            //새로운 타겟 설정
            currentTarget = FindTarget();
        }
    }

    private bool IsTargetOutOfRange(GameObject target)  // 현재 타겟과의 거리 검사
    {
        return Vector3.Distance(transform.position, target.transform.position) > range;
    }

    private GameObject FindTarget()
    {
        GameObject nearestMonster = null;
        float shortDistance = float.MaxValue;

        Collider[] colliders = Physics.OverlapSphere(transform.position, range);
        
        foreach (Collider collider in colliders) 
        {
            if(collider != null)
            {
                if(collider.gameObject.CompareTag("monster"))
                {
                    float distance = Vector3.Distance(transform.position, collider.gameObject.transform.position);
                    if (distance < shortDistance)
                    {
                        shortDistance = distance;
                        nearestMonster = collider.gameObject;
                    }
                }
            }
        }

        return nearestMonster;
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    private void Shoot(GameObject target)
    {
        if (bulletPrefab == null)
        {
            return;   
        }

        var pos = transform.position;
        pos.y += 1.7f;

        GameObject bulletGO = PoolManager.instance.GetObjectPool(bulletPrefab.name);
        if(bulletGO != null) 
        {
            bulletGO.transform.position = pos;
            bulletGO.transform.rotation = Quaternion.identity;

            Bullet bullet = bulletGO.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.Set(currentTarget.transform, speed, damage, range);
            }
        }
    }

    public void UpgradeTower(TowerData data)
    {
        if (towerTable != null)
        {
            speed += data.atkspeedInc;
            damage += data.atkInc;
            percent += data.percentIncr;
        }
    }
}
