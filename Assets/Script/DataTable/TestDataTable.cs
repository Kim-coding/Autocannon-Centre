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
            foreach (var kvp in towerTable.towerTable)
            {
                TowerData tower = kvp.Value;
                // 타워 정보 로깅
                Logger.Log($"Tower ID: {tower.ID}, Name: {tower.name}, Type: {tower.type}, Damage: {tower.damage}, Attack Speed: {tower.atkSpeed}");
            }
        }
        else
        {
            Logger.Log("TowerTable is null or not loaded correctly.");
        }
    }
}
