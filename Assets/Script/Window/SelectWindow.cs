using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectWindow : MonoBehaviour
{
    private WindowManager windowManager;

    private void Start()
    {
        windowManager = GetComponentInParent<WindowManager>();
    }
    public void OnClickStage1()
    {
        SceneManager.LoadScene("1Level");
    }

    public void OnClickStage2()
    {
        SceneManager.LoadScene("2Level");
    }
    public void OnClickStage3()
    {
        SceneManager.LoadScene("3Level");
    }
    public void OnClickStage4()
    {
        SceneManager.LoadScene("4Level");
    }

    public void Backspace()
    {
        windowManager.Open(Windows.Start);
    }
}
