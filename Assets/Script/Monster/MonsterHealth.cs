using CsvHelper.Configuration.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static MonsterData;

public class MonsterHealth : MonoBehaviour
{
    private int maxHp;
    public int hp;
    public int Gold;

    private bool isDead = false;
    private int id;
    void Start()
    {
        id = int.Parse(name.Replace("(Clone)", ""));
        var monsterTable = DataTableMgr.Get<MonsterTable>(DataTableIds.monster);

        if (monsterTable != null)
        {
            var data = monsterTable.GetID(id);
            hp = data.monsterHP;
            maxHp = hp;
            Gold = data.monsterGold;
        }
       
    }

    public void OnDamage(int damage)
    {
        if (isDead) return;

        hp -= damage;
        if(hp <= 0)
        {
            OnDie();
        }
    }

    private void OnDie()
    {
        if (isDead) return;

        //¸ó½ºÅÍ »ç¸Á ¾Ö´Ï¸ÞÀÌ¼Ç
        isDead = true;

        Debug.Log(Gold + " °ñ È¹µæ !!");
        GameManager.Instance.AddGold(Gold);
        GameManager.Instance.SubMonsterCount();

        PoolManager.instance.ReturnObjectToPool(gameObject);
    }

    private void OnEnable()
    {
        hp = maxHp;
        isDead = false;
    }
}
