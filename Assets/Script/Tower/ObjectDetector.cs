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

    public Button purchaseButton;
    private Transform selectedTile;  //Ÿ�� ����
    public Tower selectedTower;      //Ÿ�� ����

    private Outline outline;

    private void Awake()
    {
        mainCamera = Camera.main;
        towerSpawner = GetComponent<TowerSpawner>();

        if (towerSpawner == null)
        {
            Debug.LogError("TowerSpawner component not found!");
        }
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
            towerSpawner.Spawn(selectedTile);
        }
    }

    public void OnIpgradClick()
    {

    }
}
