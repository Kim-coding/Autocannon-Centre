using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using static MonsterData;

public class MonsterSpawn : MonoBehaviour
{
    private Dictionary<int, MonsterData> monsterDatas = new Dictionary<int, MonsterData>();
    private Dictionary<int, Dictionary<int, WaveRule>> stageWaveRules = new Dictionary<int, Dictionary<int, WaveRule>>(); //스테이지별 웨이브 규칙

    private MonsterWaveTable monsterWaveTable;

    public Transform[] spawnPoints;
    public Transform[] wayPointContainers;
    private int currentStage;
    private int currentWave;

    private float spawnTime = 1f;
    private float spawnTimer;

    private int spawnCount = 0;
    private int spawnIndex = 0;
    private int subIndex = 0;

    private float waitTime = 15f;
    private float waitTimer;
    private bool isWaiting = true;

    private bool isSuccess = false;
    private GameManager gameManager;
    private class SpawnRule           //몬스터별 생성 수, 한 웨이브에 두 가지 몬스터가 나올 수 있기 때문에 배열로 받음
    {
        public int[] monsterNames;
        public int[] spawnCounts;
    }

    private class WaveRule             //웨이브별 생성 규칙
    {
        public SpawnRule[] spawnRules;
    }


    private void Start()
    {
        GameObject gameManagerObject = GameObject.FindWithTag("GameController");
        if (gameManagerObject != null)
        {
            gameManager = gameManagerObject.GetComponent<GameManager>();
        }

        monsterWaveTable = DataTableMgr.Get<MonsterWaveTable>(DataTableIds.monsterWave);

        currentWave = gameManager.wave;
        currentStage = gameManager.stage;

        CreatedRules(currentStage);

        var monsterTable = DataTableMgr.Get<MonsterTable>(DataTableIds.monster);
        if (monsterTable != null)
        {
            monsterDatas = monsterTable.monsterDatas.ToDictionary(m => m.monsterName);
        }

        MonsterPools();
    }

    private void Update()
    {
        if(currentWave > 20 && gameManager.GetMonsterCount() <= 0 && !isSuccess)
        {
            gameManager.Success();
            gameManager.StageClear();
            isSuccess = true;
            return;
        }

        if(isWaiting)
        {
            waitTimer += Time.deltaTime;
            if(waitTimer > waitTime)
            {
                isWaiting = false;
                waitTimer = 0f;
                gameManager.UpdateWave(currentWave);
                gameManager.SetMonsterCount();
            }
            return;
        }

        if (!stageWaveRules.ContainsKey(currentStage))
        {
            return;
        }

        var waveRules = stageWaveRules[currentStage];
        if(waveRules.ContainsKey(currentWave))
        {
            spawnTimer += Time.deltaTime;
            if(spawnTimer > spawnTime)
            {
                spawnTimer = 0;
                Spawn(currentWave);

                spawnCount++;
                if(spawnCount >= waveRules[currentWave].spawnRules[spawnIndex].spawnCounts[subIndex])
                {
                    spawnCount = 0; // 배열의 첫번째에 담겨있는 몬스터의 생성 횟수 도달
                    subIndex++;

                    if(subIndex >= waveRules[currentWave].spawnRules[spawnIndex].monsterNames.Length)
                    {
                        subIndex = 0;
                        spawnIndex++;
                        if(spawnIndex >= waveRules[currentWave].spawnRules.Length)
                        {
                            spawnIndex = 0;
                            currentWave++; // 배열의 모든 몬스터 생성 완료 시 웨이브 전환
                            isWaiting = true;
                        }
                    }
                }
            }
        }

    }

    private void Spawn(int wave)
    {
        var rule = stageWaveRules[currentStage][wave].spawnRules[spawnIndex];
        var monsterName = rule.monsterNames[subIndex];

        if(monsterDatas.TryGetValue(monsterName, out var monsterData))
        {
            GameObject monster = PoolManager.instance.GetObjectPool(monsterName.ToString());
            var index = Random.Range(0, spawnPoints.Length - 1);
            monster.transform.position = spawnPoints[index].position;
            monster.transform.rotation = Quaternion.identity;
            monster.transform.localScale = new Vector3(monsterData.scale, monsterData.scale, monsterData.scale);

            MonsterMove monsterMove = monster.GetComponent<MonsterMove>();
            if (monsterMove != null && wayPointContainers.Length >= index)
            {
                foreach (Transform wayPoint in wayPointContainers)
                {
                    monsterMove.SetWayPoints(wayPointContainers[index]);
                }
            }
        }
        
    }


    private void MonsterPools()
    {
        HashSet<int> requiredMonsters = new HashSet<int>();

        if (stageWaveRules.ContainsKey(currentStage))
        {
            var waveRules = stageWaveRules[currentStage];
            foreach (var waveRule in waveRules.Values)
            {
                foreach (var spawnRule in waveRule.spawnRules)
                {
                    foreach (var monsterName in spawnRule.monsterNames)
                    {
                        requiredMonsters.Add(monsterName);
                    }
                }
            }
        }

        foreach (var monsterName in requiredMonsters)
        {
            if (monsterDatas.TryGetValue(monsterName, out var monsterData))
            {
                GameObject prefab = Resources.Load<GameObject>(string.Format(MonsterData.FormatMonsterPath, monsterData.monsterName));
                PoolManager.instance.CreatePool(prefab, 1);
            }
        }
    }


    private void CreatedRules(int currentStage)   //스테이지별 몬스터 생성 규칙 
    {
        stageWaveRules.Add(currentStage, new Dictionary<int, WaveRule>());

        for (int wave = 1; ; wave++)
        {
            var waveData = monsterWaveTable.GetWaveData(currentStage, wave);
            if (waveData == null)
            {
                break;
            }
            List<int> monsterName = new List<int>();
            List<int> monsterCount = new List<int>();

            for(int i = 1; i <= 8; i++)
            {
                //리스트 체우기 
                int monsterID = (int)waveData.GetType().GetProperty($"ID{i:00}").GetValue(waveData);
                int count = (int)waveData.GetType().GetProperty($"value{i:00}").GetValue(waveData);
                if(monsterID > 0 &&  count > 0)
                {
                    monsterName.Add(monsterID);
                    monsterCount.Add(count);
                }
            }
            AddRule(currentStage, waveData.wave, monsterName.ToArray(), monsterCount.ToArray());
        }
    }
    private void AddRule(int stage, int wave, int[] monsterName, int[] monsterCount)
    {
        stageWaveRules[stage].Add(wave, new WaveRule { spawnRules = new SpawnRule[] { new SpawnRule { monsterNames = monsterName, spawnCounts = monsterCount }}});
    }
}
