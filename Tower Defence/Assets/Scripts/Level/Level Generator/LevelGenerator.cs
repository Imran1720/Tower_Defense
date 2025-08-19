using System.Collections.Generic;
using TowerDefence.Tile;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
namespace TowerDefence.Level
{
#if UNITY_EDITOR
    public class LevelGenerator : MonoBehaviour
    {
        [Header("Grid Configuration")]
        [SerializeField] private int numberOfVerticalTiles;
        [SerializeField] private int numberOfHorizontalTiles;
        [SerializeField] private float tileOffset;

        [Header("Waypoints")]
        [SerializeField] private Transform waypointPrefab;
        [SerializeField] private float waypointOffset;
        private List<Vector3Int> waypointGridPositions = new();
        [SerializeField, HideInInspector] private List<Transform> waypointInstances;

        [Header("Tile Configuration")]
        [SerializeField] private TileListSO tileListSO;

        [Header("Editor Configuration")]
        [SerializeField] private int buttonSize;
        [SerializeField] private int brushButtonSize;
        [SerializeField] private GameService gameService;

        private TileType[,] tileArray;
        private Vector3 startPosition;
        private Vector3 offsetOrigin;
        private Transform levelContainer;
        private Transform waypointContainer;

        public void CreateTileArray()
        {
            if (numberOfHorizontalTiles <= 0 || numberOfVerticalTiles <= 0) return;
            tileArray = new TileType[numberOfHorizontalTiles, numberOfVerticalTiles];
        }

        public void SpawnTiles()
        {
            DeleteContainers();
            SetContainers();

            offsetOrigin = new Vector3(numberOfHorizontalTiles / 2f, transform.position.y, numberOfVerticalTiles / 2f);
            startPosition = new Vector3(transform.position.x - offsetOrigin.x, transform.position.y, transform.position.z - offsetOrigin.z);

            for (int x = 0; x < numberOfHorizontalTiles; x++)
            {
                for (int z = 0; z < numberOfVerticalTiles; z++)
                {
                    SpawnTile(x, z);
                }
            }

            SpawnWaypoints();
        }

        public void ClearLevel()
        {
            ResetTileArray();
            DeleteContainers();
        }

        public void DeleteLevel()
        {
            ResetTileArray();
            tileArray = null;
            DeleteContainers();
            waypointGridPositions.Clear();
        }

        public bool IsTileArrayEmpty() => tileArray == null;
        public int GetButtonCountInRow() => numberOfHorizontalTiles;
        public int GetButtonCountInColumn() => numberOfVerticalTiles;
        public int GetBrushButtonSize() => brushButtonSize;
        public int GetButtonSize() => buttonSize;
        public TileType GetTileTypeAt(int x, int y) => tileArray[x, y];

        public void SetTileType(int x, int y, TileType type)
        {
            tileArray[x, y] = type;
        }

        public void AddWaypointPosition(Vector3Int position)
        {
            waypointGridPositions.Add(position);
        }

        private void SpawnTile(int x, int z)
        {
            GameObject prefab = GetTilePrefabToSpawn(x, z);
            if (prefab == null) return;

            GameObject instance = Instantiate(prefab, GetTileSpawnPosition(x, z), Quaternion.identity);
            instance.transform.SetParent(levelContainer, false);
        }

        private GameObject GetTilePrefabToSpawn(int x, int z)
        {
            TileType type = tileArray[x, z];
            return type switch
            {
                TileType.ROAD or TileType.WAYPOINT => GetPrefabOfType(TileType.ROAD),
                TileType.TREE => GetPrefabOfType(GetRandomTreeVariant()),
                TileType.WATER => GetPrefabOfType(TileType.WATER),
                TileType.ROCK => GetPrefabOfType(TileType.ROCK),
                TileType.SHRUB => GetPrefabOfType(TileType.SHRUB),
                TileType.GRASS or _ => GetPrefabOfType(TileType.GRASS),
            };
        }

        private TileType GetRandomTreeVariant()
        {
            return (TileType)Random.Range((int)TileType.TREE_1, (int)TileType.TREE_3 + 1);
        }

        private GameObject GetPrefabOfType(TileType type)
        {
            TileTypeData data = tileListSO.tileTypesList.Find(item => item.tileType == type);
            return data?.tilePrefab;
        }

        private Vector3 GetTileSpawnPosition(int x, int z)
        {
            Vector3 gridOffset = new Vector3(x * tileOffset, 0, z * tileOffset);
            return startPosition + new Vector3(x, 0, z) + gridOffset;
        }

        private void ResetTileArray()
        {
            if (tileArray == null) return;

            for (int x = 0; x < numberOfHorizontalTiles; x++)
            {
                for (int z = 0; z < numberOfVerticalTiles; z++)
                {
                    tileArray[x, z] = TileType.GRASS;
                }
            }

            waypointGridPositions.Clear();
        }

        private void SpawnWaypoints()
        {
            foreach (Vector3Int pos in waypointGridPositions)
            {
                Vector3 basePos = GetTileSpawnPosition(pos.x, pos.z);
                Vector3 spawnPos = new Vector3(basePos.x, basePos.y + waypointOffset, basePos.z);

                Transform waypoint = Instantiate(waypointPrefab, spawnPos, Quaternion.identity);
                waypoint.SetParent(waypointContainer, false);
                waypointInstances.Add(waypoint);

                EditorUtility.SetDirty(this);
                EditorSceneManager.MarkSceneDirty(gameObject.scene);
            }
        }
        private void SetContainers()
        {
            levelContainer = gameService.GetLevelContainer();
            waypointContainer = gameService.GetWaypointContainer();
            waypointInstances = new List<Transform>();
        }

        private void DeleteContainers()
        {
            DeleteChildren(levelContainer);
            DeleteChildren(waypointContainer);
        }

        private void DeleteChildren(Transform parent)
        {
            if (parent == null) return;

            List<Transform> children = new();
            foreach (Transform child in parent)
                children.Add(child);

            foreach (Transform child in children)
                DestroyImmediate(child.gameObject);
        }
    }
#endif
}
