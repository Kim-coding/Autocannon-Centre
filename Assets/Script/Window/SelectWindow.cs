using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectWindow : MonoBehaviour
{
    private WindowManager windowManager;
    public AudioClip selectedSound;

    private void Start()
    {
        windowManager = GetComponentInParent<WindowManager>();
    }
    public void OnClickStage1()
    {
        AudioManager.Instance.EffectPlay(selectedSound);

        SceneManager.LoadScene("1Level");
    }

    public void OnClickStage2()
    {
        AudioManager.Instance.EffectPlay(selectedSound);

        SceneManager.LoadScene("2Level");
    }
    public void OnClickStage3()
    {
        AudioManager.Instance.EffectPlay(selectedSound);

        SceneManager.LoadScene("3Level");
    }
    public void OnClickStage4()
    {
        AudioManager.Instance.EffectPlay(selectedSound);

        SceneManager.LoadScene("4Level");
    }

    public void Backspace()
    {
        AudioManager.Instance.EffectPlay(selectedSound);

        windowManager.Open(Windows.Start);
    }
}
