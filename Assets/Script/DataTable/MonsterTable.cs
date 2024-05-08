using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterData
{
    public int ID { get; set; }
    public int monsterName { get; set; }
    public int stage { get; set; }
    public int monsterType { get; set; }
    public int monsterHP { get; set; }
    public int monsterSpeed { get; set; }
    public int monsterGold { get; set; }


    public class MonsterTable : DataTable
    {
        public List<TowerData> monsterTable = new List<TowerData>();

        public override void Load(string path)
        {
            Debug.Log(path);

            TextAsset data = Resources.Load<TextAsset>(string.Format(FormatPath, path));
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

                    };
                    monsterTable.Add(tower);
                }
                catch (FormatException ex)
                {
                    Logger.LogError($"Error parsing data on line {i + 1}: {lines[i]} - {ex.Message}");
                    throw;
                }
            }

        }
    }
}
