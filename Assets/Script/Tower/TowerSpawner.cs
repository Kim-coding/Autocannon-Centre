using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TowerSpawner : MonoBehaviour
{
    private Dictionary<int, TowerData> towerDatas = new Dictionary<int, TowerData>();
    private TowerTable towerTable;
    public int stage;

    private void Awake()
    {
        towerTable = DataTableMgr.Get<TowerTable>(DataTableIds.tower);
        var data = towerTable.towerDatas.Where(t => t.stage <= stage && t.towerGrade == 1);

        foreach (var t in data)
        {
            towerDatas[t.ID] = t;
        }
    }

    public void UpgradeTowerPercent(int id, int percentInc)
    {
        if(towerDatas.ContainsKey(id)) 
        {
            towerDatas[id].percent += percentInc;
        }
    }

    public void Spawn(Transform towerSpawnPoint)
    {
        Tile tile = towerSpawnPoint.GetComponent<Tile>();

        if (tile.isBuildTower)
        {
            return;
        }

        int totalWeight = towerDatas.Values.Sum(t => t.percent);
        int randomNumber = Random.Range(0, totalWeight);
        int cumulative = 0;

        TowerData selectedTower = null;

        foreach (var tower in towerDatas.Values)
        {
            cumulative += tower.percent;
            if (randomNumber < cumulative)
            {
                selectedTower = tower;
                break;
            }
        }

        if(selectedTower != null) 
        {
            var towerName = selectedTower.ID.ToString();
            var towerPrefab = Resources.Load<GameObject>(string.Format(TowerData.FormatTowerPath, towerName));
            Instantiate(towerPrefab, towerSpawnPoint.position, Quaternion.identity,transform);
            tile.isBuildTower = true;
        }
    }
}
