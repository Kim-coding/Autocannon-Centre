using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDataTable : MonoBehaviour
{
    void Start()
    {
        var towerTable = DataTableMgr.Get<TowerTable>("TowerTable");

        if (towerTable != null)
        {
            foreach (var tower in towerTable.towerTable)
            {
                Debug.Log($"Id: {tower.ID}, Name: {tower.name}, Type: {tower.type}, Damage: {tower.damage}, Attack Speed: {tower.atkSpeed}");
            }
        }
        else
        {
            Logger.Log("TowerTable is null or not loaded correctly.");
        }
    }
}
