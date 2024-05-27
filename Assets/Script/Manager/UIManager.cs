using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI stage;
    public TextMeshProUGUI wave;
    public TextMeshProUGUI monsterCount;
    public TextMeshProUGUI gold;
    public TextMeshProUGUI health;

    private static UIManager m_instance;
    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>();
            }

            return m_instance;
        }
    }

    public void UpdateStageText(int newStage)
    {
        stage.text = $"스테이지 : {newStage}";
    }

    public void UpdateWaveText(int newWave)
    {
        wave.text = $"웨이브 : {newWave} / 20";
    }

    public void UpdateMonsterText(int newMonsterCount)
    {
        monsterCount.text = $"몬스터 : {newMonsterCount}";
    }
    public void UpdateGoldText(int newGold) 
    {
        gold.text = $" : {newGold}G";
    }
    public void UpdateHealthText(int newHealth) 
    {
        health.text = $" : {newHealth}";
    }
}
