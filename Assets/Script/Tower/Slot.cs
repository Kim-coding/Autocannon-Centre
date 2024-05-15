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

    public void SetData(TowerData data)
    {
        towerData = data;
        slotText.text = data.name;
    }

    public void OnSlotClick()
    {
        if(upgradeCount < 3)
        {
            Debug.Log(towerData.ID + " 업그레이드");
            GameManager.Instance.upgradeTower.TowerUpgrade(towerData.ID);
            upgradeCount++;
            upgradeText.text = $"+{upgradeCount}";
        }
        else
        {
            Debug.Log("횟수 초과");
        }
    }
}
