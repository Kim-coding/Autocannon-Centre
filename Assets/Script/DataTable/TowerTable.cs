using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
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
    public string skillId { get; set; }
    public int stag {  get; set; }
    public int percent { get; set; }
    public int percentIncr { get; set; }
}

[System.Serializable]
public class TowerTable : DataTable
{
    public List<TowerData> towerTable = new List<TowerData>();

    public override void Load(string path)
    {
        TextAsset data = Resources.Load<TextAsset>(string.Format(FormatPath,path));
        string[] lines = data.text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] columns = lines[i].Split(',');
            try
            {
                TowerData tower = new TowerData
                {
                    ID = int.Parse(columns[0]),
                    name = columns[1],
                    towerGrade = int.Parse(columns[2]),
                    type = int.Parse(columns[3]),
                    damage = int.Parse(columns[4]),
                    atkSpeed = int.Parse(columns[5]),
                    atkRange = int.Parse(columns[6]),
                    maxTarget = int.Parse(columns[7]),
                    atkInc = int.Parse(columns[8]),
                    atkspeedInc = int.Parse(columns[9]),
                    skillId = columns[10],
                    stag = int.Parse(columns[11]),
                    percent = int.Parse(columns[12]),
                    percentIncr = int.Parse(columns[13]),
                };
                towerTable.Add(tower);
            }
            catch (FormatException ex)
            {
                Logger.LogError($"Error parsing data on line {i + 1}: {lines[i]} - {ex.Message}");
                throw;
            }
        }
    }
}