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
    public Button stage5;

    private void Start()
    {
        windowManager = GetComponentInParent<WindowManager>();
        saveData = SaveLoadSystem.LoadGame();

        stage1.interactable = true;
        stage2.interactable = saveData.stagesCleared[1];
        stage3.interactable = saveData.stagesCleared[2];
        stage4.interactable = saveData.stagesCleared[3];
        stage5.interactable = saveData.stagesCleared[4];

        stage1.onClick.AddListener(()=>OnClickStage(1));
        stage2.onClick.AddListener(()=>OnClickStage(2));
        stage3.onClick.AddListener(()=>OnClickStage(3));
        stage4.onClick.AddListener(()=>OnClickStage(4));
        stage5.onClick.AddListener(()=>OnClickStage(5));
    }

    public void OnClickStage(int stage)
    {
        AudioManager.Instance.EffectPlay(selectedSound);
        GameManager.Instance.ChangeStage(stage);
    }

    public void Backspace()
    {
        AudioManager.Instance.EffectPlay(selectedSound);

        windowManager.Open(Windows.Start);
    }
}
