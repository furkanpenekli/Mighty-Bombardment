using UnityEngine;

public class Building : Damageable
{
    public WallBuilder.BuildingType buildingType;
    public Transform assignedSpawnPoint;

    [Header("VFX")]
    public string destroyVFXName = "BallExplosion";
    public Vector3 _destroyVFXScale = Vector3.one;

    [Header("Score")]
    [SerializeField]
    private int scoreValue = 10; // Score to be added when destroyed.

    /// <summary>
    /// Sets the building's health (damage points).
    /// </summary>
    public void SetHealth(float health)
    {
        _maxDamagePoint = health;
        damagePoint = health;
    }

    protected override void Destroy()
    {
        // Play destruction VFX.
        if (VFXManager.Instance != null && !string.IsNullOrEmpty(destroyVFXName))
        {
            VFXManager.Instance.PlayVFX(
                destroyVFXName,
                transform.position,
                transform.rotation,
                _destroyVFXScale
            );
        }

        // Add score
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(scoreValue);
        }
    
        // Notify BuildingManager that this building is destroyed, so spawn point can be freed.
        if (BuildingManager.Instance != null)
        {
            BuildingManager.Instance.RemoveBuilding(this);
        }

        base.Destroy();
    }

}