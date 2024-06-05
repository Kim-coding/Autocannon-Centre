using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectWindow : MonoBehaviour
{
    private WindowManager windowManager;

    SaveData saveData;

    public Button[] stage;

    private void Start()
    {
        windowManager = GetComponentInParent<WindowManager>();
        saveData = SaveLoadSystem.LoadGame();

        for (int i = 0; i < stage.Length; i++)
        {
            int index = i;
            stage[i].onClick.AddListener(() => OnClickStage(index + 1));
        }
    }

    public void OnClickStage(int stage)
    {
        AudioManager.Instance.SelectedSoundPlay();
        SceneManager.LoadScene($"{stage}Level");
    }

    public void Backspace()
    {
        AudioManager.Instance.SelectedSoundPlay();

        windowManager.Open(Windows.Start);
    }
}
