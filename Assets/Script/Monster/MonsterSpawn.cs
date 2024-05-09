using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static MonsterData;


// 현재 상황 : 해당 스테이지에 나오는 몬스터를 1초에 한 번 씩 생성
// 추가 해야하는 사항 : 

public class MonsterSpawn : MonoBehaviour
{
    public List<MonsterData> monsterDatas = new List<MonsterData>();

    public Transform spawnPoint;
    public int currentStage;

    //public int wave = 1;

    private float spawnTime = 1f;
    private float spawnTimer;
    private int currentMonsterIndex = 0;

    private int spawnedCount = 0;
    private int maxCount = 20;

    private float waitTime = 10f;
    private float waitTimer;
    private bool isWaiting = false;

    private void Start()
    {
        CompareStage();
    }

    private void Update()
    {
        if(isWaiting)
        {
            waitTimer += Time.deltaTime;
            if(waitTimer > waitTime)
            {
                isWaiting = false;
                waitTimer = 0f;
                currentMonsterIndex++;
            }
            return;
        }

        if (monsterDatas.Count > 0 && currentMonsterIndex < monsterDatas.Count)
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnTime)
            {
                spawnTimer = 0;
                Spawn(monsterDatas[currentMonsterIndex]);
                isWaiting = true;
            }
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
                    monsterDatas.Add(monster);
                }
            }
        }        
    }

    private void Spawn(MonsterData monsterData)
    {
        if(monsterDatas != null) 
        {
            GameObject prefab = Resources.Load<GameObject>(string.Format(MonsterData.FormatMonsterPath, monsterData.monsterName));
            if (prefab != null)
            {
                GameObject monster = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
            }
            else
            {
                Debug.LogError("Failed to load prefab from path: " + FormatMonsterPath);
            }
        }
    }
    
}
