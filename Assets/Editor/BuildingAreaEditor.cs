using System;
using System.Collections;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(BuildingArea))]
public class BuildingAreaEditor : Editor
{
    [SerializeField]
    private Texture2D _stoneTowerTexture;
    [SerializeField]
    private Texture2D _highTowerTexture;

    private GUIStyle _deleteBuildingStyle = new GUIStyle();
    GUIContent _deleteBuildingContent = new GUIContent();
    [SerializeField]
    private Texture2D _deleteBuildingTexture;

    private void Awake()
    {
        DeleteBuildingContent();
    }

    private void DeleteBuildingContent()
    {
        _deleteBuildingStyle.imagePosition = ImagePosition.ImageLeft;
        
        _deleteBuildingContent.text = "Delete Current Building";
        _deleteBuildingContent.image = _deleteBuildingTexture;
    }
    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        BuildingArea buildingArea = (BuildingArea)target;
        
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Towers");

        if(GUILayout.Button(_stoneTowerTexture))
        {
            buildingArea.BuildStoneTower();
        }
        if(GUILayout.Button(_highTowerTexture))
        {
            buildingArea.BuildHighTower();
        }
        GUILayout.EndHorizontal();
        
        if (GUILayout.Button(_deleteBuildingContent))
        {
            buildingArea.DeleteCurrentBuilding();
        }
    }
}
