using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TowerSpawner : MonoBehaviour
{
    private Dictionary<int, TowerData> towerDatas = new Dictionary<int, TowerData>();
    private TowerTable towerTable;
    public int stage;
    public AudioClip buildSound;

    private void Start()
    {
        stage = GameManager.Instance.stage;
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
        AudioManager.Instance.EffectPlay(buildSound);
        Tile tile = towerSpawnPoint.GetComponent<Tile>();

        if (tile.isBuildTower)
        {
            return;
        }

        TowerData selectedTower = selectRandomTower();

        if(selectedTower != null) 
        {
            InstantiateTower(selectedTower, towerSpawnPoint, tile);
            tile.isBuildTower = true;
        }
    }

    private TowerData selectRandomTower()
    {
        int totalWeight = towerDatas.Values.Sum(t => t.percent);
        
        int randomNumber = Random.Range(0, totalWeight);
        int cumulative = 0;

        foreach (var tower in towerDatas.Values)
        {
            cumulative += tower.percent;
            if (randomNumber < cumulative)
            {
                return tower;
            }
        }

        return null;
    }

    private void InstantiateTower(TowerData selectedTower, Transform towerSpawnPoint, Tile tile)
    {
        var towerName = selectedTower.ID.ToString();
        var towerPrefab = Resources.Load<GameObject>(string.Format(TowerData.FormatTowerPath, towerName));
        Instantiate(towerPrefab, towerSpawnPoint.position, Quaternion.identity, tile.transform);
    }
}
