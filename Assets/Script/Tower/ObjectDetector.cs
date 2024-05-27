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

    public Button button;
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

        if (GameManager.Instance.gold < cost)
        {
            button.enabled = false;
        }
        else
        {
            button.enabled = true;
        }

        if (Input.GetMouseButtonDown(0)) 
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
            }
            else
            {
                ClearSelection();
            }
        }
    }

    private void ClearSelection()
    {
        if(currentOutline != null)
        {
            currentOutline.enabled = false;
        }

        if(selectedTower != null && selectedTower.onRange != null) 
        {
            selectedTower.onRange.SetActive(false);
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
            if(selectedTower.onRange != null)
            {
                selectedTower.onRange.SetActive(true);
            }
        }

        towerCombiner.OnInfo(selectedTower);
        towerCombiner.AddTowerSlot(selectedTower);
    }

    public void OnClick()
    {
        if(selectedTile != null) 
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

    public void OnClickDemolish()
    {
        if(selectedTower != null) 
        {
            if (GameManager.Instance.gold < 5)
            {
                return;
            }
            GameManager.Instance.SubGold(5);
            Tile tile = selectedTower.GetComponentInParent<Tile>();
            tile.RemoveCurrentTower();
            Destroy(selectedTower.gameObject);
        }
    }
}
