using CsvHelper;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;


public class SkillData
{
    public static readonly string FormatSkillPath = "Tower/{0}";

    public int ID {  get; set; }
    public string skillName {  get; set; }
    public int buffType {  get; set; }
    public int debuffType {  get; set; }
    public float value { get; set; }

}


public class SkillTable : DataTable
{
    private Dictionary<int, SkillData> skillTable = new Dictionary<int, SkillData>();

    public SkillData GetID(int id)
    {
        skillTable.TryGetValue(id,out var data);
        return data;
    }
    public List<SkillData> towerDatas
    {
        get
        {
            return skillTable.Values.ToList();
        }
    }
    public override void Load(string path)
    {
        string fullPath = string.Format(FormatPath, path);
        TextAsset data = Resources.Load<TextAsset>(fullPath);

        using (var reader = new StringReader(data.text))
        using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csvReader.GetRecords<SkillData>();
            foreach (var record in records)
            {
                skillTable.Add(record.ID, record);
            }
        }
    }

}
