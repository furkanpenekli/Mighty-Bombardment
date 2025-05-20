using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingArea : MonoBehaviour
{
    [SerializeField]
    private Building _currentBuilding;
    public List<Building> buildings = new List<Building>();

    //Buildings
    public void BuildStoneTower()
    {
        Build(buildings[0]);
    }

    public void BuildHighTower()
    {
        Build(buildings[1]);
    }
    private void Build(Building building)
    {
        if (_currentBuilding == null)
        {
            _currentBuilding = 
                Instantiate(building,transform.position,building.transform.rotation,transform);
        }
        else
        {
            DestroyImmediate(_currentBuilding.gameObject);
            _currentBuilding = 
                Instantiate(building,transform.position,building.transform.rotation,transform);
        }
    }
    //Base functions
    public void DeleteCurrentBuilding()
    {
        if (_currentBuilding != null)
        {
            DestroyImmediate(_currentBuilding.gameObject);
            _currentBuilding = null;
        }
        else
        {
            Debug.LogError("You are not have a building in that building area!");
        }
    }
}
