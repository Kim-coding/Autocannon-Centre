    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MonsterData;

public class MonsterHealth : MonoBehaviour
{
    public int hp;
    public int Gold;
    void Start()
    {
        var monsterTable = DataTableMgr.Get<MonsterTable>(DataTableIds.monster);
        foreach (var kvp in monsterTable.monsterTable)
        {
            MonsterData monster = kvp.Value;
            if (monster.monsterName.ToString() == this.name.Replace("(Clone)", ""))
            {
                hp = monster.monsterHP;
                Gold = monster.monsterGold;
            }
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
        //¸ó½ºÅÍ »ç¸Á ¾Ö´Ï¸ÞÀÌ¼Ç
        Debug.Log(Gold + " °ñ È¹µæ !!");
        gameObject.SetActive(false);
    }
}
