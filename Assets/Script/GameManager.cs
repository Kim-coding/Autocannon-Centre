using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance
    {
        get
        {
            if(instance == null)
            {
                m_Instance = FindAnyObjectByType<GameManager>();
            }
            return m_Instance;
        }
        
    }

    private static GameManager m_Instance;

    public void OptionButton()
    {

    }

    public void GameStartButton()
    {

    }
}
