using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class DataTableMgr
{
    private static Dictionary<string, DataTable> tables = new Dictionary<string, DataTable>();

    static DataTableMgr()
    {
        //foreach (var id in DataTableIds.String)
        //{
        //    DataTable table = CreateDataTable(id);
        //    table.Load(id);
        //    tables.Add(id, table);
        //}
        DataTable table = new TowerTable();
        table.Load("TowerTable");
        tables.Add("TowerTable", table);
    }

    //private static DataTable CreateDataTable(string id)
    //{
    //    switch (id)
    //    {
    //        case "TowerTable":
    //            return new TowerTable();
    //        case "MonsterTable":
    //            return new MonsterTable();
    //        default:
    //            throw new System.Exception("Unsupported table type");
    //    }
    //}

    public static T Get<T>(string id) where T : DataTable
    {
        if (!tables.ContainsKey(id))
            return null;
        return tables[id] as T;
    }
}
