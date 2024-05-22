using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using EPOOutline;
using System.Linq;

public class TowerCombiner : MonoBehaviour
{
    public AudioClip bulidSound;

    public TextMeshProUGUI towerName;
    public TextMeshProUGUI towerdamage;
    public TextMeshProUGUI towerAtkSpeed;
    public TextMeshProUGUI towerRange;

    public GameObject combinationSlot1;
    public GameObject combinationSlot2;
    public GameObject combinationSlot3;

    private Tower selectedTower;
    private Tower combinationTower1;
    private Tower combinationTower2;
    private Tower combinationTower3;

    private Outlinable currentOutline;

    private TowerTable towerTable;
    private TowerSpawner towerSpawner;

    private void Awake()
    {
        towerTable = DataTableMgr.Get<TowerTable>(DataTableIds.tower);
        towerSpawner = GetComponent<TowerSpawner>();
    }

    private void Update()
    {
        if(selectedTower != null) 
        {
            OnInfo(selectedTower);
        }
    }

    public void ClearSelection()
    {
        if(currentOutline != null)
        {
            currentOutline.enabled = false;
        }

        selectedTower = null;
    }

    public void OnInfo(Tower tower)
    {
        selectedTower = tower;
        towerName.text = $"name : {tower.towerName.Replace("(Clone)", "")}";
        towerdamage.text = $"damage : {tower.damage.ToString()}";
        towerAtkSpeed.text = $"Speed : {tower.fireRate.ToString()}";
        towerRange.text = $"range : {tower.range.ToString()}";
    }

    public void CombinationSlot1()
    {
        if (combinationTower1 != null)
        {
            ClearSlot1();
        }
        else if (selectedTower != null && !IsTowerInSlots(selectedTower.TowerID)) 
        {
            combinationTower1 = selectedTower;
            combinationSlot1.GetComponentInChildren<TextMeshProUGUI>().text = selectedTower.towerName;
        }
    }

    public void CombinationSlot2()
    {
        if (combinationTower2 != null)
        {
            ClearSlot2();
        }
        else if(selectedTower != null && !IsTowerInSlots(selectedTower.TowerID))
        {
            combinationTower2 = selectedTower;
            combinationSlot2.GetComponentInChildren<TextMeshProUGUI>().text = selectedTower.towerName;
        }
    }

    public void CombinationSlot3()
    {
        if (combinationTower3 != null)
        {
            ClearSlot3();
        }
        else if(selectedTower != null && !IsTowerInSlots(selectedTower.TowerID))
        {
            combinationTower3 = selectedTower;
            combinationSlot3.GetComponentInChildren<TextMeshProUGUI>().text = selectedTower.towerName;
        }
    }

    public void OnClickButton()
    {
        if (combinationTower1 != null && combinationTower2 != null && combinationTower3 != null)
        {
            Tile tile1 = combinationTower1.GetComponentInParent<Tile>();
            Tile tile2 = combinationTower2.GetComponentInParent<Tile>();
            Tile tile3 = combinationTower3.GetComponentInParent<Tile>();

            if (combinationTower1.id ==  combinationTower2.id && combinationTower2.id == combinationTower3.id)
            {
                AudioManager.Instance.EffectPlay(bulidSound);
                int newTowerID = combinationTower1.id + 100;
                TowerData newTowerData = towerTable.GetID(newTowerID);
                if (newTowerData != null)
                {
                    tile2.RemoveCurrentTower();
                    tile3.RemoveCurrentTower();
                    SpawnNewTower(newTowerData, combinationTower1.transform.position, tile1);
                    ReSetSlot();
                }
            }
            else
            {
                Debug.Log("잘못된 조합입니다");
            }
        }
    }

    public void OnClickRandomButton()
    {
        if (combinationTower1 != null && combinationTower2 != null && combinationTower3 != null)
        {
            Tile tile1 = combinationTower1.GetComponentInParent<Tile>();
            Tile tile2 = combinationTower2.GetComponentInParent<Tile>();
            Tile tile3 = combinationTower3.GetComponentInParent<Tile>();

            if (combinationTower1.towerGrade == combinationTower2.towerGrade && combinationTower2.towerGrade == combinationTower3.towerGrade)
            {
                AudioManager.Instance.EffectPlay(bulidSound);
                List<TowerData> Towers = towerTable.towerDatas
                        .FindAll(t => t.stage <= towerSpawner.stage && t.towerGrade == combinationTower1.towerGrade + 1);
                if (Towers.Count > 0)
                {
                    TowerData newTowerData = SelectRandomTower(Towers);
                    tile2.RemoveCurrentTower();
                    tile3.RemoveCurrentTower();
                    SpawnNewTower(newTowerData, combinationTower1.transform.position, tile1);
                    ReSetSlot();
                }
            }
            else
            {
                Debug.Log("잘못된 조합입니다");
            }
        }
    }

    private TowerData SelectRandomTower(List<TowerData> possibleTowers)
    {
        int total = possibleTowers.Sum(t => t.percent);
        int random = Random.Range(0, total);
        int cumulative = 0;

        foreach (var tower in possibleTowers)
        {
            cumulative += tower.percent;
            if (random < cumulative)
            {
                return tower;
            }
        }

        return null;
    }

    private void ReSetSlot()
    {
        Destroy(combinationTower1.gameObject);
        Destroy(combinationTower2.gameObject);
        Destroy(combinationTower3.gameObject);

        ClearSlot1();
        ClearSlot2();
        ClearSlot3();
    }

    private void SpawnNewTower(TowerData newTowerData, Vector3 position, Tile tile1)
    {
        // 새로운 타워 생성
        var towerName = newTowerData.ID.ToString();
        var towerPrefab = Resources.Load<GameObject>(string.Format(TowerData.FormatTowerPath, towerName));
        if (towerPrefab != null)
        {
            GameObject newTower = Instantiate(towerPrefab, position, Quaternion.identity, tile1.transform);
            Tile tile = newTower.GetComponentInParent<Tile>();
            if (tile != null)
            {
                tile.BuildTower(newTower.GetComponent<Tower>());
            }
        }
        else
        {
            Debug.LogError($"Tower prefab not found for {towerName}");
        }
    }

    private bool IsTowerInSlots(string towerID)
    {
        return (combinationTower1 != null && combinationTower1.TowerID == towerID) ||
               (combinationTower2 != null && combinationTower2.TowerID == towerID) ||
               (combinationTower3 != null && combinationTower3.TowerID == towerID);
    }

    private void ClearSlot1()
    {
        combinationTower1 = null;
        combinationSlot1.GetComponentInChildren<TextMeshProUGUI>().text = "Empty";
    }

    private void ClearSlot2()
    {
        combinationTower2 = null;
        combinationSlot2.GetComponentInChildren<TextMeshProUGUI>().text = "Empty";
    }

    private void ClearSlot3()
    {
        combinationTower3 = null;
        combinationSlot3.GetComponentInChildren<TextMeshProUGUI>().text = "Empty";
    }
}
