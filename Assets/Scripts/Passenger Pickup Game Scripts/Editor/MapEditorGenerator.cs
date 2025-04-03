using UnityEngine;
using UnityEditor;
using System.IO;
using PPG;

public class MapEditorGenerator : EditorWindow
{
    private string editorNotes = @"
<size=20><b>▌ MAP EDITOR'S GUIDE</b></size>

<size=16><color=#FF0000><b>WAGON CELLS</b></color></size>
<size=14>• Inactive: Dark gray</size>
<size=14>• Active: Selected color</size>
<size=14>• These are the cells where the wagons will be placed.</size>
<size=14>• Click + assign values from the ""Selected Cell"" field above</size>
<size=14>• The numerical value entered represents which carrier the wagon belongs to.</size>
<size=14>• WARNING! The selected wagons/cells must be placed in order, there must be no cross placement.</size>
<size=14>• For an example, click the upload button and see the test example.</size>

<size=16><color=#80804C><b>PASSENGER CELLS</b></color></size>
<size=14>• Inactive: Dark yellow</size>
<size=14>• Active: Selected color</size>
<size=14>• These are the cells where passengers will appear</size>
<size=14>• Click + assign values from the ""Selected Cell"" field above</size>
<size=14>• Indicates that the number of passengers will appear on the selected cell as the numerical value entered. </size>

<size=16><color=#202020><b>CORNER CELLS</b></color></size>
<size=14>• Black colored cells</size>
<size=14>• Unchangeable</size>
<size=14>• Map boundaries</size>";

//<size=12><i>Last update: " + System.DateTime.Now.ToString("dd.MM.yyyy") + @"</i></size>";

    private Color borderColor = new Color(0.5f, 0.5f, 0.3f);
    private Color cornerColor = new Color(0.12f, 0.12f, 0.12f);
    private Color selectedBorderColor = new Color(0.8f, 0.4f, 0.2f);
    private Color normalCellColor = new Color(0.25f, 0.25f, 0.25f);
    private Color selectedNormalColor = new Color(1f, 0.2f, 0.2f);

    private int gridSizeX = 7;
    private int gridSizeY = 7;
    private float cellSize = 45f;
    private Color gridColor = Color.gray;
    private bool showNumbers = false;
    private bool showIndex = false;
    private bool showValues = true;
    private LevelDataScriptableObject currentGridData;
    private int selectedCellX = -1;
    private int selectedCellY = -1;
    private int editValue = 0;
    private int[] passengerGroupList;
    private EItemColor[] passengerColorList;
    private EItemColor selectedCellColor = EItemColor.Default;
    private Vector2 scrollPosition;

    [MenuItem("Tools/Map Editor")]
    public static void ShowWindow()
    {
        GetWindow<MapEditorGenerator>("Map Editor");
    }

    private void OnEnable()
    {
        CreateNewGrid();
    }

    private void CreateNewGrid()
    {
        currentGridData = ScriptableObject.CreateInstance<LevelDataScriptableObject>();
        currentGridData.Initialize(gridSizeX, gridSizeY);
    }

    private void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        // Grid ayarları
        EditorGUI.BeginChangeCheck();
        gridSizeX = Mathf.Max(3, EditorGUILayout.IntField("X-Axis Dimension", gridSizeX));
        gridSizeY = Mathf.Max(3, EditorGUILayout.IntField("Y-Axis Dimension", gridSizeY));
        if (EditorGUI.EndChangeCheck())
        {
            CreateNewGrid();
            selectedCellX = -1;
            selectedCellY = -1;
        }

        cellSize = EditorGUILayout.Slider("Cell Size", cellSize, 10f, 50f);
        gridColor = EditorGUILayout.ColorField("Cell Color", gridColor);
        showNumbers = EditorGUILayout.Toggle("Show Coordinates", showNumbers);
        showIndex = EditorGUILayout.Toggle("Show Index", showIndex);
        showValues = EditorGUILayout.Toggle("Show Values", showValues);

        // Seçili hücre bilgisi
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Selected Cell", EditorStyles.boldLabel);

