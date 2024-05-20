using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject bulletPrefab;
    private GameObject currentTarget;
    public GameObject[] soldiers;

    public string towerName;
    public int damage;
    public float range;
    public float speed;
    public int percent;
    public TowerTable towerTable;
    public float fireRate = 0.25f;
    public float fireTime;

    public int towerGrade;
    public int type;
    public int skillID;
    public int id;

    public SkillData skillData;
    public string TowerID {  get; private set; }


    private void Start()
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
        }

        if (type == 2)
        {
            var skillTable = DataTableMgr.Get<SkillTable>(DataTableIds.monsterWave);
            if (skillTable != null)
            {
                skillData = skillTable.GetID(skillID);
            }
        }

        if (bulletPrefab != null)
        {
            PoolManager.instance.CreatePool(bulletPrefab, 1);
        }
    }

    private void Update()
    {
        if (type == 2)
        {
            BuffDebuff();
        }
        else
        {
            UpdateCurrentTarget();

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
                    Shoot(currentTarget);
                    fireTime = 0f;
                }
            }
        }
    }

    private void UpdateCurrentTarget()
    {
        if (currentTarget == null || IsTargetOutOfRange(currentTarget) || !currentTarget.activeInHierarchy)
        {
            //���ο� Ÿ�� ����
            currentTarget = FindTarget();
        }
    }

    private bool IsTargetOutOfRange(GameObject target)  // ���� Ÿ�ٰ��� �Ÿ� �˻�
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

    private void BuffDebuff()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);

        foreach (Collider collider in colliders)
        {
            if (collider != null)
            {
                if (skillData != null)
                {
                    if (collider.gameObject.CompareTag("Tower") && skillData.buffType != 0) // ���� ����
                    {
                        Tower tower = collider.GetComponent<Tower>();
                        if (tower != null)
                        {
                            Buff(tower, skillData.buffType, skillData.value);
                        }
                    }
                    else if (collider.gameObject.CompareTag("monster") && skillData.debuffType != 0) // ����� ����
                    {
                        MonsterHealth monster = collider.GetComponent<MonsterHealth>();
                        if (monster != null)
                        {
                            Debuff(monster, skillData.debuffType, skillData.value);
                        }
                    }
                }
            }
        }
    }

    private void Buff(Tower tower, int buffType, int value)
    {
        // ������ ��� ���� or ���� �ӵ� ����
        Debug.Log("����");
    }

    private void Debuff(MonsterHealth monster, int debuffType, int value)
    {
        // ���� �̵� �ӵ� �����
        Debug.Log("�����");
    }
}
