using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;

    [Header("SPAWN SETTINGS")]
    public List<Transform> spawnPoints = new List<Transform>();
    public GameObject[] buildingPrefabs;
    public Transform buildingsParent;
    public float spawnInterval = 3f;
    public float riseDuration = 1.5f;

    [Header("BUILDING HEALTH")]
    public float minHealth = 30f;
    public float maxHealth = 100f;

    private Dictionary<Transform, bool> spawnPointStatus = new Dictionary<Transform, bool>();
    private List<Building> activeBuildings = new List<Building>();
    private float nextSpawnTime;

    void Awake()
    {
        Instance = this;
        InitializeSpawnPoints();
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime && HasAvailableSpawnPoint())
        {
            SpawnBuilding();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    private void InitializeSpawnPoints()
    {
        foreach (Transform point in spawnPoints)
        {
            spawnPointStatus[point] = false;
        }
    }

    private void SpawnBuilding()
    {
        Transform spawnPoint = GetRandomAvailableSpawnPoint();
        if (spawnPoint == null) return;

        GameObject newBuilding = Instantiate(
            buildingPrefabs[Random.Range(0, buildingPrefabs.Length)],
            spawnPoint.position,
            spawnPoint.rotation,
            buildingsParent
        );

        Building building = newBuilding.GetComponent<Building>();
        if (building != null)
        {
            building.assignedSpawnPoint = spawnPoint;
            SetupBuilding(building);
            activeBuildings.Add(building);
            spawnPointStatus[spawnPoint] = true;
        }
    }

    private Transform GetRandomAvailableSpawnPoint()
    {
        List<Transform> availablePoints = new List<Transform>();
        
        foreach (var point in spawnPointStatus)
        {
            if (!point.Value)
                availablePoints.Add(point.Key);
        }

        return availablePoints.Count > 0 ? 
               availablePoints[Random.Range(0, availablePoints.Count)] : null;
    }

    private bool HasAvailableSpawnPoint()
    {
        return spawnPointStatus.ContainsValue(false);
    }

    private void SetupBuilding(Building building)
    {
        float health = Random.Range(minHealth, maxHealth);
        building.Initialize(health);
        StartCoroutine(RiseBuildingAnimation(building.transform));
    }

    private IEnumerator RiseBuildingAnimation(Transform building)
    {
        Vector3 targetScale = building.localScale;
        building.localScale = new Vector3(targetScale.x, 0.01f, targetScale.z);
        
        float elapsed = 0f;
        while (elapsed < riseDuration)
        {
            building.localScale = Vector3.Lerp(
                building.localScale,
                targetScale,
                elapsed / riseDuration
            );
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        building.localScale = targetScale;
    }

    public void RemoveBuilding(Building building)
    {
        if (activeBuildings.Contains(building))
        {
            if (building.assignedSpawnPoint != null)
                spawnPointStatus[building.assignedSpawnPoint] = false;
            
            activeBuildings.Remove(building);
        }
    }
}