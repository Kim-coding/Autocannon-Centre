using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TowerSpawner : MonoBehaviour
{
    private List<TowerData> towerDatas = new List<TowerData>();

    public int stage;

    private void Awake()
    {
        var towerTable = DataTableMgr.Get<TowerTable>(DataTableIds.tower);
        if (towerTable != null)
        {
            var data = towerTable.towerDatas;
            foreach (var t in data) 
            {
                if (stage <= t.stage && t.towerGrade == 1)
                {
                    towerDatas.Add(t);
                }
            }
        }
        
    }

    public void Spawn(Transform towerSpawnPoint)
    {
        Tile tile = towerSpawnPoint.GetComponent<Tile>();

        if (tile.isBuildTower)
        {
            return;
        }

        int totalWeight = towerDatas.Sum(t => t.percent);
        int randomNumber = Random.Range(0, totalWeight);
        int cumulative = 0;

        TowerData selectedTower = null;

        foreach (var tower in towerDatas)
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
            Instantiate(towerPrefab, towerSpawnPoint.position, Quaternion.identity);
            tile.isBuildTower = true;
        }
    }
}
