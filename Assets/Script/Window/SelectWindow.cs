using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectWindow : MonoBehaviour
{
    private WindowManager windowManager;
    public AudioClip selectedSound;

    SaveData saveData;

    public Button stage1;
    public Button stage2;
    public Button stage3;
    public Button stage4;

    private void Start()
    {
        windowManager = GetComponentInParent<WindowManager>();
        saveData = SaveLoadSystem.LoadGame();

        stage1.interactable = true;
        stage2.interactable = saveData.stagesCleared[1];
        stage3.interactable = saveData.stagesCleared[2];
        stage4.interactable = saveData.stagesCleared[3];
    }
    public void OnClickStage1()
    {
        AudioManager.Instance.EffectPlay(selectedSound);

        SceneManager.LoadScene("1Level");
    }

    public void OnClickStage2()
    {
        if (saveData.stagesCleared[1])
        {
            AudioManager.Instance.EffectPlay(selectedSound);
            SceneManager.LoadScene("2Level");
        }
    }
    public void OnClickStage3()
    {
        if (saveData.stagesCleared[2])
        {
            AudioManager.Instance.EffectPlay(selectedSound);
            SceneManager.LoadScene("3Level");
        }
    }

    public void OnClickStage4()
    {
        if (saveData.stagesCleared[3])
        {
            AudioManager.Instance.EffectPlay(selectedSound);
            SceneManager.LoadScene("4Level");
        }
    }
    public void Backspace()
    {
        AudioManager.Instance.EffectPlay(selectedSound);

        windowManager.Open(Windows.Start);
    }
}
