using System.Collections.Generic;
using UnityEngine;

public class BuffDebuffMgr
{
    private HashSet<Tower> buffedTowers = new HashSet<Tower>();
    private HashSet<MonsterMove> debuffedMonsters = new HashSet<MonsterMove>();
    private Tower ownerTower;

    public BuffDebuffMgr(Tower owner)
    {
        ownerTower = owner;
    }

    public void ApplyBuffsAndDebuffs(SkillData skillData)
    {
        Collider[] colliders = Physics.OverlapSphere(ownerTower.transform.position, ownerTower.range);

        HashSet<Tower> currentBuffedTowers = new HashSet<Tower>();
        HashSet<MonsterMove> currentDebuffedMonsters = new HashSet<MonsterMove>();

        foreach (Collider collider in colliders)
        {
            if (collider != null && skillData != null)
            {
                if (collider.gameObject.CompareTag("Tower") && collider.gameObject != ownerTower.gameObject && skillData.buffType != 0)
                {
                    Tower tower = collider.GetComponent<Tower>();
                    currentBuffedTowers.Add(tower);

                    if (tower != null)
                        {
                            if (!buffedTowers.Contains(tower))
                            {
                                ApplyBuff(tower, skillData.buffType, skillData.value);
                                buffedTowers.Add(tower);
                            }
                        }
                }
                else if (collider.gameObject.CompareTag("monster") && skillData.debuffType != 0)
                {
                    MonsterMove monster = collider.GetComponent<MonsterMove>();
                    if (monster != null)
                    {
                        currentDebuffedMonsters.Add(monster);
                        if (!debuffedMonsters.Contains(monster))
                        {
                            ApplyDebuff(monster, skillData.debuffType, skillData.value);
                            debuffedMonsters.Add(monster);
                        }
                    }
                }
            }
        }

        foreach (var tower in buffedTowers)
        {
            if (!currentBuffedTowers.Contains(tower))
            {
                RemoveBuff(tower, skillData.buffType, skillData.value);
            }
        }
        buffedTowers = currentBuffedTowers;

        foreach (var monster in debuffedMonsters)
        {
            if (!currentDebuffedMonsters.Contains(monster))
            {
                RemoveDebuff(monster, skillData.debuffType, skillData.value);
            }
        }
        debuffedMonsters = currentDebuffedMonsters;
    }

    private void ApplyBuff(Tower tower, int buffType, float value)
    {
        Debug.Log("버프 적용: " + tower.towerName);
        if(buffType == 1) 
        {
            tower.damage += value;
        }
        else if (buffType == 2)
        {
            tower.fireRate -= value;
        }
    }

    private void RemoveBuff(Tower tower, int buffType, float value)
    {
        Debug.Log("버프 제거: " + tower.towerName);
        if (buffType == 1)
        {
            tower.damage -= value;
        }
        if (buffType == 2)
        {
            tower.fireRate += value;
        }
    }

    private void ApplyDebuff(MonsterMove monster, int debuffType, float value)
    {
        Debug.Log("몬스터 디버프 적용: ");
        if (debuffType == 1)
        {
            monster.speed -= value;
        }
    }

    private void RemoveDebuff(MonsterMove monster, int debuffType, float value)
    {
        Debug.Log("몬스터 디버프 제거: ");
        if (debuffType == 1)
        {
            monster.speed += value;
        }
    }

    public void ClearAll()
    {
        foreach (var tower in buffedTowers)
        {
            RemoveBuff(tower, ownerTower.skillData.buffType, ownerTower.skillData.value);
        }
        buffedTowers.Clear();

        foreach (var monster in debuffedMonsters)
        {
            RemoveDebuff(monster, ownerTower.skillData.debuffType, ownerTower.skillData.value);
        }
        debuffedMonsters.Clear();
    }
}
