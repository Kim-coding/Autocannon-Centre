using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectDetector : MonoBehaviour
{
    private TowerSpawner towerSpawner;
    private UpgradeTower upgradeTower;
    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;

    private Transform selectedTile;  //타일 선택
    private Tower selectedTower;      //타워 선택


    

    private void Awake()
    {
        
        mainCamera = Camera.main;
        towerSpawner = GetComponent<TowerSpawner>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0)) 
        {
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray,out hit,Mathf.Infinity))
            {
                if(hit.transform.CompareTag("Tile"))
                {
                    if (towerSpawner != null)
                    {
                        selectedTile = hit.transform;
                        selectedTower = null;
                    }
                }
                else if (hit.collider.GetComponent<Tower>() != null)
                {
                    selectedTower = hit.collider.GetComponent<Tower>();
                    selectedTile = null;
                }
            }
        }
    }

    public void OnClick()
    {
        if(selectedTile != null) 
        {
            if(GameManager.Instance.gold <= 0)
            {
                return;
            }
            GameManager.Instance.SubGold(10);
            towerSpawner.Spawn(selectedTile);
        }
        selectedTile = null;
    }

}
