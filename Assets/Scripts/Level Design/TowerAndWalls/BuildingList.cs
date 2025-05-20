using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingList : MonoBehaviour
{
    [SerializeField]
    private List<Building> _buildings;

    public Building GetLastBuilding()
    {
        return GetBuilding(GetBuildingCount());
    }
    public int GetBuildingCount()
    {
        return _buildings.Count;
    }
    
    public Building GetBuilding(int index)
    {
        index--;
        if (_buildings.Contains(_buildings[index]))
        {
            return _buildings[index];
        }
        return null;
    }
    
    public void AddBuilding(Building building)
    {
        _buildings.Add(building);
    }
    public void RemoveLastBuilding()
    {
        RemoveBuilding(GetBuildingCount() - 1);
    }
    public void RemoveBuilding(int index)
    {
        if (_buildings.Contains(_buildings[index]))
        {
            _buildings.Remove(_buildings[index]);
        }
        else
        {
            Debug.LogError("Building list not have this building");
        }
    }
}
