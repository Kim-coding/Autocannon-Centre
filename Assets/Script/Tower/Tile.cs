using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool isBuildTower { get; set; }
    private Tower currentTower;
    private void Awake()
    {
        isBuildTower = false;
    }
    public void BuildTower(Tower tower)
    {
        isBuildTower = true;
        currentTower = tower;
    }

    public Tower GetCurrentTower()
    {
        return currentTower;
    }
}
