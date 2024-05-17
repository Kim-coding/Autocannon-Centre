using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using EPOOutline;

public class TowerCombiner : MonoBehaviour
{
    public TextMeshProUGUI towerName;
    public TextMeshProUGUI towerdamage;
    public TextMeshProUGUI towerAtkSpeed;
    public TextMeshProUGUI towerRange;

    public GameObject combinationSlot1;
    public GameObject combinationSlot2;
    public GameObject combinationSlot3;

    public Button combiButton;
    public Button randomCombiButton;

    private Tower selectedTower;
    private string combinationTower1ID;
    private string combinationTower2ID;
    private string combinationTower3ID;

    private Outlinable currentOutline;

    private Camera mainCamara;
    private Ray ray;
    private RaycastHit hit;

    private void Awake()
    {
        mainCamara = Camera.main;
        combiButton = Camera.main.GetComponent<Button>();
        randomCombiButton = Camera.main.GetComponent<Button>();

        combinationTower1ID = string.Empty;
        combinationTower2ID = string.Empty;
        combinationTower3ID = string.Empty;
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
        towerName.text = $"name : {tower.name.Replace("(Clone)", "")}";
        towerdamage.text = $"damage : {tower.damage.ToString()}";
        towerAtkSpeed.text = $"atk Speed : {tower.speed.ToString()}";
        towerRange.text = $"range : {tower.range.ToString()}";
    }

    public void CombinationSlot1()
    {
        if (!string.IsNullOrEmpty(combinationTower1ID))
        {
            ClearSlot1();
        }
        else if (selectedTower != null && !IsTowerInSlots(selectedTower.TowerID)) 
        {
            combinationTower1ID = selectedTower.TowerID;
            combinationSlot1.GetComponentInChildren<TextMeshProUGUI>().text = selectedTower.TowerID;
        }
    }

    public void CombinationSlot2()
    {
        if (!string.IsNullOrEmpty(combinationTower2ID))
        {
            ClearSlot2();
        }
        else if(selectedTower != null && !IsTowerInSlots(selectedTower.TowerID))
        {
            combinationTower2ID = selectedTower.TowerID;
            combinationSlot2.GetComponentInChildren<TextMeshProUGUI>().text = selectedTower.TowerID;
        }
    }

    public void CombinationSlot3()
    {
        if (!string.IsNullOrEmpty(combinationTower3ID))
        {
            ClearSlot3();
        }
        else if(selectedTower != null && !IsTowerInSlots(selectedTower.TowerID))
        {
            combinationTower3ID = selectedTower.TowerID;
            combinationSlot3.GetComponentInChildren<TextMeshProUGUI>().text = selectedTower.TowerID;
        }
    }

    public void OnClickRandomButton()
    {
        if(combinationTower1ID != null && combinationTower2ID != null && combinationTower3ID != null) 
        {
            //타워 조합
        }
    }

    public void OnClickButton()
    {
        if (combinationTower1ID != null && combinationTower2ID != null && combinationTower3ID != null)
        {

        }
    }

    private bool IsTowerInSlots(string towerID)
    {
        return towerID == combinationTower1ID || towerID == combinationTower2ID || towerID == combinationTower3ID;
    }

    private void ClearSlot1()
    {
        combinationTower1ID = string.Empty;
        combinationSlot1.GetComponentInChildren<TextMeshProUGUI>().text = "Empty";
    }

    private void ClearSlot2()
    {
        combinationTower2ID = string.Empty;
        combinationSlot2.GetComponentInChildren<TextMeshProUGUI>().text = "Empty";
    }

    private void ClearSlot3()
    {
        combinationTower3ID = string.Empty;
        combinationSlot3.GetComponentInChildren<TextMeshProUGUI>().text = "Empty";
    }
}
