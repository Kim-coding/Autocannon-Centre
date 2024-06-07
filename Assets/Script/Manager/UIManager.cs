using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Numerics;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI stage;
    public TextMeshProUGUI wave;
    public TextMeshProUGUI monsterCount;
    public TextMeshProUGUI gold;
    public TextMeshProUGUI health;

    public GameObject failedWindow;
    public GameObject successWindow;
    public GameObject optionWindow;

    public GameObject tutorialPanel;
    public List<GameObject> tutorialImages;
    public List<GameObject> tutorialInfo;
    public Button nextButton;
    public Button backButton;
    public TextMeshProUGUI nextButtonText;

    private int currentTutorialImageIndex = 0;
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
        monsterCount.text = $"남은 몬스터 : {newMonsterCount}";
    }
    public void UpdateGoldText(int newGold) 
    {
        gold.text = $" : {newGold}G";
    }
    public void UpdateHealthText(int newHealth) 
    {
        health.text = $" : {newHealth}";
    }
    public void ShowFailedWindow()
    {
        if (failedWindow != null)
        {
            failedWindow.SetActive(true);
        }
    }

    public void ShowSuccessWindow()
    {
        if (successWindow != null)
        {
            successWindow.SetActive(true);
        }
    }
    public void ToggleOptionWindow(int gameSpeed)
    {
        if (optionWindow != null)
        {
            bool isOptionWindowActive = optionWindow.activeSelf;

            if (isOptionWindowActive)
            {
                Time.timeScale = gameSpeed;
                optionWindow.SetActive(false);
            }
            else
            {
                Time.timeScale = 0;
                optionWindow.SetActive(true);
            }
        }
    }
    public void ShowTutorialPanel()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true);
        }
    }

    public void ShowTutorialImage(int index)
    {
        if (index < 0 || index >= tutorialImages.Count)
            return;

        foreach (var image in tutorialImages)
        {
            image.SetActive(false);
        }
        foreach (var image in tutorialInfo)
        {
            image.SetActive(false);
        }

        tutorialImages[index].SetActive(true);
        tutorialInfo[index].SetActive(true);
        currentTutorialImageIndex = index;

        backButton.gameObject.SetActive(index > 0);

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
        AudioManager.Instance.SelectedSoundPlay();
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
        AudioManager.Instance.SelectedSoundPlay();
        int nextIndex = currentTutorialImageIndex - 1;

        if (nextIndex >= 0 && nextIndex < tutorialImages.Count)
        {
            ShowTutorialImage(nextIndex);
        }
    }

    public void EndTutorial()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
