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

        [Header("Tile Details")]
        [SerializeField] private TileListSO tileListSO;

        private TileType[,] tileTypeArray;
        private Vector3 startPosition;
        private Vector3 startOffset;
        private Transform tileContainer;

        [Header("Editor Details")]
        [SerializeField] private int buttonSize;
        [SerializeField] private int brushButtonSize;
        private void Start()
        {
            SpawnTiles();
        }

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
                    GameObject spawnedObject = Instantiate(GetPrefabToSpawn(i, j), GetTileSpawnPosition(i, j), Quaternion.identity);
                    spawnedObject.transform.SetParent(tileContainer, false);
                }
            }
        }

        private GameObject GetPrefabToSpawn(int x, int y)
        {
            switch (tileTypeArray[x, y])
            {
                case TileType.ROAD:
                    return GetTileOfType(TileType.ROAD);

                case TileType.TREE_1:
                    return GetTileOfType(TileType.TREE_1);
                case TileType.TREE_2:
                    return GetTileOfType(TileType.TREE_2);
                case TileType.TREE_3:
                    return GetTileOfType(TileType.TREE_3);

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



        private void SpawnTileContainer()
        {
            tileContainer = Instantiate(tileContainerPrefab, transform.position, Quaternion.identity);
            tileContainer.transform.SetParent(transform, false);
        }

        private void DeleteTileContainer()
        {
            if (tileContainer == null)
                return;
            DestroyImmediate(tileContainer.gameObject);
        }
        private Vector3 GetTileSpawnPosition(int x, int z)
        {
            return startPosition + new Vector3(x, 0, z);
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
        }

        //GETTERS
        public bool IsTileArrayEmpty() => tileTypeArray == null;

        public int GetButtonCountInRow() => numberOfHorizontalTiles;
        public int GetButtonCountInColumn() => numberOfVerticalTiles;
        public int GetBrushButtonSize() => brushButtonSize;
        public int GetButtonSize() => buttonSize;

        public TileType GetTileTypeOf(int x, int y)
        {
            return tileTypeArray[x, y];
        }


        //SETTER
        public void SetTileValue(int x, int y, TileType tileType)
        {
            tileTypeArray[x, y] = tileType;
        }
    }
#endif
}
