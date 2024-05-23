using UnityEngine;
using UnityEngine.UI;

public class StartWindow : MonoBehaviour
{
    public Button gameButton;
    public Button optionButton;

    private WindowManager windowManager;
    public AudioClip selectedSound;

    void Start()
    {
        windowManager = GetComponentInParent<WindowManager>();

        gameButton.onClick.AddListener(OnClickGame);
        optionButton.onClick.AddListener(OnClickManner);
    }
    public void OnClickGame()
    {
        AudioManager.Instance.EffectPlay(selectedSound);
        windowManager.Open(Windows.SelectStage);
    }

    public void OnClickManner()
    {
        windowManager.Open(Windows.Manner);
    }

    public void OnClickExit()
    {
#if UNITY_EDITOR
        //Application.isPlaying=false;
#else
        Application.Quit();
#endif
    }
}
