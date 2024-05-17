using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using EPOOutline;

public class TowerCombiner : MonoBehaviour
{
    public GameObject towerInfo;
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
    private Tower combinationTower1;
    private Tower combinationTower2;
    private Tower combinationTower3;

    private Outlinable currentOutline;

    private Camera mainCamara;
    private Ray ray;
    private RaycastHit hit;

    private void Awake()
    {
        mainCamara = Camera.main;
        combiButton = Camera.main.GetComponent<Button>();
        randomCombiButton = Camera.main.GetComponent<Button>();
        towerInfo.SetActive(false);
    }

    public void ClearSelection()
    {
        if(currentOutline != null)
        {
            currentOutline.enabled = false;
        }

        selectedTower = null;
        combinationTower1 = null;
        combinationTower2 = null;
        combinationTower3 = null;

        //combinationSlot1 = null;
        //combinationSlot2 = null;

        towerInfo.SetActive(false);
    }

    public void OnInfo(Tower tower)
    {
        selectedTower = tower;
        towerInfo.SetActive(true);
        towerName.text = $"name : {tower.name.Replace("(Clone)", "")}";
        towerdamage.text = $"damage : {tower.damage.ToString()}";
        towerAtkSpeed.text = $"atk Speed : {tower.speed.ToString()}";
        towerRange.text = $"range : {tower.range.ToString()}";
    }

    public void CombinationSlot1()
    {
        if(selectedTower != null) 
        {
            combinationTower1 = selectedTower;
            combinationSlot1.GetComponentInChildren<TextMeshProUGUI>().text = selectedTower.name;
        }
    }

    public void CombinationSlot2()
    {
        if(selectedTower != null)
        {
            combinationTower2 = selectedTower;
            combinationSlot2.GetComponentInChildren<TextMeshProUGUI>().text = selectedTower.name;
        }
    }

    public void CombinationSlot3()
    {
        if (selectedTower != null)
        {
            combinationTower3 = selectedTower;
            combinationSlot3.GetComponentInChildren<TextMeshProUGUI>().text = selectedTower.name;
        }
    }

    public void OnClickRandomButton()
    {
        if(combinationTower1 != null && combinationTower2 != null && combinationTower3 != null) 
        {
            //타워 조합
        }
    }

    public void OnClickButton()
    {
        if (combinationTower1 != null && combinationTower2 != null && combinationTower3 != null)
        {

        }
    }
}
