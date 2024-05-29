using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TowerSpawner : MonoBehaviour
{
    private Dictionary<int, TowerData> towerDatas;
    public Dictionary<int, TowerData> temporaryUpgrades;

    private TowerTable towerTable;
    public int stage;
    public AudioClip buildSound;
    private TowerCombiner towerCombiner;

    private void Start()
    {
        towerDatas = new Dictionary<int, TowerData>();
        temporaryUpgrades = new Dictionary<int, TowerData>();
        
        stage = GameManager.Instance.stage;
        towerCombiner = GetComponent<TowerCombiner>();
        towerTable = DataTableMgr.Get<TowerTable>(DataTableIds.tower);

        var data = towerTable.towerDatas.Where(t => t.stage <= stage && t.towerGrade == 1);
        foreach (var t in data)
        {
            towerDatas[t.ID] = t;
            temporaryUpgrades[t.ID] = new TowerData
            {
                ID = t.ID,
                percent = t.percent,
                towerSpeed = t.towerSpeed,
                damage = t.damage,
                percentIncr = 0,
                towerSpeedInc = 0,
                atkInc = 0,

            };
        }
    }

    public void UpgradeSpwanTower(int id, TowerData data)
    {
        if (towerDatas.ContainsKey(id))
        {
            if (!temporaryUpgrades.ContainsKey(id))
            {
                temporaryUpgrades[id] = new TowerData
                {
                    ID = id,
                    percent = towerDatas[id].percent,
                    towerSpeed = towerDatas[id].towerSpeed,
                    damage = towerDatas[id].damage
                };
            }

            temporaryUpgrades[id].percentIncr += data.percentIncr;
            temporaryUpgrades[id].towerSpeedInc += data.towerSpeedInc;
            temporaryUpgrades[id].atkInc += data.atkInc;

        }
        else
        {
            towerCombiner.UpgradeCombiTower(id, data);
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
        GameObject towerInstance = Instantiate(towerPrefab, towerSpawnPoint.position, Quaternion.identity, tile.transform);
        Tower towerScript = towerInstance.GetComponent<Tower>();
        if (towerScript != null)
        {
            towerScript.InitializeTower();

            if (temporaryUpgrades.ContainsKey(selectedTower.ID))
            {
                towerScript.UpgradeTower(temporaryUpgrades[selectedTower.ID]);
            }
        }
    }
    public void ResetAllTowers()
    {
        temporaryUpgrades = new Dictionary<int, TowerData>(towerDatas);
    }
}
