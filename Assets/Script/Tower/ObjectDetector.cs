using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectDetector : MonoBehaviour
{
    private TowerSpawner towerSpawner;
    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;

    private Transform selectedTile;
    private Tower selectedTower;


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
                    SelectTile(hit.transform);
                }
                else if (hit.collider.GetComponent<Tower>() != null)
                {
                    SelectTower(hit.collider.GetComponent<Tower>());
                }
            }
        }
    }

    private void ClearSelection()
    {
        selectedTile = null;
        selectedTower = null;
    }

    private void SelectTile(Transform tile)
    {
        ClearSelection();
        selectedTile = hit.transform;
        selectedTower = null;
    }

    private void SelectTower(Tower tower)
    {
        ClearSelection();
        selectedTower = hit.collider.GetComponent<Tower>();
        selectedTile = null;
    }

    public void OnClick()
    {
        if(selectedTile != null) 
        {
            if(GameManager.Instance.gold < 10)
            {
                return;
            }
            GameManager.Instance.SubGold(10);
            towerSpawner.Spawn(selectedTile);
        }
        selectedTile = null;
    }

}
