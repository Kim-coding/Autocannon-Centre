using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject bulletPrefab;
    private GameObject currentTarget;
    public GameObject soldier;

    private Collider[] hitCollider;
    private int maxCollider = 100;

    public int damage;
    public float range;
    public float speed;
    public int percent;
    public TowerTable towerTable;
    public float fireRate = 0.05f;
    public float fireTime;

    private int upgradeCount = 3;

    private int id;

    private TowerSpawner towerSpawner;
    private void Awake()
    {
        hitCollider = new Collider[maxCollider];

    }

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
    }

    private void Update()
    {
        if(currentTarget == null || IsTargetOutOfRange(currentTarget))
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
        float shortDistance = float.MaxValue;
        //int colliders = Physics.OverlapSphereNonAlloc(transform.position, range, hitCollider);
        //Debug.Log(colliders);
        //for (int i = 0; i < colliders; i++)
        //{
        //    Collider collider = hitCollider[i];
        //    Debug.Log(collider != null);
        //    Debug.Log(collider.gameObject.CompareTag("monster"));
        //    Debug.Log(collider.gameObject.tag) ;

        //    if (collider != null && collider.gameObject.CompareTag("monster"))
        //    {
        //        Debug.Log("2");
        //        float distance = Vector3.Distance(transform.position, collider.gameObject.transform.position);
        //        if(distance < shortDistance)
        //        {
        //            shortDistance = distance;
        //            nearestMonster = collider.gameObject;
        //        }
        //    }
        //}

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
        GameObject bulletGO = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        if(bullet != null ) 
        {
            bullet.Set(currentTarget.transform, speed, damage);
        }
    }

    public void UpgradeTower()
    {
        if(upgradeCount > 0)
        {
            upgradeCount--;
        }
        if (upgradeCount <= 0)
            return;
        if (towerTable != null)
        {
            var data = towerTable.GetID(id);
            speed += data.atkspeedInc;
            damage += data.atkInc;
            percent += data.percentIncr;
            towerSpawner.UpgradeTowerPercent(id, percent);
        }
    }
}
