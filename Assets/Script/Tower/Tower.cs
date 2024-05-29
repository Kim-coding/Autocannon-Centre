using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject currentTarget;
    public GameObject[] soldiers;

    public string towerName;
    public float damage;
    public float range;
    public float speed;
    public int percent;
    public TowerTable towerTable;
    public float fireRate;
    public float fireTime;

    public int towerGrade;
    public int type;
    public int skillID;
    public int id;
    public string towerIcon;
    public GameObject onRange;

    public AudioClip fireSound;

    public SkillData skillData;

    private BuffDebuffMgr buffDebuffmgr;
    public string TowerID {  get; private set; }

    private float initialDamage;
    private float initialSpeed;
    private float initialFireRate;
    private int initialPercent;

    private void Awake()
    {
        TowerID = $"{name.Replace("(Clone)", "")}_{DateTime.Now.Ticks}";

        id = int.Parse(name.Replace("(Clone)", ""));

        towerTable = DataTableMgr.Get<TowerTable>(DataTableIds.tower);
        if (towerTable != null)
        {
            var data = towerTable.GetID(id);
            towerName = data.name;
            speed = data.atkSpeed;
            range = data.atkRange;
            damage = data.damage;
            percent = data.percent;
            towerGrade = data.towerGrade;
            type = data.type;
            skillID = data.skillID;
            fireRate = data.towerSpeed;
            towerIcon = data.towerIcon;
            Debug.Log(data.damage);
        }
        SaveInitialState();

        if (type == 2)
        {
            var skillTable = DataTableMgr.Get<SkillTable>(DataTableIds.towerSkill);
            if (skillTable != null)
            {
                skillData = skillTable.GetID(skillID);
            }
            buffDebuffmgr = new BuffDebuffMgr(this);
        }

        if (bulletPrefab != null)
        {
            PoolManager.instance.CreatePool(bulletPrefab, 1);
        }
    }
    public void InitializeTower()
    {
        ResetState();
    }


    private void SaveInitialState()
    {
        initialDamage = damage;
        initialSpeed = speed;
        initialFireRate = fireRate;
        initialPercent = percent;
    }

    public void ResetState()
    {
        damage = initialDamage;
        speed = initialSpeed;
        fireRate = initialFireRate;
        percent = initialPercent;
    }

    private void Update()
    {
        if (type == 2 && buffDebuffmgr != null)
        {
            buffDebuffmgr.ApplyBuffsAndDebuffs(skillData);
        }
        else
        {
            UpdateCurrentTarget();
            
            if (currentTarget != null && IsTargetOutOfRange(currentTarget))
            {
                currentTarget = null;
            }

            foreach (var soldier in soldiers)
            {
                if (soldier != null && currentTarget != null && currentTarget.activeInHierarchy)
                {
                    soldier.transform.LookAt(currentTarget.transform.position);
                    soldier.transform.Rotate(0, 180, 0);
                }
            }

            for (int i = 0; i < towerGrade; i++)
            {
                fireTime += Time.deltaTime;
                if (currentTarget != null && currentTarget.activeInHierarchy && fireTime > fireRate)
                {
                    fireTime = 0f;
                    Shoot(currentTarget);
                    
                }
            }
        }
    }

    private void UpdateCurrentTarget()
    {
        if (currentTarget == null || IsTargetOutOfRange(currentTarget) || !currentTarget.activeInHierarchy) 
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
            if(collider != null && collider.gameObject.activeInHierarchy && collider.gameObject.CompareTag("monster"))
            {
                
                float distance = Vector3.Distance(transform.position, collider.gameObject.transform.position);
                if (distance < shortDistance)
                {
                    shortDistance = distance;
                    nearestMonster = collider.gameObject;
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
        if (bulletPrefab == null || target == null)
        {
            return;
        }
        var pos = transform.position;
        pos.y += 2f;

        GameObject bulletGO = PoolManager.instance.GetObjectPool(bulletPrefab.name);
        if(bulletGO != null) 
        {
            bulletGO.transform.position = pos;
            bulletGO.transform.rotation = Quaternion.identity;

            Bullet bullet = bulletGO.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.Set(target.transform, speed, damage, range);
                AudioManager.Instance.EffectPlay(fireSound);
            }
        }
    }

    public void UpgradeTower(TowerData data)
    {
        if (towerTable != null)
        {
            if(data.type == 2)
            {
                percent += data.percentIncr;
            }
            else
            {
                fireRate -= data.towerSpeedInc;
                damage += data.atkInc;
                percent += data.percentIncr;
            }
        }
    }

    private void OnDestroy()
    {
        if (buffDebuffmgr != null)
        {
            buffDebuffmgr.ClearAll();
        }
    }
}
