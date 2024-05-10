using CsvHelper;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class MonsterWaveData
{
    public int stage { get; set; }
    public int wave { get; set; }
    public int ID01 { get; set; }
    public int value01 { get; set; }
    public int ID02 { get; set; }
    public int value02 { get; set; }
    public int ID03 { get; set; }
    public int value03 { get; set; }
    public int ID04 { get; set; }
    public int value04 { get; set; }
}


public class MonsterWaveTable : DataTable
{
    private Dictionary<int, MonsterWaveData> monsterWaveTable = new Dictionary<int, MonsterWaveData>();
    public MonsterWaveData GetID(int id)
    {
        monsterWaveTable.TryGetValue(id, out var data);
        return data;
    }

    public override void Load(string path)
    {
        string fullPath = string.Format(FormatPath, path);
        TextAsset data = Resources.Load<TextAsset>(fullPath);

        using (var reader = new StringReader(data.text))
        using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csvReader.GetRecords<MonsterWaveData>();
            foreach (var record in records)
            {
                monsterWaveTable.Add(record.stage, record);
            }
        }
    }
}
