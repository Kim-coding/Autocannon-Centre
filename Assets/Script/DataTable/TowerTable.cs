using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class TowerData
{
    public static readonly string FormatTowerPath = "Tower/{0}";  //타워 프리팹 위치
    public static readonly string FormatTowerIconsPath = "TowerIcons/{0}";  //타워 아이콘 위치

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
    public int skillID { get; set; }
    public int stage {  get; set; }
    public int percent { get; set; }
    public int percentIncr { get; set; }
    public float towerSpeed { get; set; }
    public float towerSpeedInc {  get; set; }
    public string towerIcon { get; set; }
}

[System.Serializable]
public class TowerTable : DataTable
{
    private Dictionary<int, TowerData> towerTable = new Dictionary<int, TowerData>();
    
    public TowerData GetID(int id)
    {
        towerTable.TryGetValue(id,out var data);
        return data;
    }
    public List<TowerData> towerDatas
    {
        get
        {
            return towerTable.Values.ToList();
        }
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