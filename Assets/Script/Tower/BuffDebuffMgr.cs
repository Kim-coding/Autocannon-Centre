using System.Collections.Generic;
using UnityEngine;

public class BuffDebuffMgr
{
    private HashSet<Tower> buffedTowers = new HashSet<Tower>();
    private HashSet<MonsterMove> debuffedMonsters = new HashSet<MonsterMove>();
    private Tower buffDebuffTower;

    public BuffDebuffMgr(Tower tower)
    {
        buffDebuffTower = tower;
    }

    public void ApplyBuffsAndDebuffs(SkillData skillData)
    {
        Collider[] colliders = Physics.OverlapSphere(buffDebuffTower.transform.position, buffDebuffTower.range);

        HashSet<Tower> currentBuffedTowers = new HashSet<Tower>();
        HashSet<MonsterMove> currentDebuffedMonsters = new HashSet<MonsterMove>();

        foreach (Collider collider in colliders)
        {
            if (collider != null && skillData != null)
            {
                if (collider.gameObject.CompareTag("Tower") && collider.gameObject != buffDebuffTower.gameObject && skillData.buffType != 0)
                {
                    Tower tower = collider.GetComponent<Tower>();
                    if (tower != null)
                    {
                        currentBuffedTowers.Add(tower);
                        if (!buffedTowers.Contains(tower))
                        {
                            ApplyBuff(tower, skillData.buffType, skillData.value);
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
        if (buffType == 1) 
        {
            tower.damage += value;
        }
        else if (buffType == 2)
        {
            tower.fireRate -= value;
        }
        buffedTowers.Add(tower);
    }

    private void RemoveBuff(Tower tower, int buffType, float value)
    {
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
        if (debuffType == 1)
        {
            monster.speed -= value;
        }
        debuffedMonsters.Add(monster);
    }

    private void RemoveDebuff(MonsterMove monster, int debuffType, float value)
    {
        if (debuffType == 1)
        {
            monster.speed += value;
        }
    }

    public void ClearAll()
    {
        foreach (var tower in buffedTowers)
        {
            RemoveBuff(tower, buffDebuffTower.skillData.buffType, buffDebuffTower.skillData.value);
        }
        buffedTowers.Clear();

        foreach (var monster in debuffedMonsters)
        {
            RemoveDebuff(monster, buffDebuffTower.skillData.debuffType, buffDebuffTower.skillData.value);
        }
        debuffedMonsters.Clear();
    }
}
