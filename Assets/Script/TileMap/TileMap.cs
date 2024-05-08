using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    public GameObject parent;
    public float cellSize = 0.5f;

    private GameObject selectedPrefab;
    private GameObject currentPrefab;
    private bool isPlacing = false;
    private Dictionary<Vector3, GameObject> gridTiles = new Dictionary<Vector3, GameObject>();

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SlectedPrefab();
            isPlacing = true;
        }
        else if(Input.GetMouseButtonUp(0)) 
        {
            isPlacing = false ;
        }

        if(isPlacing) 
        {
            UpdateTilePositon();
        }
    }

    private void SlectedPrefab()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition );
        RaycastHit hit;
        
        if( Physics.Raycast(ray, out hit) )
        {
            if (hit.collider.gameObject.tag == "Tile")
            {
                selectedPrefab = hit.collider.gameObject;
                //StartPlacingTile();
            }
        }
       
    }
    private void StartPlacingTile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            currentPrefab = Instantiate(selectedPrefab, hit.point, Quaternion.identity);
            currentPrefab.transform.SetParent(parent.transform);
            
        }
    }

    private void UpdateTilePositon()
    {
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;

        //if (Physics.Raycast(ray, out hit))
        //{
        //    Vector3 newPos = hit.point;
        //    newPos.y = 0f;

        //    newPos.x = Mathf.Round(newPos.x / cellSize) * cellSize;
        //    newPos.z = Mathf.Round(newPos.z / cellSize) * cellSize;
        //    currentPrefab.transform.position = newPos;
        //}

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 newPos = SnapToGrid(hit.point);
            if (gridTiles.TryGetValue(newPos, out GameObject existingTile))
            {
                Destroy(existingTile);  // 기존 타일 삭제
            }

            GameObject newTile = Instantiate(selectedPrefab, newPos, Quaternion.identity, parent.transform);
            gridTiles[newPos] = newTile;  // 새 타일 저장
        }
    }

    private Vector3 SnapToGrid(Vector3 originalPosition)
    {
        float newX = Mathf.Round(originalPosition.x / cellSize) * cellSize;
        float newZ = Mathf.Round(originalPosition.z / cellSize) * cellSize;
        return new Vector3(newX, 0.0f, newZ);
    }
}
