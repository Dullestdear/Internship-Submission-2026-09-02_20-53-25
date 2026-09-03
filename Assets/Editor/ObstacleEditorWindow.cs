using UnityEngine;
using UnityEditor;

public class ObstacleEditorWindow : EditorWindow
{
    // File to edit
    private ObstacleData targetData;

    //Menubar button creation
    [MenuItem("Tools/Obstacle Editor")]

    public static void OpenWindow()
    {
        // Opening the window and giving a title for the window
        GetWindow<ObstacleEditorWindow>("Grid Editor");
    }

}
