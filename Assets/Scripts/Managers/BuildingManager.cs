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

    [Header("SPAWN COOLDOWN")]
    [SerializeField] private float spawnPointCooldown = 3f; // Cooldown time per spawn point.

    private Dictionary<Transform, float> spawnPointCooldowns = new Dictionary<Transform, float>();
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
            spawnPointCooldowns[point] = 0f; // Initially ready to spawn.
        }
    }

    private bool HasAvailableSpawnPoint()
    {
        float currentTime = Time.time;
        foreach (var kvp in spawnPointCooldowns)
        {
            if (kvp.Value <= currentTime)
                return true;
        }
        return false;
    }

    private Transform GetRandomAvailableSpawnPoint()
    {
        float currentTime = Time.time;
        List<Transform> availablePoints = new List<Transform>();
        
        foreach (var kvp in spawnPointCooldowns)
        {
            if (kvp.Value <= currentTime)
                availablePoints.Add(kvp.Key);
        }

        return availablePoints.Count > 0 ? 
               availablePoints[Random.Range(0, availablePoints.Count)] : null;
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
            spawnPointCooldowns[spawnPoint] = float.MaxValue; // Mark as occupied.
        }
    }

    private void SetupBuilding(Building building)
    {
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
            {
                // Start cooldown before this spawn point can be used again.
                spawnPointCooldowns[building.assignedSpawnPoint] = Time.time + spawnPointCooldown;
            }
            
            activeBuildings.Remove(building);
        }
    }
}
