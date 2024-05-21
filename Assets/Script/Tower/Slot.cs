using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Slot : MonoBehaviour
{
    public TowerData towerData;
    public TextMeshProUGUI slotText;
    public TextMeshProUGUI upgradeText;

    private int upgradeCount = 0;
    private int cost = 25;
    private int costInc = 5;

    public void SetData(TowerData data)
    {
        towerData = data;
        slotText.text = data.name;
    }

    public void OnSlotClick()
    {
        if(upgradeCount < 3)
        {
            if (GameManager.Instance.gold < cost)
            {
                Debug.Log("�ܾ� ����");
                return;
            }

            GameManager.Instance.SubGold(cost);
            GameManager.Instance.upgradeTower.TowerUpgrade(towerData.ID);
            GameManager.Instance.upgradeTower.TowerUpgrade(towerData.ID + 100);
            GameManager.Instance.upgradeTower.TowerUpgrade(towerData.ID + 200);
            upgradeCount++;
            cost += costInc * upgradeCount;
            upgradeText.text = $"+{upgradeCount}";
        }
        else
        {
            Debug.Log("Ƚ�� �ʰ�");
        }
    }
}
