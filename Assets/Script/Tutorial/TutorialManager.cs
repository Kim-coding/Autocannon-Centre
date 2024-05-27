using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject plane;

    public GameObject tutorial1;
    public GameObject tutorial2;
    public GameObject tutorial3;
    public GameObject tutorial4;


    private void Awake()
    {
        Time.timeScale = 0f;
    }
}
