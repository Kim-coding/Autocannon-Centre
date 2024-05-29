using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public UpgradeTower upgradeTower;
    public TowerSpawner towerSpawner;

    public int stage;
    public int wave = 0;
    public int monsterCount = 0;
    
    public int gold = 50;
    public int health = 100;

    public GameObject failedWindow;
    public GameObject optionWindow;
    public GameObject Plane;

    public GameObject tutorialPanel;
    public List<GameObject> tutorialImages;
    public List<GameObject> tutorialInfo;
    public Button nextButton;
    public Button backButton;
    public TextMeshProUGUI nextButtonText;

    public AudioClip failedSound;
    public AudioClip selectedSound;

    public bool isGameOver { get; private set; }
    private bool isPlay;
    private SaveData saveData;

    private int gameSpeed = 1;
    private int currentTutorialImageIndex = 0;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        isPlay = false;
        Time.timeScale = 1;
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

        if(saveData.tutorial == false && stage == 1)
        {
            tutorialPanel.SetActive(true);
            Plane.SetActive(true);
            ShowTutorialImage(currentTutorialImageIndex);
            Time.timeScale = 0;
        }
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
        towerSpawner.ResetAllTowers();
        SceneManager.LoadScene($"{newStage}Level");
    }

    private void ShowTutorialImage(int index)
    {
        if (index < 0 || index >= tutorialImages.Count)
            return;

        foreach (var image in tutorialImages)
        {
            image.SetActive(false);
        }
        foreach ( var image in tutorialInfo)
        {
            image.SetActive(false);
        }

        tutorialImages[index].SetActive(true);
        tutorialInfo[index].SetActive(true);
        currentTutorialImageIndex = index;

        // backButton 활성화/비활성화 설정
        backButton.gameObject.SetActive(index > 0);

        // nextButton 텍스트 설정
        if (index == tutorialImages.Count - 1)
        {
            nextButtonText.text = "완료";
        }
        else
        {
            nextButtonText.text = "다음";
        }
    }

    public void OnClickNext()
    {
        int nextIndex = currentTutorialImageIndex + 1;

        if (nextIndex < tutorialImages.Count)
        {
            ShowTutorialImage(nextIndex);
        }
        else
        {
            EndTutorial();
        }
    }

    public void OnClickBack()
    {
        int nextIndex = currentTutorialImageIndex - 1;

        if(nextIndex >= 0 && nextIndex < tutorialImages.Count)
        {
            ShowTutorialImage(nextIndex);
        }
    }
    private void EndTutorial()
    {
        saveData.tutorial = true;
        SaveLoadSystem.SaveGame(saveData);
        tutorialPanel.SetActive(false);
        Plane.SetActive(false);
        Time.timeScale = 1;
    }

}
