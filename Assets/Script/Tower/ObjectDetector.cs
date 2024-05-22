using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EPOOutline;

public class ObjectDetector : MonoBehaviour
{
    private TowerSpawner towerSpawner;
    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;

    private Transform selectedTile;
    private Tower selectedTower;

    public TowerCombiner towerCombiner;
    public AudioClip selectedSound;

    private Outlinable currentOutline;
    private int cost = 10;
    private void Awake()
    {
        mainCamera = Camera.main;
        towerSpawner = GetComponent<TowerSpawner>();
    }

    private void Update()
    {
        if(GameManager.Instance.isGameOver)
        {
            return;
        }

        if(Input.GetMouseButtonDown(0)) 
        {
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray,out hit,Mathf.Infinity))
            {
                if(hit.transform.CompareTag("Tile"))
                {
                    AudioManager.Instance.EffectPlay(selectedSound);
                    SelectTile(hit.transform);
                }
                else if (hit.transform.CompareTag("Tower"))
                {
                    AudioManager.Instance.EffectPlay(selectedSound);
                    SelectTower(hit.collider.GetComponent<Tower>());
                }
                else
                {
                    ClearSelection();
                }
            }
        }
    }

    private void ClearSelection()
    {
        if(currentOutline != null)
        {
            currentOutline.enabled = false;
        }

        selectedTile = null;
        selectedTower = null;
        towerCombiner.ClearSelection();
    }

    private void SelectTile(Transform tile)
    {
        ClearSelection();
        selectedTile = tile;
        selectedTower = null;

        var outline = selectedTile.GetComponent<Outlinable>();
        if (outline != null) 
        {
            outline.enabled = true;
            currentOutline = outline;
        }
    }

    private void SelectTower(Tower tower)
    {
        ClearSelection();
        selectedTower = hit.collider.GetComponent<Tower>();
        selectedTile = null;

        var outline = selectedTower.GetComponent<Outlinable>();
        if (outline != null)
        {
            outline.enabled = true;
            currentOutline = outline;
        }

        towerCombiner.OnInfo(selectedTower);
    }

    public void OnClick()
    {
        AudioManager.Instance.EffectPlay(selectedSound);

        if (selectedTile != null) 
        {
            if(GameManager.Instance.gold < cost)
            {
                return;
            }
            GameManager.Instance.SubGold(cost);
            towerSpawner.Spawn(selectedTile);
        }
        selectedTile = null;
    }

}
