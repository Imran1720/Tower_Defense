using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    [Header("Spawn-Offset-Data")]
    [SerializeField] private float towerSpawnOffset;
    [SerializeField] private float highlighterOffset;

    [Header("Prefabs")]
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private Transform cellHighlighterPrefab;

    [Header("Tile-Detection-Data")]
    [SerializeField] private float maxRayDistance;
    [SerializeField] private LayerMask grassLayerMask;

    private Transform cellHighlighter;
    private bool isEditModeActive = false;

    Ray ray;
    TileView tileView;
    TileView towerTile;

    Vector3 cellPosition;
    Vector3 towerPosition;


    private void Start()
    {
        Vector3 position = new Vector3(0, -5, 0);
        cellHighlighter = Instantiate(cellHighlighterPrefab, position, Quaternion.identity);

        EnableCellHighlighter(false);
    }

    private void EnableCellHighlighter(bool value)
    {
        cellHighlighter.gameObject.SetActive(value);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            //Entering Edit mode
            ToggleEditMode();
        }

        if (isEditModeActive)
        {
            // Updating highlighter position
            PlaceCellHighlighter();
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && isEditModeActive && tileView.IsTileFree())
        {
            //Tower Placing logic
            towerTile = tileView;
            towerTile.SetTileOccupied();
            towerPosition = new Vector3(cellPosition.x, cellPosition.y + towerSpawnOffset, cellPosition.z);
            Instantiate(towerPrefab, towerPosition, Quaternion.identity);
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && !tileView.IsTileFree())
        {
            //Show tower range and UI to update or sell
        }

    }

    private void PlaceCellHighlighter()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, maxRayDistance, grassLayerMask))
        {
            tileView = hit.collider.gameObject.GetComponent<TileView>();
            if (tileView != null)
            {
                EnableCellHighlighter(true);
                cellPosition = tileView.GetTilePosition();
                cellHighlighter.transform.position = new Vector3(cellPosition.x, cellPosition.y + highlighterOffset, cellPosition.z);
            }
        }
        else
        {
            EnableCellHighlighter(false);
        }
    }

    private void ToggleEditMode()
    {
        isEditModeActive = !isEditModeActive;
        EnableCellHighlighter(isEditModeActive);
    }
}
