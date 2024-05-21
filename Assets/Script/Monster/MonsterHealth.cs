using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterHealth : MonoBehaviour
{
    private float maxHp;
    public float hp;
    public int Gold;

    public bool isDead;
    private int id;

    private Animator animator;

    void Start()
    {
        isDead = false;

        id = int.Parse(name.Replace("(Clone)", ""));
        var monsterTable = DataTableMgr.Get<MonsterTable>(DataTableIds.monster);

        if (monsterTable != null)
        {
            var data = monsterTable.GetID(id);
            hp = data.monsterHP;
            maxHp = hp;
            Gold = data.monsterGold;
        }

        animator = GetComponent<Animator>();
    }

    public void OnDamage(float damage)
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

        isDead = true;

        Debug.Log(Gold + " °ñ È¹µæ !!");
        GameManager.Instance.AddGold(Gold);

        //¸ó½ºÅÍ »ç¸Á ¾Ö´Ï¸ÞÀÌ¼Ç
        animator.SetTrigger("Die");

        float dieAniLength = animator.GetCurrentAnimatorStateInfo(0).length;
        StartCoroutine(DieAnimation(dieAniLength));
    }

    private IEnumerator DieAnimation(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        GameManager.Instance.SubMonsterCount();
        PoolManager.instance.ReturnObjectToPool(gameObject);
    }

    private void OnEnable()
    {
        hp = maxHp;
        isDead = false;
    }
}
