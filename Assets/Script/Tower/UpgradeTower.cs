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
    public int stage;

    void Awake()
    {
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
    public void OnUpgradClick()
    {
        upgradeWindow.SetActive(!upgradeWindow.activeSelf);
    }

    public void TowerUpgrade(int towerId)
    {
        TowerData data = towerTable.GetID(towerId);

        if(data != null) 
        {
            data.atkSpeed += data.atkspeedInc;
            data.damage += data.atkInc;

            Tower[] towers = FindObjectsOfType<Tower>();
            foreach (var tower in towers)
            {
                if (tower.name.Replace("(Clone)", "") == towerId.ToString())
                {
                    tower.UpgradeTower(data);
                }
            }
            towerSpawner.UpgradeTowerPercent(towerId, data.percentIncr);
        }
    }
}