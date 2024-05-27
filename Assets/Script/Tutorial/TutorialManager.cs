using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject plane;
    public TextMeshProUGUI[] tutorial;
    private int currentTutorialIndex = 0;

    private void Awake()
    {
        Time.timeScale = 0f;
        plane.gameObject.SetActive(true);
        ShowTutorial(currentTutorialIndex);
    }

    public void OnClickNext()
    {
        currentTutorialIndex++;
        if (currentTutorialIndex < tutorial.Length)
        {
            ShowTutorial(currentTutorialIndex);
        }
        else
        {
            Debug.Log("모든 튜토리얼이 완료되었습니다!");
        }
    }

    private void ShowTutorial(int index)
    {
        for (int i = 0; i < tutorial.Length; i++)
        {
            tutorial[i].gameObject.SetActive(i == index);
        }
    }
}
