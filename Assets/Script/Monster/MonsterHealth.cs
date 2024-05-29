using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHealth : MonoBehaviour
{
    private float maxHp;
    public float hp;
    public int Gold;

    public bool isDead;
    private int id;

    public Image hpBar;

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
        hpBar.fillAmount = hp / maxHp;
        if(hp <= 0)
        {
            OnDie();
        }
    }

    private void OnDie()
    {
        if (isDead) return;

        isDead = true;

        GameManager.Instance.AddGold(Gold);

        //몬스터 사망 애니메이션
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
        hpBar.fillAmount = 1;
    }
}
