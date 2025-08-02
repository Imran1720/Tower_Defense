using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Transform cellHighlighterPrefab;
    [SerializeField] private float highlighterOffset;
    [SerializeField] private float towerSpawnOffset;

    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private LayerMask grassLayerMask;
    [SerializeField] private float maxRayDistance;

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

        if (Input.GetKeyUp(KeyCode.Mouse0) && isEditModeActive)
        {
            //Tower Placing logic
            Debug.Log(tileView);
            towerTile = tileView;
            towerPosition = new Vector3(cellPosition.x, cellPosition.y + towerSpawnOffset, cellPosition.z);
            Instantiate(towerPrefab, towerPosition, Quaternion.identity);
            towerTile.SetTileOccupied();
            Debug.Log(tileView);
        }

    }

    private void PlaceCellHighlighter()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, maxRayDistance, grassLayerMask))
        {
            tileView = hit.collider.gameObject.GetComponent<TileView>();
            if (tileView != null && tileView.IsTileFree())
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
