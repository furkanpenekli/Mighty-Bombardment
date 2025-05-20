using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WallBuilder))]

public class WallBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        WallBuilder wallBuilder = (WallBuilder)target;
        GUILayout.BeginHorizontal();
        //EditorGUILayout.LabelField("Build:");
        if (GUILayout.Button("Delete Last Building"))
        {
            wallBuilder.DeleteLastBuilding();
        }
        switch (wallBuilder.buildingType)
        {
            case WallBuilder.BuildingType.wallTower:
                switch (wallBuilder.buildAxis)
                {
                    case WallBuilder.BuildAxis.X:
                        if (GUILayout.Button("Build Stone Tower"))
                        { 
                            wallBuilder.BuildStoneWallTower(WallBuilder.BuildAxis.X);
                        }
                        break;
                    case WallBuilder.BuildAxis.negative_X:
                        if (GUILayout.Button("Build Stone Tower"))
                        { 
                            wallBuilder.BuildStoneWallTower(WallBuilder.BuildAxis.negative_X);
                        }
                        break;
                    case WallBuilder.BuildAxis.Z:
                        if (GUILayout.Button("Build Stone Tower"))
                        { 
                            wallBuilder.BuildStoneWallTower(WallBuilder.BuildAxis.Z);
                        }
                        break;
                    case WallBuilder.BuildAxis.negative_Z:
                        if (GUILayout.Button("Build Stone Tower"))
                        { 
                            wallBuilder.BuildStoneWallTower(WallBuilder.BuildAxis.negative_Z);
                        }
                        break;
                }
                break;
            case WallBuilder.BuildingType.wall:
                switch (wallBuilder.buildAxis)
                {
                    case WallBuilder.BuildAxis.X:
                        if (GUILayout.Button("Build Stone Wall"))
                        { 
                            wallBuilder.BuildStoneWall(WallBuilder.BuildAxis.X);
                        }
                        break;
                    case WallBuilder.BuildAxis.negative_X:
                        if (GUILayout.Button("Build Stone Wall"))
                        { 
                            wallBuilder.BuildStoneWall(WallBuilder.BuildAxis.negative_X);
                        }
                        break;
                    case WallBuilder.BuildAxis.Z:
                        if (GUILayout.Button("Build Stone Wall"))
                        {
                            wallBuilder.BuildStoneWall(WallBuilder.BuildAxis.Z);
                        }
                        break;
                    case WallBuilder.BuildAxis.negative_Z:
                        if (GUILayout.Button("Build Stone Wall"))
                        { 
                            wallBuilder.BuildStoneWall(WallBuilder.BuildAxis.negative_Z);
                        }
                        break;
                }
                break;
            case WallBuilder.BuildingType.wallGate:
                switch (wallBuilder.buildAxis)
                {
                    case WallBuilder.BuildAxis.X:
                        if (GUILayout.Button("Build Stone Wall Gate"))
                        { 
                         
                        }
                        break;
                    case WallBuilder.BuildAxis.negative_X:
                        if (GUILayout.Button("Build Stone Wall Gate"))
                        { 
                         
                        }
                        break;
                    case WallBuilder.BuildAxis.Z:
                        if (GUILayout.Button("Build Stone Wall Gate"))
                        { 
                         
                        }
                        break;
                    case WallBuilder.BuildAxis.negative_Z:
                        if (GUILayout.Button("Build Stone Wall Gate"))
                        { 
                         
                        }
                        break;
                }
                break;
        }
        GUILayout.EndHorizontal();
        
        GUILayout.BeginVertical();
        if (GUILayout.Button("Reset create building position"))
        {
            wallBuilder.ResetBuildPosition();
        }
        GUILayout.EndVertical();
    }
}
