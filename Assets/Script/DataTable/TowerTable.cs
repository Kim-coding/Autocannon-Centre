using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

[System.Serializable]
public class TowerData
{
    public static readonly string FormatTowerPath = "{0}";  //타워 프리팹 위치

    public int ID {  get; set; }
    public string name { get; set; }
    public int towerGrade { get; set; }
    public int type { get; set; }
    public int damage { get; set; }
    public int atkSpeed { get; set; }
    public int atkRange { get; set; }
    public int maxTarget { get; set; }
    public int atkInc { get; set; }
    public int atkspeedInc { get; set; }
    public string skillID { get; set; }
    public int stage {  get; set; }
    public int percent { get; set; }
    public int percentIncr { get; set; }
}

[System.Serializable]
public class TowerTable : DataTable
{
    public Dictionary<int, TowerData> towerTable = new Dictionary<int, TowerData>();
    
    public TowerData GetID(int id)
    {
        towerTable.TryGetValue(id,out var data);
        return data;
    }

    public override void Load(string path)
    {
        string fullPath = string.Format(FormatPath, path);
        TextAsset data = Resources.Load<TextAsset>(fullPath);

        using(var reader = new StringReader(data.text))
        using(var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csvReader.GetRecords<TowerData>();
            foreach (var record in records) 
            {
                towerTable.Add(record.ID, record);
            }
        }
    }
}