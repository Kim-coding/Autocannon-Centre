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
    public GameObject optionWindow;
    public GameObject Plane;

    public AudioClip failedSound;
    public AudioClip selectedSound;
    public bool isGameOver { get; private set; }
    private bool isPlay;
    private SaveData saveData;

    private int gameSpeed = 1;
    private void Awake()
    {
        isPlay = false;
        Time.timeScale = 1;
        Instance = this;
        if(upgradeTower != null) 
        {
            upgradeTower = GetComponent<UpgradeTower>();
        }
        if(failedWindow != null)
        {
            failedWindow.SetActive(false);
        }
        if(optionWindow != null)
        {
            optionWindow.SetActive(false);
        }
        if (Plane != null)
        {
            Plane.SetActive(false);
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
            if(isPlay)
            {
                return;
            }
            if (failedWindow != null)
            {
                failedWindow.SetActive(true);
            }
            AudioManager.Instance.EffectPlay(failedSound);
            isPlay = true;
            Time.timeScale = 0f;
            return;
        }

        if(wave >= 20 && monsterCount <= 0)
        {
            Debug.Log("����");
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
        bool isOptionWindowActive = optionWindow.activeSelf;

        if (isOptionWindowActive)
        {
            Time.timeScale = gameSpeed;
            optionWindow.SetActive(false);
            Plane.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            optionWindow.SetActive(true);
            Plane.SetActive(true);
        }
    }

    public void Backspace()
    {
        SceneManager.LoadScene("Start");
    }

    public void GameSpeed1()
    {
        gameSpeed = 1;
    }

    public void GameSpeed2() 
    {
        gameSpeed = 2;
    }

    public void ChangeStage(int newStage)
    {
        stage = newStage;
        SceneManager.LoadScene($"{newStage}Level");
    }
}
