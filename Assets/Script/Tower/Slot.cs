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
        //towerData.ID;를 넘기면
        //해당 ID를 가지고 있는 타워의 speed와 damage, percent가 증가
        //해당 ID를 가지고 있는 생성된 타워는 물론이고, 이후에 생성될 해당 ID를 가지는 타워도 speed와 damage, percent가 증가해야함
    }
}