        if (selectedCellX >= 0 && selectedCellY >= 0)
        {
            var cell = currentGridData.GetCell(selectedCellX, selectedCellY);
            EditorGUILayout.LabelField($"Location: [{selectedCellX}, {selectedCellY}]");

            EditorGUI.BeginChangeCheck();
            editValue = EditorGUILayout.IntField("Cell Value", cell.Value);

            if (!cell.IsBorder)
            {
                selectedCellColor = (EItemColor)EditorGUILayout.EnumPopup("Cell Color", selectedCellColor);

                if (EditorGUI.EndChangeCheck())
                {
                    currentGridData.SetWagonValue(selectedCellX, selectedCellY, editValue, selectedCellColor);
                    EditorUtility.SetDirty(currentGridData);
                }
            }
            else
            {
                EditorGUILayout.LabelField("Passenger Group");

                if (passengerGroupList == null || passengerGroupList.Length != editValue)
                {
                    passengerGroupList = new int[editValue];
                    passengerColorList = new EItemColor[editValue];
                }

                for (int i = 0; i < editValue; i++)
                {
                    passengerGroupList[i] = EditorGUILayout.IntField($"Group Size {i}", passengerGroupList[i]);
                    passengerColorList[i] = (EItemColor)EditorGUILayout.EnumPopup($"Group Color {i}", passengerColorList[i]);
                }

                if (EditorGUI.EndChangeCheck())
                {
                    currentGridData.SetPassengerValue(selectedCellX, selectedCellY, passengerGroupList, passengerColorList, editValue);
                    EditorUtility.SetDirty(currentGridData);
                }
            }
            
        }
        else
        {
            EditorGUILayout.LabelField("No cells are selected");
        }

        // Grid veri yönetimi
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Level Data Management", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("New Map"))
        {
            CreateNewGrid();
            selectedCellX = -1;
            selectedCellY = -1;
        }

        if (GUILayout.Button("Save"))
        {
            SaveGridData();
        }

        if (GUILayout.Button("Load"))
        {
            LoadGridData();
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {

            // Grid önizleme (sol %75)
            EditorGUILayout.BeginVertical(GUILayout.Width(position.width * 0.5f));
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Map Preview", EditorStyles.boldLabel);
                Rect gridRect = GUILayoutUtility.GetRect(
                    gridSizeX * cellSize + 20,
                    gridSizeX * cellSize + 20,
                    gridSizeY * cellSize + 20,
                    gridSizeY * cellSize + 20
                );
                DrawGrid(gridRect);
                HandleGridInteraction(gridRect);
            }
            EditorGUILayout.EndVertical();

            // Not bölümü (sağ %25)
            EditorGUILayout.BeginVertical(GUILayout.Width(position.width * 0.5f - 10));
            {
                EditorGUILayout.Space(25);

                // Not içeriği
                GUIStyle textStyle = new GUIStyle(EditorStyles.helpBox)
                {
                    richText = true,
                    alignment = TextAnchor.UpperLeft,
                    wordWrap = true,
                    padding = new RectOffset(4, 4, 4, 4),
                    margin = new RectOffset(0, 5, 0, 0)
                };

                EditorGUILayout.TextArea(editorNotes, textStyle,
                    GUILayout.ExpandHeight(true));
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndScrollView();
    }

    private void DrawGrid(Rect rect)
    {
        // Arka plan
        EditorGUI.DrawRect(rect, new Color(0.18f, 0.18f, 0.18f));

        int cellCounter = 1;

        // Hücreler
        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                var cell = currentGridData.GetCell(x, y);
                if (cell == null) continue;

                Rect cellRect = new Rect(
                    rect.x + x * cellSize,
                    rect.y + y * cellSize,
                    cellSize,
                    cellSize
                );

                if(y == 0 || x == 0 || x == gridSizeX - 1 || y == gridSizeY - 1)
                {
                    cell.CellID = y * gridSizeX + x;
                }
                else
                {
                    cell.CellID = cellCounter;
                    cellCounter++;
                }

                Color cellColor;
                if (cell.IsClosed)
                {
                    cellColor = cornerColor;
                }
                else if (cell.IsBorder)
                {
                    if(cell.Value != 0) selectedBorderColor = new Color(0.8f, 0.4f, 0.2f);
                    else selectedBorderColor = new Color(0.5f, 0.5f, 0.3f);
                    cellColor = cell.IsSelected ? selectedBorderColor : borderColor;
                }
                else
                {
                    if (cell.Value != 0) selectedNormalColor = cell.GetColor(cell.WagonColor);
                    else selectedNormalColor = new Color(0.25f, 0.25f, 0.25f);
                    cellColor = cell.IsSelected ? selectedNormalColor : normalCellColor;
                }

                EditorGUI.DrawRect(cellRect, cellColor);

                // Hücre kenarları
                Handles.BeginGUI();
                Handles.color = gridColor;
                Handles.DrawLine(
                    new Vector3(cellRect.x, cellRect.y),
                    new Vector3(cellRect.x + cellSize, cellRect.y)
                );
                Handles.DrawLine(
                    new Vector3(cellRect.x, cellRect.y),
                    new Vector3(cellRect.x, cellRect.y + cellSize)
                );
                Handles.EndGUI();

                // Hücre bilgileri
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.black;
                style.fontSize = 12;
                style.alignment = TextAnchor.MiddleCenter;

                string cellText = "";
                if (showNumbers) cellText += $"{x},{y}\n";
                if (showIndex) cellText += $"{cell.CellID}\n";
                if (showValues) cellText += $"<b>V:{cell.Value}</b>";

                EditorGUI.LabelField(
                    new Rect(
                        cellRect.x + cellSize / 2 - 25,
                        cellRect.y + cellSize / 2 - 10,
                        50, 20
                    ),
                    cellText,
                    style
                );

                // Seçili hücre çerçevesi
                if (x == selectedCellX && y == selectedCellY)
                {
                    Handles.BeginGUI();
                    Handles.color = Color.yellow;
                    Handles.DrawAAPolyLine(3f,
                        new Vector3(cellRect.x, cellRect.y),
                        new Vector3(cellRect.x + cellSize, cellRect.y),
                        new Vector3(cellRect.x + cellSize, cellRect.y + cellSize),
                        new Vector3(cellRect.x, cellRect.y + cellSize),
                        new Vector3(cellRect.x, cellRect.y)
                    );
                    Handles.EndGUI();
                }
            }
        }

