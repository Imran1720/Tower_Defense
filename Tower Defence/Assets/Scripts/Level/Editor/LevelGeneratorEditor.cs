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
    private bool canHoverDraw = false;
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

                Rect buttonRect = GUILayoutUtility.GetLastRect();
                Vector2 mousePosition = Event.current.mousePosition;

                if (buttonRect.Contains(mousePosition) && canHoverDraw)
                {
                    levelGenerator.SetTileValue(i, j, tileTypeToSet);
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

        if (GUILayout.Button("Draw Water", GUILayout.Width(brushButtonSize * 2), GUILayout.Height(brushButtonSize)))
        {
            SetTileToDraw(TileType.WATER);
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(20);
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Draw Road", GUILayout.Width(brushButtonSize * 2), GUILayout.Height(brushButtonSize)))
        {
            SetTileToDraw(TileType.ROAD);
        }

        if (GUILayout.Button("Draw Waypoint", GUILayout.Width(brushButtonSize * 2), GUILayout.Height(brushButtonSize)))
        {
            SetTileToDraw(TileType.WAYPOINT);
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
            SetTileToDraw(TileType.TREE);
        }

        if (GUILayout.Button("Draw Shrub", GUILayout.Width(brushButtonSize * 2), GUILayout.Height(brushButtonSize)))
        {
            SetTileToDraw(TileType.SHRUB);
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(20);

        if (GUILayout.Button("Togle Hover Draw"))
        {
            canHoverDraw = !canHoverDraw;
        }


        if (GUILayout.Button("Generate Level"))
        {
            levelGenerator.SpawnTiles();
        }
        if (GUILayout.Button("Clear Level"))
        {
            levelGenerator.ClearLevel();
        }
        if (GUILayout.Button("Delete Level"))
        {
            levelGenerator.DeleteLevel();
        }

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



    private void InitializeEditorData()
    {
        buttonCountInRow = levelGenerator.GetButtonCountInRow();
        buttonCountInColumn = levelGenerator.GetButtonCountInColumn();
        buttonSize = levelGenerator.GetButtonSize();
        brushButtonSize = levelGenerator.GetBrushButtonSize();
    }
}
