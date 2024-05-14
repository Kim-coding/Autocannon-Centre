using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Slot : MonoBehaviour
{
    public TowerData towerData;
    public TextMeshProUGUI slotText;

    public void SetData(TowerData data)
    {
        towerData = data;
        slotText.text = data.name;
    }

    public void OnSlotClick()
    {
        Debug.Log(towerData.ID);
        GameManager.Instance.upgradeTower.TowerUpgrade(towerData.ID);
        //towerData.ID;�� �ѱ��
        //�ش� ID�� ������ �ִ� Ÿ���� speed�� damage, percent�� ����
        //�ش� ID�� ������ �ִ� ������ Ÿ���� �����̰�, ���Ŀ� ������ �ش� ID�� ������ Ÿ���� speed�� damage, percent�� �����ؾ���
    }
}
