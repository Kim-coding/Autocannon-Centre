    using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static MonsterData;

public class MonsterHealth : MonoBehaviour
{
    public int hp;
    public int Gold;

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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            OnDamage(100);
        }
    }

    private void OnDamage(int damage)
    {
        hp -= damage;
        if(hp < 0)
        {
            OnDie();
        }
    }

    private void OnDie()
    {
        //���� ��� �ִϸ��̼�
        Debug.Log(Gold + " �� ȹ�� !!");
        gameObject.SetActive(false);
    }
}
