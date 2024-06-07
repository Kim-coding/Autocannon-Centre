using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EPOOutline;
using TMPro;
using UnityEngine.EventSystems;

public class ObjectDetector : MonoBehaviour
{
    private TowerSpawner towerSpawner;
    private GameManager gameManager;
    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;

    private Transform selectedTile;
    private Tower selectedTower;

    public TowerCombiner towerCombiner;

    private Outlinable currentOutline;
    private int cost = 10;

    public Button button;
    public Button autoButton;
    private bool isAutoAddMode = false;

    private EventSystem eventSystem;
    public List<GraphicRaycaster> graphicRaycasters;
    private void Awake()
    {
        mainCamera = Camera.main;
        gameManager = GetComponent<GameManager>();
        towerSpawner = GetComponent<TowerSpawner>();
        eventSystem = EventSystem.current;

        if (autoButton != null )
        {
            autoButton.onClick.AddListener(() => TowerAutoAddMode());
        }
    }

    private void Update()
    {
        if (gameManager == null)
        {
            return;
        }

        if (gameManager.isGameOver)
        {
            return;
        }

        if (gameManager.GetGold() < cost)
        {
            button.enabled = false;
        }
        else
        {
            button.enabled = true;
        }

        if (Input.GetMouseButtonDown(0)) 
        {
            if (IsPointerOverSpecificUIObject())
            {
                return;
            }

            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray,out hit,Mathf.Infinity))
            {
                if(hit.transform.CompareTag("Tile"))
                {
                    AudioManager.Instance.SelectedSoundPlay();
                    SelectTile(hit.transform);
                }
                else if (hit.transform.CompareTag("Tower"))
                {
                    AudioManager.Instance.SelectedSoundPlay();
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
            selectedTower.onLevel.SetActive(false);
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
                selectedTower.onLevel.SetActive(true);
            }
        }

        towerCombiner.OnInfo(selectedTower);
        if(isAutoAddMode) 
        {
            towerCombiner.AddTowerSlot(selectedTower);
        }
    }

    private void TowerAutoAddMode()
    {
        isAutoAddMode = !isAutoAddMode;
        if(isAutoAddMode ) 
        {
            autoButton.GetComponentInChildren<TextMeshProUGUI>().text = "ON";
        }
        else
        {
            autoButton.GetComponentInChildren<TextMeshProUGUI>().text = "OFF";
        }
    }

    public void OnClick()
    {
        if(selectedTile != null) 
        {
            if(gameManager.GetGold() < cost)
            {
                return;
            }
            AudioManager.Instance.SelectedSoundPlay();
            gameManager.SubGold(cost);
            towerSpawner.Spawn(selectedTile);
        }
        selectedTile = null;
    }

    public void OnClickDemolish()
    {
        if(selectedTower != null) 
        {
            if (gameManager.GetGold() < 5)
            {
                return;
            }
            AudioManager.Instance.SelectedSoundPlay();
            gameManager.SubGold(5);
            Tile tile = selectedTower.GetComponentInParent<Tile>();
            tile.RemoveCurrentTower();
            Destroy(selectedTower.gameObject);
        }
    }

    private bool IsPointerOverSpecificUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(eventSystem)
        {
            position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
        };

        List<RaycastResult> results = new List<RaycastResult>();

        foreach (var raycaster in graphicRaycasters)
        {
            raycaster.Raycast(eventDataCurrentPosition, results);
            if (results.Count > 0)
            {
                return true;
            }
        }

        return false;
    }
}
