using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using static MonsterData;

public class MonsterSpawn : MonoBehaviour
{
    public List<MonsterData> monsterDatas = new List<MonsterData>();
    private Dictionary<int, Dictionary<int, WaveRule>> stageWaveRules = new Dictionary<int, Dictionary<int, WaveRule>>(); //���������� ���̺� ��Ģ

    private MonsterWaveTable monsterWaveTable;

    public Transform[] spawnPoints;
    public Transform[] wayPointContainers;
    public int currentStage;
    public int currentWave;

    private float spawnTime = 1f;
    private float spawnTimer;

    private int spawnCount = 0;
    private int spawnIndex = 0;
    private int subIndex = 0;

    private float waitTime = 15f;
    private float waitTimer;
    private bool isWaiting = true;

    private int id;

    private class SpawnRule           //���ͺ� ���� ��, �� ���̺꿡 �� ���� ���Ͱ� ���� �� �ֱ� ������ �迭�� ����
    {
        public int[] monsterNames;
        public int[] spawnCounts;
    }

    private class WaveRule             //���̺꺰 ���� ��Ģ
    {
        public SpawnRule[] spawnRules;
    }


    private void Start()
    {
        monsterWaveTable = DataTableMgr.Get<MonsterWaveTable>(DataTableIds.monsterWave);

        currentWave = GameManager.Instance.wave;
        currentStage = GameManager.Instance.stage;

        CreatedRules(currentStage);

        var monsterTable = DataTableMgr.Get<MonsterTable>(DataTableIds.monster);
        if (monsterTable != null)
        {
            var monster = monsterTable.monsterDatas;
            foreach(var m in monster)
            {
                monsterDatas.Add(m);
            }
        }

        MonsterPools();
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
                GameManager.Instance.UpdateWave(currentWave);
                GameManager.Instance.SetMonsterCount();
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
                    spawnCount = 0; // �迭�� ù��°�� ����ִ� ������ ���� Ƚ�� ����
                    subIndex++;

                    if(subIndex >= waveRules[currentWave].spawnRules[spawnIndex].monsterNames.Length)
                    {
                        subIndex = 0;
                        spawnIndex++;
                        if(spawnIndex >= waveRules[currentWave].spawnRules.Length)
                        {
                            spawnIndex = 0;
                            currentWave++; // �迭�� ��� ���� ���� �Ϸ� �� ���̺� ��ȯ
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

        MonsterData monsterData = null;
        
        foreach(var m in monsterDatas)
        {
            if(m.monsterName == monsterName)
            {
                monsterData = m;
                break;
            }
        }
        
        if(monsterData != null)
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
            MonsterData monsterData = monsterDatas.Find(m => m.monsterName == monsterName);
            if (monsterData != null)
            {
                GameObject prefab = Resources.Load<GameObject>(string.Format(MonsterData.FormatMonsterPath, monsterData.monsterName));
                PoolManager.instance.CreatePool(prefab, 1);
            }
        }
    }


    private void CreatedRules(int currentStage)   //���������� ���� ���� ��Ģ 
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
                //����Ʈ ü��� 
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
