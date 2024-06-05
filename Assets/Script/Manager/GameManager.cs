using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public UpgradeTower upgradeTower;
    public TowerSpawner towerSpawner;
    private UIManager uiManager;

    public int stage;
    public int wave = 0;
    private int monsterCount = 0;
    
    private int gold = 50;
    private int health = 100;

    public AudioClip failedSound;
    public AudioClip successSound;

    public bool isGameOver { get; private set; }
    private bool isPlay;
    private SaveData saveData;

    private int gameSpeed = 1;

    private void Awake()
    {
        uiManager = GetComponent<UIManager>();
        isPlay = false;
        Time.timeScale = 1;
        if (upgradeTower != null)
        {
            upgradeTower = GetComponent<UpgradeTower>();
        }

        saveData = SaveLoadSystem.LoadGame();

        if (saveData.tutorial == false && stage == 1)
        {
            uiManager.ShowTutorialPanel();
            Time.timeScale = 0;
        }
    }

    private void Start()
    {
        if(stage == 0)
            return;

        uiManager.UpdateStageText(stage);
        uiManager.UpdateWaveText(wave);
        uiManager.UpdateMonsterText(monsterCount);
        uiManager.UpdateGoldText(gold);
        uiManager.UpdateHealthText(health);
    }

    private void Update()
    {
        if(isGameOver) 
        {
            if(isPlay)
            {
                return;
            }
            uiManager.ShowFailedWindow();
            AudioManager.Instance.SelectedSoundPlay();
            isPlay = true;
            Time.timeScale = 0f;
            return;
        }

        if(wave >= 20 && monsterCount <= 0)
        {
            StageClear();
        }
    }

    public int GetGold()
    {
        return gold;
    }
    public int GetHealth()
    {
        return health;
    }

    public int GetMonsterCount()
    {
        return monsterCount;
    }

    public void Success()
    {
        AudioManager.Instance.EffectStop();
        AudioManager.Instance.EffectPlay(successSound);
        uiManager.ShowSuccessWindow();
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
        uiManager.UpdateWaveText(wave);
    }

    public void SetMonsterCount()
    {
        monsterCount += 20;
        uiManager.UpdateMonsterText(monsterCount);
    }

    public void SubMonsterCount()
    {
        monsterCount--;
        uiManager.UpdateMonsterText(monsterCount);
    }

    public void SubHealth(int subHealth)
    {
        health -= subHealth;
        uiManager.UpdateHealthText(health);
    }

    public void AddGold(int addGold)
    {
        gold += addGold;
        uiManager.UpdateGoldText(gold);
    }

    public void SubGold(int addGold)
    {
        gold -= addGold;
        uiManager.UpdateGoldText(gold);
    }

    public void EndGame()
    {
        AudioManager.Instance.EffectPlay(failedSound);
        isGameOver = true;
    }

    public void OnClickOption()
    {
        uiManager.ToggleOptionWindow(gameSpeed);
    }

    public void Backspace()
    {
        AudioManager.Instance.SelectedSoundPlay();
        SceneManager.LoadScene("Start");
    }

    public void GameSpeed1()
    {
        AudioManager.Instance.SelectedSoundPlay();
        gameSpeed = 1;
    }

    public void GameSpeed2() 
    {
        AudioManager.Instance.SelectedSoundPlay();
        gameSpeed = 2;
    }

    private void ShowTutorialImage(int index)
    {
        uiManager.ShowTutorialImage(index);
    }

    public void OnClickNext()
    {
        uiManager.OnClickNext();
    }

    public void OnClickBack()
    {
        uiManager.OnClickBack();
    }
    private void EndTutorial()
    {
        saveData.tutorial = true;
        SaveLoadSystem.SaveGame(saveData);
        uiManager.EndTutorial();
    }

}
