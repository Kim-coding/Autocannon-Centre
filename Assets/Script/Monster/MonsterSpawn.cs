using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MonsterData;

public class MonsterSpawn : MonoBehaviour
{
    public Transform spawnPoint;
    public int currentStage;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            CompareStage();
        }
    }

    private void CompareStage()
    {
        var monsterTable = DataTableMgr.Get<MonsterTable>(DataTableIds.monster);
        if (monsterTable != null)
        {
            foreach (var monster in monsterTable.monsterTable.Values)
            {
                if (monster.stage == currentStage)
                {
                    Spawn(monster);
                }
            }
        }
        
    }

    private void Spawn(MonsterData monsterData)
    {
        
    }
}
