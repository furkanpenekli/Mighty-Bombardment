using System;
using System.Collections.Generic;
using UnityEngine;
public class WallBuilder : MonoBehaviour
{
    public enum BuildingType{wallTower,wall,wallGate}
    public enum BuildAxis{X,Z,negative_X,negative_Z}

    public BuildingType buildingType;
    public BuildAxis buildAxis;
    
    public List<Building> wallTowers = new List<Building>();
    public List<Building> walls = new List<Building>();
    [SerializeField]//remove
    private Vector3 _currentBuildPosition;
    [SerializeField]//remove
    private Building _currentBuilding;
    private Building _lastBuilding;
    [SerializeField] 
    private BuildingList _buildingList;
    private void Awake()
    {
        _currentBuildPosition = transform.position;
    }

    public void BuildStoneWallTower(BuildAxis buildAxis)
    {
        _currentBuilding = wallTowers[0];
        if (buildAxis == BuildAxis.X)
        {
            Build_X(true);
        }
        else if (buildAxis == BuildAxis.Z)
        {
            Build_Z(true);
        }
        else if (buildAxis == BuildAxis.negative_X)
        {
            Build_Negative_X(true);
        }
        else if (buildAxis == BuildAxis.Z)
        {
            Build_Negative_Z(true);
        }
    }
    public void BuildStoneWall(BuildAxis buildAxis)
    {
        _currentBuilding = walls[0];
        if (buildAxis == BuildAxis.X)
        {
            Build_X(false);
        }
        else if (buildAxis == BuildAxis.Z)
        {
            Build_Z(false);
        }
        else if (buildAxis == BuildAxis.negative_X)
        {
            Build_Negative_X(false);
        }
        else if (buildAxis == BuildAxis.negative_Z)
        {
            Build_Negative_Z(false);
        }
    }
    
    private void Build_X(bool keepPosition)
    {
        var building = Build(_currentBuilding,_currentBuildPosition,BuildAxis.X,keepPosition);
        SetPositionAndRotation_X(building.transform);
    }
    private void Build_Negative_X(bool keepPosition)
    {
        var building = Build(_currentBuilding,_currentBuildPosition,BuildAxis.negative_X,keepPosition);
        SetPositionAndRotation_Negative_X(building.transform);
    }
    
    private void Build_Z(bool keepPosition)
    {
        var building = Build(_currentBuilding,_currentBuildPosition,BuildAxis.Z,keepPosition);
        SetPositionAndRotation_Z(building.transform);
    }
    private void Build_Negative_Z(bool keepPosition)
    {
        var building = Build(_currentBuilding,_currentBuildPosition,BuildAxis.negative_Z,keepPosition);
        SetPositionAndRotation_Negative_Z(building.transform);
    }
    
    [SerializeField]
    private Transform X_axis;
    [SerializeField]
    private Vector3 X_axisPosition;
    [SerializeField]
    private Transform negative_X_axis;
    [SerializeField]
    private Vector3 negative_X_axisPosition;
    
    [SerializeField]
    private Transform Z_axis;
    [SerializeField]
    private Vector3 Z_axisPosition;
    [SerializeField]
    private Transform negative_Z_axis;
    [SerializeField]
    private Vector3 negative_Z_axisPosition;
    private void SetPositionAndRotation_X(Transform building)
    {
        building.transform.LookAt(new Vector3(X_axis.position.x,X_axis.position.y,X_axis.position.z));
    }
    private void SetPositionAndRotation_Negative_X(Transform building)
    {
        building.transform.LookAt(new Vector3(negative_X_axis.position.x,negative_X_axis.position.y,negative_X_axis.position.z));
    }
    
    private void SetPositionAndRotation_Z(Transform building)
    {
        building.transform.LookAt(new Vector3(Z_axis.position.x,Z_axis.position.y,Z_axis.position.z));
    }
    private void SetPositionAndRotation_Negative_Z(Transform building)
    {
        building.transform.LookAt(new Vector3(negative_Z_axis.position.x,negative_Z_axis.position.y,negative_Z_axis.position.z));
    }
    
    private Building Build(Building building, Vector3 position, BuildAxis buildAxis,bool keepPosition)
    {
        var _building = Instantiate(building,position,transform.rotation);
        _buildingList.AddBuilding(_building);
        _lastBuilding = _building;
        if (!keepPosition)
        {
            if (buildAxis == BuildAxis.X)
            {
                _currentBuildPosition += new Vector3(building.transform.localScale.x, 0, 0);
                X_axis.transform.position = 
                    new Vector3(X_axis.transform.position.x, X_axis.transform.position.y, _currentBuildPosition.z);
            }
            else if (buildAxis == BuildAxis.Z)
            {
                _currentBuildPosition += new Vector3(0, 0, building.transform.localScale.z);
                Z_axis.transform.position =
                    new Vector3(_currentBuildPosition.x,Z_axis.transform.position.y,Z_axis.transform.position.z);
            }
            
            else if (buildAxis == BuildAxis.negative_X)
            {
                _currentBuildPosition -= new Vector3(building.transform.localScale.x, 0, 0);
                negative_X_axis.transform.position = 
                    new Vector3(negative_X_axis.transform.position.x, negative_Z_axis.transform.position.y, _currentBuildPosition.z);
            }
            else if (buildAxis == BuildAxis.negative_Z)
            {
                _currentBuildPosition -= new Vector3(0, 0, building.transform.localScale.z);
                negative_Z_axis.transform.position =
                    new Vector3(_currentBuildPosition.x,negative_Z_axis.transform.position.y,negative_Z_axis.transform.position.z);
            }
        }
        _building.transform.parent =  _buildingList.transform;
        return _building;
    }

    public void DeleteLastBuilding()
    {
        bool keepPosition;
        if (_lastBuilding.buildingType == BuildingType.wallTower)
        {
            keepPosition = true;
        }
        else
        {
            keepPosition = false;
        }
        
        if (!keepPosition && _buildingList.GetBuildingCount() > 1)
        {
            if (Math.Abs(_lastBuilding.transform.position.x
                         - _buildingList.GetBuilding(_buildingList.GetBuildingCount() - 1).transform.position.x) > 1)
            {
                _currentBuildPosition -= new Vector3(_lastBuilding.transform.localScale.x, 0, 0);
                X_axis.transform.position = 
                    new Vector3(X_axis.transform.position.x, X_axis.transform.position.y, _currentBuildPosition.z);
            }
            else if (Math.Abs(_lastBuilding.transform.position.z 
                              - _buildingList.GetBuilding(_buildingList.GetBuildingCount() - 1).transform.position.z) > 1)
            {
                _currentBuildPosition -= new Vector3(0, 0, _lastBuilding.transform.localScale.z);
                Z_axis.transform.position =
                    new Vector3(_currentBuildPosition.x,Z_axis.transform.position.y,Z_axis.transform.position.z);
            }
        }
        _buildingList.RemoveLastBuilding();
        DestroyImmediate(_lastBuilding.gameObject);
        if (_buildingList.GetLastBuilding() != null)
        {
            _lastBuilding = _buildingList.GetLastBuilding();
        }
        
        if (_buildingList.GetBuildingCount() < 1)
        {
            _lastBuilding = null;
            ResetBuildPosition();
        }
    }
    public void ResetBuildPosition()
    {
        _currentBuildPosition = transform.position;
        
        X_axis.transform.position = X_axisPosition;
        Z_axis.transform.position = Z_axisPosition;

        negative_X_axis.position = negative_X_axisPosition;
        negative_Z_axis.position = negative_Z_axisPosition;
    }
}
