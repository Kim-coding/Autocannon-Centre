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

    }

    public void OnDamage(int damage)
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
        GameManager.Instance.AddGold(Gold);
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
