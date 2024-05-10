using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static MonsterData;

public class MonsterSpawn : MonoBehaviour
{
    public List<MonsterData> monsterDatas = new List<MonsterData>();
    private Dictionary<int, Dictionary<int, WaveRule>> stageWaveRules = new Dictionary<int, Dictionary<int, WaveRule>>(); //���������� ���̺� ��Ģ

    public Transform spawnPoint;
    public int currentStage;
    public int currentWave = 1;

    private float spawnTime = 1f;
    private float spawnTimer;

    private int spawnCount = 0;
    private int spawnIndex = 0;
    private int subIndex = 0;

    private float waitTime = 10f;
    private float waitTimer;
    private bool isWaiting = false;

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
        CreatedRules();
        var monsterTable = DataTableMgr.Get<MonsterTable>(DataTableIds.monster);
        if (monsterTable != null)
        {
            foreach (var m in monsterTable.monsterTable.Values)
            {
                monsterDatas.Add(m);
            }
        }
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
            }
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

                    //if (subIndex >= waveRules[currentWave].spawnRules[spawnIndex].monsterNames.Length &&
                    //    spawnIndex >= waveRules[currentWave].spawnRules.Length)
                    //{
                    //    spawnIndex = 0;
                    //    subIndex = 0;
                    //    currentWave++; // �迭�� ��� ���� ���� �Ϸ� �� ���̺� ��ȯ
                    //    isWaiting = true;
                    //}
                }

               
            }
        }

    }

    private void Spawn(int wave)
    {
        var rule = stageWaveRules[currentStage][wave].spawnRules[spawnIndex];
        var monsterName = rule.monsterNames[subIndex];  //���� ���̺��� ����
        Debug.Log(monsterName);
        MonsterData monsterData = null;
        
        foreach(var m in monsterDatas)
        {
            if(m.monsterName == monsterName)
            {
                monsterData = m;
                break;
            }
        }
        Debug.Log(monsterData);
        if(monsterData != null)
        {
            GameObject prefab = Resources.Load<GameObject>(string.Format(MonsterData.FormatMonsterPath, monsterName));
            GameObject moster = Instantiate(prefab, spawnPoint.position,Quaternion.identity);
            moster.gameObject.transform.localScale = new Vector3(monsterData.scale, monsterData.scale, monsterData.scale);
        }
        
    }
    

    private void CreatedRules()   //���������� ���� ���� ��Ģ 
    {
        //stage 1
        stageWaveRules.Add(1, new Dictionary<int, WaveRule>());
        AddRule(1, 1, new int[] { 101 }, new int[]{ 20 });
        AddRule(1, 2, new int[] { 101 }, new int[]{ 20 });
        AddRule(1, 3, new int[] { 102 }, new int[]{ 20 });
        AddRule(1, 4, new int[] { 102 }, new int[]{ 20 });
        AddRule(1, 5, new int[] { 101, 111 }, new int[]{ 16, 4 });
        AddRule(1, 6, new int[] { 102 }, new int[]{ 20 });
        AddRule(1, 7, new int[] { 102 }, new int[]{ 20 });
        AddRule(1, 8, new int[] { 103 }, new int[]{ 20 });
        AddRule(1, 9, new int[] { 103 }, new int[]{ 20 });
        AddRule(1, 10, new int[] { 102, 111 }, new int[]{ 16, 4 });
        AddRule(1, 11, new int[] { 103 }, new int[]{ 20 });
        AddRule(1, 12, new int[] { 103 }, new int[]{ 20 });
        AddRule(1, 13, new int[] { 104 }, new int[]{ 20 });
        AddRule(1, 14, new int[] { 104 }, new int[]{ 20 });
        AddRule(1, 15, new int[] { 103, 111 }, new int[]{ 16, 4 });
        AddRule(1, 16, new int[] { 104 }, new int[]{ 20 });
        AddRule(1, 17, new int[] { 104 }, new int[]{ 20 });
        AddRule(1, 18, new int[] { 105 }, new int[]{ 20 });
        AddRule(1, 19, new int[] { 105 }, new int[]{ 20 });
        AddRule(1, 20, new int[] { 104, 121 }, new int[]{ 19, 1 });

        //stage 2
        stageWaveRules.Add(2, new Dictionary<int, WaveRule>());
        AddRule(2, 1, new int[] { 201 }, new int[] { 20 });
        AddRule(2, 2, new int[] { 201 }, new int[] { 20 });
        AddRule(2, 3, new int[] { 202 }, new int[] { 20 });
        AddRule(2, 4, new int[] { 202 }, new int[] { 20 });
        AddRule(2, 5, new int[] { 201, 211 }, new int[] { 16, 4 });
        AddRule(2, 6, new int[] { 202 }, new int[] { 20 });
        AddRule(2, 7, new int[] { 202 }, new int[] { 20 });
        AddRule(2, 8, new int[] { 203 }, new int[] { 20 });
        AddRule(2, 9, new int[] { 203 }, new int[] { 20 });
        AddRule(2, 10, new int[] { 202, 211 }, new int[] { 16, 4 });
        AddRule(2, 11, new int[] { 203 }, new int[] { 20 });
        AddRule(2, 12, new int[] { 203 }, new int[] { 20 });
        AddRule(2, 13, new int[] { 204 }, new int[] { 20 });
        AddRule(2, 14, new int[] { 204 }, new int[] { 20 });
        AddRule(2, 15, new int[] { 203, 211 }, new int[] { 16, 4 });
        AddRule(2, 16, new int[] { 204 }, new int[] { 20 });
        AddRule(2, 17, new int[] { 204 }, new int[] { 20 });
        AddRule(2, 18, new int[] { 205 }, new int[] { 20 });
        AddRule(2, 19, new int[] { 205 }, new int[] { 20 });
        AddRule(2, 20, new int[] { 204, 221 }, new int[] { 19, 1 });
    }

    private void AddRule(int stage, int wave, int[] monsterName, int[] monsterCount)
    {
        stageWaveRules[stage].Add(wave, new WaveRule { spawnRules = new SpawnRule[] { new SpawnRule { monsterNames = monsterName, spawnCounts = monsterCount }}});
        //�������� ��Ģ�� 
    }
}
