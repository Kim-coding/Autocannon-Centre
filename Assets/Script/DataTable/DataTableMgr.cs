using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MonsterData;

public static class DataTableMgr
{
    private static Dictionary<string, DataTable> tables = new Dictionary<string, DataTable>();

    static DataTableMgr()
    {
        foreach (var id in DataTableIds.String)
        {
            DataTable table = CreateDataTable(id);
            table.Load(id);
            tables.Add(id, table);
        }
    }

    private static DataTable CreateDataTable(string id)
    {
        var tableTypes = new Dictionary<string, Func<DataTable>>
        {
            { "TowerTable", () => new TowerTable() },
            { "MonsterTable", () => new MonsterTable() },
            //{ "MonsterWaveTable", () => new MonsterWaveTable() },
            { "SkillTable", () => new SkillTable() }
        };

        if (tableTypes.ContainsKey(id))
        {
            return tableTypes[id]();
        }

        throw new Exception("Unsupported table type");
    }

    public static T Get<T>(string id) where T : DataTable
    {
        if (!tables.ContainsKey(id))
            return null;
        return tables[id] as T;
    }
}
