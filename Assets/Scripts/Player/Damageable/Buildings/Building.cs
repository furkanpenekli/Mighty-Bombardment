using UnityEngine;
using System.Collections;
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
        DestroyAnimation();
    }

    protected void DestroyWithoutScore()
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
    
        // Notify BuildingManager that this building is destroyed, so spawn point can be freed.
        if (BuildingManager.Instance != null)
        {
            BuildingManager.Instance.RemoveBuilding(this);
        }
        DestroyAnimation();
    }

    public void DestroyAnimation()
    {
        StartCoroutine(DestroyAnimationCoroutine());
        base.Destroy();
    }

    private IEnumerator DestroyAnimationCoroutine()
    {
        float duration = 1.0f;
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + Vector3.down * 5f;

    while (elapsed < duration)
    {
        transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
        elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = endPos;
    }
}