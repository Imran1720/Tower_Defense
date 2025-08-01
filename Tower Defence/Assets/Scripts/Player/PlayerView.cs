using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Transform cellHighlighterPrefab;
    [SerializeField] private float highlighterOffset;

    [SerializeField] private GameObject towerPrefab;

    private Transform cellHighlighter;
    private bool isEditModeActive = false;

    Ray ray;
    TileView tileView;
    TileView towerTile;

    Vector3 cellPosition;


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

        if (Input.GetKeyUp(KeyCode.Mouse0) && isEditModeActive)
        {
            //Tower Placing logic

            towerTile = tileView;
            towerTile.SetTileOccupied();

            Instantiate(towerPrefab, cellPosition, Quaternion.identity);
        }

        if (isEditModeActive)
        {
            // Updating highlighter position
            PlaceCellHighlighter();
        }
    }

    private void PlaceCellHighlighter()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
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
