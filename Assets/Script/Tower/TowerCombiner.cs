using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using EPOOutline;
using System.Linq;
using UnityEngine.SceneManagement;

public class TowerCombiner : MonoBehaviour
{
    private Dictionary<int, TowerData> towerDatas2 = new Dictionary<int, TowerData>();
    private Dictionary<int, TowerData> towerDatas3 = new Dictionary<int, TowerData>();

    public AudioClip bulidSound;
    public AudioClip selectedSound;

    public Button cancelButton;

    public Image towerIcon;

    public TextMeshProUGUI towerName;
    public TextMeshProUGUI towerdamage;
    public TextMeshProUGUI towerAtkSpeed;

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

        var data2 = towerTable.towerDatas.Where(t => t.stage <= towerSpawner.stage && t.towerGrade == 2);
        foreach (var t in data2)
        {
            towerDatas2[t.ID] = t;
        }

        var data3 = towerTable.towerDatas.Where(t => t.stage <= towerSpawner.stage && t.towerGrade == 3);
        foreach (var t in data3)
        {
            towerDatas3[t.ID] = t;
        }
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
        if (currentOutline != null)
        {
            currentOutline.enabled = false;
        }

        towerIcon.sprite = Resources.Load<Sprite>(string.Format(TowerData.FormatTowerIconsPath, "Default"));
        towerName.text = $"타워 이름";
        towerdamage.text = $"공격력 : ";
        towerAtkSpeed.text = $"공격 빈도 : ";

        selectedTower = null;
    }

    public void OnInfo(Tower tower)
    {
        selectedTower = tower;

        Sprite icon = Resources.Load<Sprite>(string.Format(TowerData.FormatTowerIconsPath, selectedTower.towerIcon));
        if (icon != null)
        {
            towerIcon.sprite = icon;
        }
        towerName.text = $"{tower.towerName.Replace("(Clone)", "")}";
        towerdamage.text = $"공격력 : {tower.damage.ToString()}";
        towerAtkSpeed.text = $"공격 빈도 : {tower.fireRate.ToString("F2")}";
    }

    public void CombinationSlot1()
    {
        AudioManager.Instance.EffectPlay(selectedSound);

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
        AudioManager.Instance.EffectPlay(selectedSound);

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
        AudioManager.Instance.EffectPlay(selectedSound);

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
        AudioManager.Instance.EffectPlay(selectedSound);
        if (combinationTower1 != null && combinationTower2 != null && combinationTower3 != null)
        {
            Tile tile1 = combinationTower1.GetComponentInParent<Tile>();
            Tile tile2 = combinationTower2.GetComponentInParent<Tile>();
            Tile tile3 = combinationTower3.GetComponentInParent<Tile>();

            if (combinationTower1.id ==  combinationTower2.id && combinationTower2.id == combinationTower3.id)
            {
                AudioManager.Instance.EffectPlay(bulidSound);
                int newTowerID = combinationTower1.id + 100;
                TowerData newTowerData;

                if (combinationTower1.towerGrade == 1)
                {
                    newTowerData = towerDatas2.ContainsKey(newTowerID) ? towerDatas2[newTowerID] : null;
                }
                else if (combinationTower1.towerGrade == 2)
                {
                    newTowerData = towerDatas3.ContainsKey(newTowerID) ? towerDatas3[newTowerID] : null;
                }
                else
                {
                    Debug.Log("다음 등급 없음");
                    return;
                }

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
        AudioManager.Instance.EffectPlay(selectedSound);
        if (combinationTower1 != null && combinationTower2 != null && combinationTower3 != null)
        {
            Tile tile1 = combinationTower1.GetComponentInParent<Tile>();
            Tile tile2 = combinationTower2.GetComponentInParent<Tile>();
            Tile tile3 = combinationTower3.GetComponentInParent<Tile>();

            if (combinationTower1.towerGrade == combinationTower2.towerGrade && combinationTower2.towerGrade == combinationTower3.towerGrade)
            {
                AudioManager.Instance.EffectPlay(bulidSound);
                List<TowerData> nextTowers;

                if(combinationTower1.towerGrade == 1)
                {
                    nextTowers = towerDatas2.Values.ToList();
                }
                else if(combinationTower1.towerGrade == 2)
                {
                    nextTowers = towerDatas3.Values.ToList();
                }
                else
                {
                    Debug.Log("다음 등급 없음");
                    return;
                }

                if (nextTowers.Count > 0)
                {
                    TowerData newTowerData = SelectRandomTower(nextTowers);
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
        if(combinationTower1 != null)
        {
            combinationTower1 = null;
            combinationSlot1.GetComponentInChildren<TextMeshProUGUI>().text = null;
        }
    }

    private void ClearSlot2()
    {
        if (combinationTower2 != null)
        {
            combinationTower2 = null;
            combinationSlot2.GetComponentInChildren<TextMeshProUGUI>().text = null;
        }
    }

    private void ClearSlot3()
    {
        if (combinationTower3 != null)
        {
            combinationTower3 = null;
            combinationSlot3.GetComponentInChildren<TextMeshProUGUI>().text = null;
        }
    }

    public void UpgradeCombiTower(int id, TowerData data)
    {
        if (towerDatas2.ContainsKey(id))
        {
            towerDatas2[id].percent += data.percent;
            towerDatas2[id].towerSpeed -= data.towerSpeedInc;
            towerDatas2[id].damage += data.atkInc;
        }
        else if (towerDatas3.ContainsKey(id))
        {
            towerDatas3[id].percent += data.percent;
            towerDatas3[id].towerSpeed -= data.towerSpeedInc;
            towerDatas3[id].damage += data.atkInc;
        }

        if (!towerSpawner.temporaryUpgrades.ContainsKey(id))
        {
            towerSpawner.temporaryUpgrades[id] = new TowerData
            {
                ID = id,
                percent = data.percent,
                towerSpeed = data.towerSpeed,
                damage = data.damage
            };
        }

        towerSpawner.temporaryUpgrades[id].percent += data.percentIncr;
        towerSpawner.temporaryUpgrades[id].towerSpeed -= data.towerSpeedInc;
        towerSpawner.temporaryUpgrades[id].damage += data.atkInc;
    }

    public void AddTowerSlot(Tower tower)
    {
        if (IsTowerInSlots(tower.TowerID))
        {
            return;
        }
        if (combinationTower1 != null && combinationTower2 != null && combinationTower3 != null)
        {
            ClearSlot1();

            combinationTower1 = combinationTower2;
            combinationSlot1.GetComponentInChildren<TextMeshProUGUI>().text = combinationTower2.towerName;

            combinationTower2 = combinationTower3;
            combinationSlot2.GetComponentInChildren<TextMeshProUGUI>().text = combinationTower3.towerName;

            combinationTower3 = tower;
            combinationSlot3.GetComponentInChildren<TextMeshProUGUI>().text = tower.towerName;
        }
        else
        {
            if (combinationTower1 == null)
            {
                combinationTower1 = tower;
                combinationSlot1.GetComponentInChildren<TextMeshProUGUI>().text = tower.towerName;
            }
            else if (combinationTower2 == null)
            {
                combinationTower2 = tower;
                combinationSlot2.GetComponentInChildren<TextMeshProUGUI>().text = tower.towerName;
            }
            else if (combinationTower3 == null)
            {
                combinationTower3 = tower;
                combinationSlot3.GetComponentInChildren<TextMeshProUGUI>().text = tower.towerName;
            }
        }
    }

    public void OnClickCancel()
    {
        ClearSlot1 ();
        ClearSlot2 ();
        ClearSlot3 ();
    }
}
