using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeTower : MonoBehaviour
{
    public GameObject upgradeWindow;
    public GameObject slotPrefab;
    public Transform slotParent;
    private Dictionary<int, TowerData> towerDatas = new Dictionary<int, TowerData>();
    private TowerTable towerTable;
    public TowerSpawner towerSpawner;
    private TowerCombiner towerCombiner;
    public int stage;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GetComponent<GameManager>();
        towerCombiner = GetComponent<TowerCombiner>();
        stage = gameManager.stage;

        upgradeWindow.SetActive(false);
        
        towerTable = DataTableMgr.Get<TowerTable>(DataTableIds.tower);
        var data = towerTable.towerDatas.Where(t => t.stage <= stage && t.towerGrade == 1);
        foreach (var t in data)
        {
            towerDatas[t.ID] = t;
            GameObject slot = Instantiate(slotPrefab, slotParent);

            Slot slotScript = slot.GetComponent<Slot>();
            if (slotScript != null)
            {
                slotScript.SetData(t);
            }
        }
    }
    public void OnClickUpgrade()
    {
        AudioManager.Instance.SelectedSoundPlay();
        upgradeWindow.SetActive(!upgradeWindow.activeSelf);
    }

    public void TowerUpgrade(int towerId)
    {
        TowerData data = towerTable.GetID(towerId);

        if(data != null) 
        {
            if(towerSpawner.spawnedTowers.ContainsKey(towerId))
            {
                foreach (var tower in towerSpawner.spawnedTowers[towerId])
                {
                    if (tower.id == towerId)
                    {
                        tower.UpgradeTower(data);
                        towerCombiner.UpdateTowerInfo(tower);
                    }
                }
            }
            towerSpawner.UpgradeSpwanTower(towerId, data);
        }
    }
}