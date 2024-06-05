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

    private GameManager gameManager;
    private void Awake()
    {
        GameObject gameManagerObject = GameObject.FindWithTag("GameController");
        if (gameManagerObject != null)
        {
            gameManager = gameManagerObject.GetComponent<GameManager>();
        }
        button = this.GetComponent<Button>();
    }

    public void SetData(TowerData data)
    {
        towerData = data;
        slotText.text = data.name;
    }

    private void Update()
    {
        if(gameManager.GetGold() < cost)
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
            if (gameManager.GetGold() < cost)
            {
                return;
            }
            AudioManager.Instance.EffectPlay(sound);
            gameManager.SubGold(cost);
            for (int i = 0; i < 3; i++)
            {
                gameManager.upgradeTower.TowerUpgrade(towerData.ID + i * 100);
            }
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
