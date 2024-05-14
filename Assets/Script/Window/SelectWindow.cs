using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectWindow : MonoBehaviour
{
    public void OnClickStage1()
    {
        SceneManager.LoadScene("1Level");
    }
}
