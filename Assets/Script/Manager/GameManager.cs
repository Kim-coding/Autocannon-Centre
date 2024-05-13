using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public UpgradeTower upgradeTower;

    void Awake()
    {
        Instance = this;
        upgradeTower = GetComponent<UpgradeTower>();
    }
}
