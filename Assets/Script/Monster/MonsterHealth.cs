using CsvHelper.Configuration.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static MonsterData;

public class MonsterHealth : MonoBehaviour
{
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
            Gold = data.monsterGold;
        }
    }

    private void Update()
    {
        if (isDead) return;
    }

    public void OnDamage(int damage)
    {
        

        hp -= damage;
        if(hp <= 0)
        {
            OnDie();
        }
    }

    private void OnDie()
    {
        if (isDead) return;

        //���� ��� �ִϸ��̼�
        isDead = true;

        Debug.Log(Gold + " �� ȹ�� !!");
        GameManager.Instance.AddGold(Gold);
        GameManager.Instance.SubMonsterCount(1);

        Destroy(gameObject);
    }
}
