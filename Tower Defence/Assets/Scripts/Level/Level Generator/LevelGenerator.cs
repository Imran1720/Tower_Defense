using System.Collections.Generic;
using TowerDefence.Tile;
using UnityEngine;
namespace TowerDefence.Level
{
#if UNITY_EDITOR
    public class LevelGenerator : MonoBehaviour
    {
        [Header("Grid Details")]
        [SerializeField] private int numberOfVerticalTiles;
        [SerializeField] private int numberOfHorizontalTiles;
        [SerializeField] private Transform tileContainerPrefab;
        [SerializeField] private Transform waypointContainerPrefab;
        [SerializeField] private Transform waypointPrefab;
        [SerializeField] private float waypointOffset;
        [SerializeField] private float tileOffset;

        [Header("Tile Details")]
        [SerializeField] private TileListSO tileListSO;

        private TileType[,] tileTypeArray;
        private List<Vector3Int> waypoinGridPositionList = new List<Vector3Int>();
        private Vector3 startPosition;
        private Vector3 startOffset;

        [SerializeField] private Transform tileContainer;
        [SerializeField] private Transform waypointContainer;

        [Header("Editor Details")]
        [SerializeField] private int buttonSize;
        [SerializeField] private int brushButtonSize;

        public void SpawnTiles()
        {
            DeleteTileContainer();

            startOffset = new Vector3((numberOfHorizontalTiles / 2), transform.position.y, (numberOfVerticalTiles / 2));
            startPosition = new Vector3(transform.position.x - startOffset.x, transform.position.y, transform.position.z - startOffset.z);
            SpawnTileContainer();

            for (int i = 0; i < numberOfHorizontalTiles; i++)
            {
                for (int j = 0; j < numberOfVerticalTiles; j++)
                {
                    SpawnTile(i, j);
                }
            }

            SpawnWayPoint();
        }

        private void SpawnTile(int i, int j)
        {
            GameObject spawnedObject = Instantiate(GetPrefabToSpawn(i, j), GetTileSpawnPosition(i, j), Quaternion.identity);
            spawnedObject.transform.SetParent(tileContainer, false);

        }

        private GameObject GetPrefabToSpawn(int x, int y)
        {
            switch (tileTypeArray[x, y])
            {
                case TileType.ROAD:
                case TileType.WAYPOINT:
                    return GetTileOfType(TileType.ROAD);

                case TileType.TREE:
                    return GetTileOfType(GetRandomTreeTile());

                case TileType.WATER:
                    return GetTileOfType(TileType.WATER);

                case TileType.ROCK:
                    return GetTileOfType(TileType.ROCK);

                case TileType.SHRUB:
                    return GetTileOfType(TileType.SHRUB);

                case TileType.GRASS:
                default:
                    return GetTileOfType(TileType.GRASS);

            }
        }

        private TileType GetRandomTreeTile()
        {
            int treeCount = Random.Range(1, 4);

            switch (treeCount)
            {
                case 3:
                    return TileType.TREE_3;
                case 2:
                    return TileType.TREE_2;
                case 1:
                default:
                    return TileType.TREE_1;
            }
        }

        private void SpawnTileContainer()
        {
            tileContainer = Instantiate(tileContainerPrefab, transform.position, Quaternion.identity);
            //tileContainer.transform.SetParent(transform, false);
            waypointContainer = Instantiate(waypointContainerPrefab, transform.position, Quaternion.identity);
        }

        private void DeleteTileContainer()
        {
            if (tileContainer != null)
                DestroyImmediate(tileContainer.gameObject);

            if (waypointContainer != null)
                DestroyImmediate(waypointContainer.gameObject);
        }
        private Vector3 GetTileSpawnPosition(int x, int z)
        {
            Vector3 pos = (startPosition + new Vector3(x, 0, z)) + new Vector3(x * tileOffset, 0, z * tileOffset);
            if (tileTypeArray[x, z] == TileType.WAYPOINT)
            {
                Debug.Log(startPosition);
                Debug.Log(x + " " + z);
                Debug.Log(x * tileOffset + " " + z * tileOffset);
                Debug.Log(pos);
            }

            return pos;
        }

        public void CreateTileArray()
        {
            if (numberOfVerticalTiles == 0 || numberOfHorizontalTiles == 0)
            {
                return;
            }
            tileTypeArray = new TileType[numberOfHorizontalTiles, numberOfVerticalTiles];
        }
        private GameObject GetTileOfType(TileType tileType)
        {
            TileTypeData data = tileListSO.tileTypesList.Find(item => item.tileType == tileType);
            if (data == null)
                return null;

            return data.tilePrefab;
        }

        public void ClearLevel()
        {
            ResetTileArray();
            DeleteTileContainer();
        }
        public void DeleteLevel()
        {
            ResetTileArray();
            tileTypeArray = null;
            DeleteTileContainer();
            waypoinGridPositionList.Clear();
        }

        private void ResetTileArray()
        {
            if (tileTypeArray == null)
            {
                return;
            }
            for (int i = 0; i < numberOfHorizontalTiles; i++)
            {
                for (int j = 0; j < numberOfVerticalTiles; j++)
                {
                    tileTypeArray[i, j] = TileType.GRASS;
                }
            }

            waypoinGridPositionList.Clear();
        }

        //GETTERS
        public bool IsTileArrayEmpty() => tileTypeArray == null;

        public int GetButtonCountInRow() => numberOfHorizontalTiles;
        public int GetButtonCountInColumn() => numberOfVerticalTiles;
        public int GetBrushButtonSize() => brushButtonSize;
        public int GetButtonSize() => buttonSize;
        public TileType GetTileTypeOf(int x, int y) => tileTypeArray[x, y];

        //SETTER
        public void SetTileValue(int x, int y, TileType tileType)
        {
            tileTypeArray[x, y] = tileType;
        }

        public void AddWaypointPosition(Vector3Int position)
        {
            waypoinGridPositionList.Add(position);
        }
        public void SpawnWayPoint()
        {
            //Debug.Log(waypoinGridPositionList.Count);
            foreach (Vector3Int pos in waypoinGridPositionList)
            {
                Vector3 gridWorldPosition = GetTileSpawnPosition(pos.x, pos.z);
                Debug.Log("grid:" + gridWorldPosition);

                Vector3 spawnPosition = new Vector3(gridWorldPosition.x, gridWorldPosition.y + waypointOffset, gridWorldPosition.z);
                Debug.Log("grid:" + spawnPosition);

                Transform waypoint = Instantiate(waypointPrefab, spawnPosition, Quaternion.identity);
                waypoint.SetParent(waypointContainer, false);
            }
        }
    }
#endif
}
