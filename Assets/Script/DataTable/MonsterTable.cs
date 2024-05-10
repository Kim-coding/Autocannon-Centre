using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;
using static MonsterData;

public class MonsterData
{
    public static readonly string FormatMonsterPath = "Monster/{0}";

    public int ID { get; set; }
    public int monsterName { get; set; }
    public int stage { get; set; }
    public int monsterType { get; set; }
    public int monsterHP { get; set; }
    public int monsterSpeed { get; set; }
    public int monsterGold { get; set; }
    public float scale { get; set; }
}
public class MonsterTable : DataTable
{
    private Dictionary<int, MonsterData> monsterTable = new Dictionary<int, MonsterData>();

    public List<int> monsterIds
    {
        get
        {
            return monsterTable.Keys.ToList();
        }
    }

    public List<MonsterData> monsterDatas
    {
        get
        {
            return monsterTable.Values.ToList();
        }
    }

    public MonsterData GetID(int id)
    {
        monsterTable.TryGetValue(id, out var data);
        return data;
    }

    public override void Load(string path)
    {
        string fullPath = string.Format(FormatPath, path);
        TextAsset data = Resources.Load<TextAsset>(fullPath);

        using (var reader = new StringReader(data.text))
        using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csvReader.GetRecords<MonsterData>();
            foreach (var record in records)
            {
                monsterTable.Add(record.ID, record);
            }
        }
    }
}