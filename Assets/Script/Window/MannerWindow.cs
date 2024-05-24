using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MannerWindow : MonoBehaviour
{
    private WindowManager windowManager;

    public AudioClip selectedSound;
    public Button[] buttons;
    public GameObject[] imges;

    public void Start()
    {
        windowManager = GetComponentInParent<WindowManager>();

        for(int i = 0; i < buttons.Length; i++) 
        {
            int index = i;
            buttons[i].onClick.AddListener(()=>OnClick(index));
        }
    }

    public void OnClick(int index)
    {
        AudioManager.Instance.EffectPlay(selectedSound);
        for (int i = 0; i < imges.Length; i++) 
        {
            imges[i].SetActive(i == index);
        }
    }

   
    public void Backspace()
    {
        windowManager.Open(Windows.Start);
    }
}
