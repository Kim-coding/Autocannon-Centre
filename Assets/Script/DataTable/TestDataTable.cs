using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MonsterData;

public class TestDataTable : MonoBehaviour
{
    //void Start()
    //{
    //    var towerTable = DataTableMgr.Get<TowerTable>(DataTableIds.tower);
    //    var monsterTable = DataTableMgr.Get<MonsterTable>(DataTableIds.monster);

    //    if (towerTable != null)
    //    {
    //        foreach (var kvp in towerTable.towerTable)
    //        {
    //            TowerData tower = kvp.Value;
                
    //            Debug.Log($"Tower ID: {tower.ID}, Name: {tower.name}, Type: {tower.type}, Damage: {tower.damage}, Attack Speed: {tower.atkSpeed}");
    //        }
    //    }
    //    else
    //    {
    //        Debug.Log("TowerTable is null or not loaded correctly.");
    //    }

    //    if( monsterTable != null ) 
    //    {
    //        foreach (var kvp in monsterTable.monsterTable)
    //        {
    //            MonsterData tower = kvp.Value;

    //            Debug.Log($"Monster ID: {tower.ID}, Monster Name: {tower.monsterName}, Monster Type: {tower.monsterType}, Monster HP: {tower.monsterHP}, Monster Speed: {tower.monsterSpeed}");
    //        }
    //    }
    //}
}
