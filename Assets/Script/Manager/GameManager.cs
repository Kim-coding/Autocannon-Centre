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
    public int monsterCount = 0;
    
    public int gold = 50;
    public int health = 100;

    public GameObject failedWindow;
    public GameObject OptionWindow;

    public AudioClip failedSound;
    public AudioClip selectedSound;
    public bool isGameOver { get; private set; }
    private SaveData saveData;

    private void Awake()
    {
        Instance = this;
        if(upgradeTower != null) 
        {
            upgradeTower = GetComponent<UpgradeTower>();
        }
        if(failedWindow != null)
        {
            failedWindow.SetActive(false);
        }
        if(OptionWindow != null)
        {
            OptionWindow.SetActive(false);
        }
        saveData = SaveLoadSystem.LoadGame();
    }

    private void Start()
    {
        if(stage == 0)
            return;

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
            if (failedWindow != null)
            {
                failedWindow.SetActive(true);
            }
            AudioManager.Instance.EffectPlay(failedSound);
            Time.timeScale = 0f;
            return;
        }

        if(wave >= 20 && monsterCount <= 0)
        {
            Debug.Log("ÀúÀå");
            StageClear();
        }
    }

    private void StageClear()
    {
        if (saveData.stagesCleared[stage] == true)
            return;
        
        saveData.stagesCleared[stage] = true;
        SaveLoadSystem.SaveGame(saveData);
    }

    public void UpdateWave(int newWave)
    {
        if (newWave > 20)
            return;
        wave = newWave;
        UIManager.instance.UpdateWaveText(wave);
    }

    public void SetMonsterCount()
    {
        monsterCount += 20;
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
        AudioManager.Instance.EffectPlay(selectedSound);
        Time.timeScale = (Time.timeScale == 1) ? 0 : 1;
        OptionWindow.isStatic = (OptionWindow.isStatic == true) ? false : true;
        OptionWindow.SetActive(OptionWindow.isStatic);
    }
    public void Backspace()
    {
        SceneManager.LoadScene("Start");
    }
}
