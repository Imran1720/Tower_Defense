using TowerDefence.Level;
using TowerDefence.Tile;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelGenerator))]
public class LevelGeneratorEditor : Editor
{
    private LevelGenerator levelGenerator;

    private int rows, columns;
    private int tileSize, brushSize;

    private TileType currentTileType;

    private bool isHoverDrawEnabled = false;

    public override void OnInspectorGUI()
    {
        levelGenerator = (LevelGenerator)target;

        DrawDefaultInspector();
        InitializeEditorData();

        DrawUtilityButtons();

        if (levelGenerator.IsTileArrayEmpty())
            return;

        DrawTileGrid();
        DrawTileBrushPalette();
        DrawLevelActions();
    }

    private void InitializeEditorData()
    {
        rows = levelGenerator.GetButtonCountInRow();
        columns = levelGenerator.GetButtonCountInColumn();
        tileSize = levelGenerator.GetButtonSize();
        brushSize = levelGenerator.GetBrushButtonSize();
    }

    private void DrawUtilityButtons()
    {
        if (GUILayout.Button("Create Tile Array"))
            levelGenerator.CreateTileArray();

        EditorGUILayout.Space(10);
    }

    private void DrawTileGrid()
    {
        for (int x = 0; x < rows; x++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int z = 0; z < columns; z++)
            {
                DrawTileButton(x, z);
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space(10);
        GUI.color = Color.white;


        if (GUILayout.Button("Toggle Hover Draw"))
            isHoverDrawEnabled = !isHoverDrawEnabled;

        DetectHoverKeyToggle();
    }

    private void DrawTileButton(int x, int z)
    {
        TileType tile = levelGenerator.GetTileTypeAt(x, z);
        GUI.color = GetTileColor(tile);

        if (GUILayout.Button("", GUILayout.Width(tileSize * 2), GUILayout.Height(tileSize * 2)))
        {
            Undo.RecordObject(levelGenerator, "Set Tile Type");
            SetTileAtPosition(x, z);
        }

        Rect buttonRect = GUILayoutUtility.GetLastRect();
        Vector2 mousePos = Event.current.mousePosition;

        if (buttonRect.Contains(mousePos) && isHoverDrawEnabled)
        {
            SetTileAtPosition(x, z);
        }

    }

    private void DrawTileBrushPalette()
    {

        EditorGUILayout.Space(10);
        GUILayout.Label("Tile Brush Palette", EditorStyles.boldLabel);
        DrawBrushRow(TileType.GRASS, TileType.WATER);
        DrawBrushRow(TileType.ROAD, TileType.WAYPOINT);
        DrawBrushRow(TileType.ROCK, TileType.TREE, TileType.SHRUB);

        EditorGUILayout.Space(10);
    }

    private void DrawBrushRow(params TileType[] types)
    {
        EditorGUILayout.BeginHorizontal();
        foreach (TileType type in types)
        {
            if (GUILayout.Button($"Draw {type}", GUILayout.Width(brushSize * 2), GUILayout.Height(brushSize)))
            {
                currentTileType = type;
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawLevelActions()
    {
        GUILayout.Space(20);
        GUILayout.Label("Level Actions", EditorStyles.boldLabel);

        if (GUILayout.Button("Generate Level"))
            levelGenerator.SpawnTiles();

        if (GUILayout.Button("Clear Level"))
            levelGenerator.ClearLevel();

        if (GUILayout.Button("Delete Level"))
            levelGenerator.DeleteLevel();
    }

    private void SetTileAtPosition(int x, int z)
    {
        levelGenerator.SetTileType(x, z, currentTileType);

        if (currentTileType == TileType.WAYPOINT)
        {
            Vector3Int waypoint = new Vector3Int(x, 0, z);
            levelGenerator.AddWaypointPosition(waypoint);
        }

        EditorUtility.SetDirty(levelGenerator);
        Repaint();
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
            case TileType.WAYPOINT:
                return Color.black;
            case TileType.WATER:
                return Color.cyan;
            case TileType.TREE:
                return Color.yellow;
            case TileType.ROCK:
                return Color.grey;
            case TileType.SHRUB:
                return Color.magenta;
        }
    }

    private void DetectHoverKeyToggle()
    {
        Event keyEvent = Event.current;
        if (keyEvent.type == EventType.KeyDown && keyEvent.keyCode == KeyCode.LeftControl)
        {
            isHoverDrawEnabled = !isHoverDrawEnabled;
        }
    }
}
