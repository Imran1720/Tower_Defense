using TowerDefence.Tile;
using UnityEngine;

public class TileView : MonoBehaviour
{
    [SerializeField] private TileState currentTileState;

    public Vector3 GetTilePosition() => transform.position;
    public bool IsTileFree() => currentTileState == TileState.FREE;

    public void SetTileOccupied() => currentTileState = TileState.OCCUPIED;
}
