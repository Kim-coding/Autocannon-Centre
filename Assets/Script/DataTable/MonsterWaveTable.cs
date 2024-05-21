using CsvHelper;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
    public int ID05 { get; set; }
    public int value05 { get; set; }
    public int ID06 { get; set; }
    public int value06 { get; set; }
    public int ID07 { get; set; }
    public int value07 { get; set; }
    public int ID08 { get; set; }
    public int value08 { get; set; }
}


public class MonsterWaveTable : DataTable
{
    private Dictionary<(int stage, int wave), MonsterWaveData> monsterWaveTable = new Dictionary<(int stage, int wave), MonsterWaveData>();
    public MonsterWaveData GetWaveData(int stage, int wave)
    {
        monsterWaveTable.TryGetValue((stage, wave), out var data);
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
                var key = (record.stage, record.wave);
                if (!monsterWaveTable.ContainsKey(key))
                {
                    monsterWaveTable.Add(key, record);
                }
            }
        }
    }
}
