using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowManager : MonoBehaviour
{
    public GameObject startWindow;
    public GameObject optionWindow;
    public GameObject selectStageWindow;

    private Dictionary<Windows, GameObject> windows;

    private void Start()
    {
        windows = new Dictionary<Windows, GameObject>()
        {
            {Windows.Start, startWindow },
            {Windows.Option, optionWindow},
            {Windows.SelectStage, selectStageWindow }
        };

        Open(Windows.Start);
    }

    public void Open(Windows window)
    {
        foreach (var win in windows) 
        {
            win.Value.SetActive(false);
        }

        if(windows.ContainsKey(window)) 
        {
            windows[window].SetActive(true);
        }
    }
}