        // Dış kenarlık
        Handles.BeginGUI();
        Handles.color = gridColor;
        Handles.DrawLine(
            new Vector3(rect.x, rect.y + gridSizeY * cellSize),
            new Vector3(rect.x + gridSizeX * cellSize, rect.y + gridSizeY * cellSize)
        );
        Handles.DrawLine(
            new Vector3(rect.x + gridSizeX * cellSize, rect.y),
            new Vector3(rect.x + gridSizeX * cellSize, rect.y + gridSizeY * cellSize)
        );
        Handles.EndGUI();
    }

    private void HandleGridInteraction(Rect gridRect)
    {
        Event currentEvent = Event.current;

        if (currentEvent.type == EventType.MouseDown && gridRect.Contains(currentEvent.mousePosition))
        {
            selectedCellX = Mathf.FloorToInt((currentEvent.mousePosition.x - gridRect.x) / cellSize);
            selectedCellY = Mathf.FloorToInt((currentEvent.mousePosition.y - gridRect.y) / cellSize);

            var cell = currentGridData.GetCell(selectedCellX, selectedCellY);
            if (cell != null && !cell.IsClosed)
            {
                editValue = cell.Value;

                if (cell.IsBorder)
                {
                    passengerGroupList = cell.PassengerValue;
                    passengerColorList = cell.PassengerColor;
                }

                Repaint();
            }
            else
            {
                selectedCellX = -1;
                selectedCellY = -1;
            }

            currentEvent.Use();
        }
    }


    private void SaveGridData()
    {
        string defaultName = "LevelData.asset";
        string path = EditorUtility.SaveFilePanel(
            "Load Level Data",
            "Assets\\Scripts\\Passenger Pickup Game Scripts\\Scriptable Objects\\LevelDataStorage",
            defaultName,
            "asset"
        );

        if (string.IsNullOrEmpty(path)) return;

        // Assets klasörü içinde mi kontrolü
        if (!path.StartsWith(Application.dataPath))
        {
            Debug.LogError("You can only save in the Assets folder!");
            return;
        }

        path = "Assets" + path.Substring(Application.dataPath.Length);

        // Aynı dosya varsa üzerine yaz, yoksa yeni oluştur
        LevelDataScriptableObject existingData = AssetDatabase.LoadAssetAtPath<LevelDataScriptableObject>(path);

        if (existingData != null)
        {
            // Mevcut veriyi güncelle
            EditorUtility.CopySerialized(currentGridData, existingData);
            EditorUtility.SetDirty(existingData);
            Debug.Log("Updated current level data: " + path);
        }
        else
        {
            // Yeni veri oluştur
            AssetDatabase.CreateAsset(currentGridData, path);
            Debug.Log("New level data created: " + path);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = currentGridData;
    }

    private void LoadGridData()
    {
        string path = EditorUtility.OpenFilePanel(
            "Load Level Data",
            "Assets\\Scripts\\Passenger Pickup Game Scripts\\Scriptable Objects\\LevelDataStorage",
            "asset"
        );

        if (!string.IsNullOrEmpty(path) && path.StartsWith(Application.dataPath))
        {
            path = "Assets" + path.Substring(Application.dataPath.Length);
            LevelDataScriptableObject loadedData = AssetDatabase.LoadAssetAtPath<LevelDataScriptableObject>(path);

            if (loadedData != null)
            {
                currentGridData = loadedData;
                gridSizeX = currentGridData.gridSizeX;
                gridSizeY = currentGridData.gridSizeY;
                Repaint();
            }
        }
    }


}