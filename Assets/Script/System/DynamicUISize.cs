using UnityEngine;
using UnityEngine.UI;

public class DynamicUISize : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector2 lastScreenSize;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        lastScreenSize = new Vector2(Screen.width, Screen.height);
        UpdateUISize();
    }

    private void Update()
    {
        if (Screen.width != lastScreenSize.x || Screen.height != lastScreenSize.y)
        {
            lastScreenSize = new Vector2(Screen.width, Screen.height);
            UpdateUISize();
        }
    }

    private void UpdateUISize()
    {
        rectTransform.sizeDelta = new Vector2(Screen.width * 0.5f, Screen.height * 0.3f);
    }
}