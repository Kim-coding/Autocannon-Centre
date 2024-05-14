using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartWindow : MonoBehaviour
{
    public Button gameButton;
    public Button optionButton;

    private WindowManager windowManager;

    void Start()
    {
        windowManager = GetComponentInParent<WindowManager>();

        gameButton.onClick.AddListener(OnClickGame);
        optionButton.onClick.AddListener(OnClickOption);
    }
    public void OnClickGame()
    {
        windowManager.Open(Windows.SelectStage);
    }

    public void OnClickOption()
    {
        windowManager.Open(Windows.Option);
    }
}
