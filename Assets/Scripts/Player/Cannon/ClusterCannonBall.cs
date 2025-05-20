using UnityEngine;

/// <summary>
/// A specialized cannonball that explodes in mid-air when reaching a certain height,
/// spawning multiple regular cannonballs.
/// </summary>
public class ClusterCannonBall : CannonBall
{
    [Header("Cluster Settings")]
    [SerializeField] private float explodeHeight = 20f;           // Height at which the ball will split
    [SerializeField] private int clusterCount = 5;                // Number of cannonballs to spawn
    [SerializeField] private float spreadForce = 5f;              // Force applied to each new ball
    [SerializeField] private GameObject cannonballPrefab;         // The cannonball prefab to spawn

    private bool hasExploded = false;

    private void Update()
    {
        base.Update();

        // Check if the object has reached the height to explode
        CheckExplodeHeight();
    }

    /// <summary>
    /// Checks whether the cannonball has reached the explode height and triggers the explosion if not yet done.
    /// </summary>
    private void CheckExplodeHeight()
    {
        if (!hasExploded && transform.position.y >= explodeHeight)
        {
            Explode();
        }
    }

    /// <summary>
    /// Spawns multiple new cannonballs in different directions and destroys the original.
    /// </summary>
    private void Explode()
    {
        hasExploded = true;

        for (int i = 0; i < clusterCount; i++)
        {
            // Random spread around a forward+up direction
            Vector3 spreadDirection = (transform.forward + Vector3.up).normalized;
            spreadDirection += new Vector3(
                Random.Range(-0.5f, 0.5f),
                Random.Range(0f, 0.5f),
                Random.Range(-0.5f, 0.5f)
            );
            spreadDirection.Normalize();

            GameObject newBall = Instantiate(cannonballPrefab, transform.position, Quaternion.identity);
            Rigidbody rb = newBall.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddForce(spreadDirection * spreadForce, ForceMode.Impulse);
            }
        }

        VFXManager.Instance.PlayVFX("Explosion", transform.position, transform.rotation, Vector3.one);

        Destroy(gameObject);
    }

}
