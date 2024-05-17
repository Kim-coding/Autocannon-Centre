using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public UpgradeTower upgradeTower;

    public int stage;
    public int wave = 0;
    public int monsterCount = 20;
    
    public int gold = 50;
    public int health = 100;

    public bool isGameOver { get; private set; }

    private void Awake()
    {
        Instance = this;
        upgradeTower = GetComponent<UpgradeTower>();
    }

    private void Start()
    {
        UIManager.instance.UpdateStageText(stage);
        UIManager.instance.UpdateWaveText(wave);
        UIManager.instance.UpdateMonsterText(monsterCount);
        UIManager.instance.UpdateGoldText(gold);
        UIManager.instance.UpdateHealthText(health);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 1.0f;
            isGameOver = false;
            SceneManager.LoadScene("Start");
        }

        if(isGameOver) 
        {
            Time.timeScale = 0f;
            return;
        }
    }

    public void UpdateWave(int newWave)
    {
        wave = newWave;
        UIManager.instance.UpdateWaveText(wave);
    }

    public void SetMonsterCount()
    {
        monsterCount = 20;
        UIManager.instance.UpdateMonsterText(monsterCount);
    }

    public void SubMonsterCount()
    {
        monsterCount--;
        UIManager.instance.UpdateMonsterText(monsterCount);
    }

    public void SubHealth(int subHealth)
    {
        health -= subHealth;
        UIManager.instance.UpdateHealthText(health);
    }

    public void AddGold(int addGold)
    {
        gold += addGold;
        UIManager.instance.UpdateGoldText(gold);
    }

    public void SubGold(int addGold)
    {
        gold -= addGold;
        UIManager.instance.UpdateGoldText(gold);
    }

    public void EndGame()
    {
        isGameOver = true;
    }

    public void OnClickOption()
    {

    }
}
