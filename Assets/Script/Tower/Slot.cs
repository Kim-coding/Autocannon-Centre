using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public TowerData towerData;
    public TextMeshProUGUI slotText;
    public TextMeshProUGUI upgradeText;
    public AudioClip sound;

    private int upgradeCount = 0;
    private int cost = 25;
    private int costInc = 5;

    private Button button;
    private void Awake()
    {
        button = this.GetComponent<Button>();
    }

    public void SetData(TowerData data)
    {
        towerData = data;
        slotText.text = data.name;
    }

    private void Update()
    {
        if(GameManager.Instance.GetGold() < cost)
        {
            button.enabled = false;
        }
        else
        {
            button.enabled = true;
        }
    }

    public void OnSlotClick()
    {
        if(upgradeCount < 3)
        {
            if (GameManager.Instance.GetGold() < cost)
            {
                return;
            }
            AudioManager.Instance.EffectPlay(sound);
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
            return;
        }
    }
}
