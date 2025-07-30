using TowerDefence.Level;
using TowerDefence.Tile;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelGenerator))]
public class LevelGeneratorEditor : Editor
{
    private int buttonCountInRow;
    private int buttonCountInColumn;
    private int buttonSize;
    private int brushButtonSize;
    private LevelGenerator levelGenerator;
    private TileType tileTypeToSet;
    private TileType currenttileType;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        levelGenerator = (LevelGenerator)target;

        InitializeEditorData();

        if (GUILayout.Button("Create Tile Type Array"))
        {
            levelGenerator.CreateTileArray();
        }


        if (levelGenerator.IsTileArrayEmpty())
        {
            return;
        }

        EditorGUILayout.Space(20);

        for (int i = 0; i < buttonCountInRow; i++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < buttonCountInColumn; j++)
            {
                currenttileType = levelGenerator.GetTileTypeOf(i, j);
                GUI.color = GetTileColor(currenttileType);
                if (GUILayout.Button("", GUILayout.Width(buttonSize * 2), GUILayout.Height(buttonSize * 2)))
                {
                    Undo.RecordObject(levelGenerator, "Toggle Tile Type");
                    levelGenerator.SetTileValue(i, j, tileTypeToSet);
                    EditorUtility.SetDirty(levelGenerator);
                    Repaint();
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space(20);


        GUI.color = Color.white;
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Draw Grass", GUILayout.Width(brushButtonSize * 2), GUILayout.Height(brushButtonSize)))
        {
            SetTileToDraw(TileType.GRASS);
        }

        if (GUILayout.Button("Draw Road", GUILayout.Width(brushButtonSize * 2), GUILayout.Height(brushButtonSize)))
        {
            SetTileToDraw(TileType.ROAD);
        }

        if (GUILayout.Button("Draw Water", GUILayout.Width(brushButtonSize * 2), GUILayout.Height(brushButtonSize)))
        {
            SetTileToDraw(TileType.WATER);
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(20);
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Draw Rock", GUILayout.Width(brushButtonSize * 2), GUILayout.Height(brushButtonSize)))
        {
            SetTileToDraw(TileType.ROCK);
        }

        if (GUILayout.Button("Draw Tree", GUILayout.Width(brushButtonSize * 2), GUILayout.Height(brushButtonSize)))
        {
            SetTileToDraw(GetRandomTreeTile());
        }

        if (GUILayout.Button("Draw Shrub", GUILayout.Width(brushButtonSize * 2), GUILayout.Height(brushButtonSize)))
        {
            SetTileToDraw(TileType.SHRUB);
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(20);


        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Generate Level"))
        {
            levelGenerator.SpawnTiles();
        }
        if (GUILayout.Button("Clear Level"))
        {
            levelGenerator.ClearLevel();
        }

        EditorGUILayout.EndHorizontal();
    }

    private void SetTileToDraw(TileType type)
    {
        tileTypeToSet = type;
    }

    private Color GetTileColor(TileType type)
    {
        switch (type)
        {
            case TileType.GRASS:
            default:
                return Color.green;
            case TileType.ROAD:
                return Color.red;
            case TileType.WATER:
                return Color.cyan;
            case TileType.TREE_1:
            case TileType.TREE_2:
            case TileType.TREE_3:
                return Color.yellow;
            case TileType.ROCK:
                return Color.grey;
            case TileType.SHRUB:
                return Color.magenta;
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

    private void InitializeEditorData()
    {
        buttonCountInRow = levelGenerator.GetButtonCountInRow();
        buttonCountInColumn = levelGenerator.GetButtonCountInColumn();
        buttonSize = levelGenerator.GetButtonSize();
        brushButtonSize = levelGenerator.GetBrushButtonSize();
    }
}
