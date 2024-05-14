using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Slot : MonoBehaviour
{
    public TowerData towerData;
    public TextMeshProUGUI slotText;

    private int maxCount = 3;

    public void SetData(TowerData data)
    {
        towerData = data;
        slotText.text = data.name;
    }

    public void OnSlotClick()
    {
        if(maxCount > 0)
        {
            Debug.Log(towerData.ID + " 업그레이드");
            GameManager.Instance.upgradeTower.TowerUpgrade(towerData.ID);
            maxCount--;
        }
        else
        {
            Debug.Log("잔액 부족");
        }
    }
}
