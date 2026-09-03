using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using Mono.Cecil;

public class ObstacleEditorWindow : EditorWindow
{
    private Vector2 scrollingPosition; // Variable used for Scrolling in the menu

    // File to edit
    private ObstacleData targetData;

    //Menubar button creation
    [MenuItem("Tools/Obstacle Editor")]

    public static void OpenWindow()
    {
        // Opening the window and giving a title for the window
        GetWindow<ObstacleEditorWindow>("Grid Editor");
    }

    private void OnGUI()
    {
        // Title
        GUILayout.Label("Grid Configuration" , EditorStyles.boldLabel);

        // Obstacle Data Slot
        targetData = (ObstacleData) EditorGUILayout.ObjectField(
            "Target Data" , targetData, typeof(ObstacleData) , false);

        if (targetData == null)
        {
            //Failsafe
            EditorGUILayout.HelpBox("ObstacleData asset is missing.", MessageType.Info);
            return;
        }

        EditorGUILayout.Space(10);

        // Dimension Controls for changeable grid sizes

        EditorGUI.BeginChangeCheck();
        int changeableWidth = EditorGUILayout.IntField("Width:" , targetData.gridWidth);
        int changeableHeight = EditorGUILayout.IntField("Height:" , targetData.gridHeight);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(targetData , "Change Grid Size");

            //Failsafe mechanism to ensure that the user cannot input the grid values to be 
            // either 0 or a negative number

            targetData.gridHeight = Mathf.Max(1,changeableHeight);
            targetData.gridWidth = Mathf.Max(1,changeableWidth);
            EditorUtility.SetDirty(targetData);


        }

        EditorGUILayout.Space(10);

        // Scrolling functionality 
        scrollingPosition = EditorGUILayout.BeginScrollView(scrollingPosition);

        // Drawing And Rendering the Grid Buttons

        for (int z = targetData.gridHeight -1 ; z >=0; z--)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0 ; x < targetData.gridWidth; x++)
            {
                Vector2Int presentCoordinate = new Vector2Int(x,z);
                bool isObstacle = targetData.Obstacle.Contains(presentCoordinate);
                bool newState = GUILayout.Toggle(isObstacle, $"{x},{z}" , "Button",
                GUILayout.Width(45) , GUILayout.Height(35));


                if (newState != isObstacle)
                {
                    Undo.RecordObject(targetData , "Add Obstacle");
                    if (newState)
                    {
                        targetData.Obstacle.Add(presentCoordinate);
                        
                    }
                    else
                    {
                        targetData.Obstacle.Remove(presentCoordinate);
                    }
                    EditorUtility.SetDirty(targetData);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.Space(10);

        if (GUILayout.Button("Clear Obstacles"))
        {
            Undo.RecordObject(targetData , "Clear Obstacles");
            targetData.Obstacle.Clear();
            EditorUtility.SetDirty(targetData);
        }
    }
}
