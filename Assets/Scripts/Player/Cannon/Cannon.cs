using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(CannonAnimation))]
public class Cannon : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private CannonAnimation _cannonAnimation;
    
    [SerializeField] private CannonBall _cannonBall;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _fireRate;
    private float gravity = -9.81f;

    [SerializeField] private float height = 25f;
    [SerializeField] private float minHeight = 10f;
    [SerializeField] private float maxHeight = 50f;
    [SerializeField] private float heightStep = 5f;

    [SerializeField] private GameObject _cannonWeapon;
    [SerializeField] private Transform _crosshair;
    [SerializeField] private float _rotateAmount = 2f;
    [SerializeField] private float _crosshairPositionAmount = 0.2f;
    [SerializeField] private Vector3 _fireVFXScale;
    
    [SerializeField] private float _minWeaponAngle;
    [SerializeField] private float _maxWeaponAngle;
    
    private float _timer;
    private bool _fire;
    
    [SerializeField] private bool debugPath;
    [SerializeField] private int pathResolution = 30;

    /// <summary>
    /// Initialize component references and set initial cannon rotation to the minimum angle.
    /// </summary>
    private void Start()
    {
        _timer = 0f;
        _fire = false;
        _cannonAnimation = GetComponent<CannonAnimation>();
        _lineRenderer = GetComponent<LineRenderer>();
        
        // Set the weapon angle to minimum weapon angle variable.
        Vector3 currentEuler = _cannonWeapon.transform.localEulerAngles;
        currentEuler.y = _minWeaponAngle;
        _cannonWeapon.transform.localEulerAngles = currentEuler;
    }

    /// <summary>
    /// Handles firing timing, debug path drawing, and clears path when debug is off.
    /// </summary>
    private void Update()
    {
        if (Time.time >= _timer)
            _fire = true;
    
        if (debugPath)
            DrawPath();
        else if (_lineRenderer != null)
            _lineRenderer.positionCount = 0; // Clear the trajectory line when debug off.
    }

    /// <summary>
    /// Fires the cannonball if the fire timer allows it, starts recoil animation and visual effects.
    /// </summary>
    public void Fire()
    {
        if (!_fire) return;

        if (_cannonAnimation != null)
            _cannonAnimation.StartRecoil();

        VFXManager.Instance.PlayVFX("FireImpact1", _firePoint.position, _firePoint.rotation, _fireVFXScale);
        AudioManager.Instance.PlaySFX(AudioManager.SFX_SHOOT);

        CannonBall cannonBall = Instantiate(_cannonBall, _firePoint.position, _firePoint.rotation);
        Rigidbody rb = cannonBall.GetComponent<Rigidbody>();
        
        Launch(rb, _crosshair.position);

        _timer = Time.time + 1f / _fireRate;
        _fire = false;
    }

    /// <summary>
    /// Calculates and sets the initial velocity of the cannonball to hit the target based on projectile physics.
    /// </summary>
    /// <param name="rb">Rigidbody of the cannonball to launch</param>
    /// <param name="targetPos">Target position to hit</param>
    private void Launch(Rigidbody rb, Vector3 targetPos)
    {
        Vector3 startPos = _firePoint.position;
        float displacementY = targetPos.y - startPos.y;
        Vector3 displacementXZ = new Vector3(targetPos.x - startPos.x, 0, targetPos.z - startPos.z);

        float time = Mathf.Sqrt(-2 * height / gravity) +
                     Mathf.Sqrt(2 * (displacementY - height) / gravity);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * height);
        Vector3 velocityXZ = displacementXZ / time;

        Vector3 finalVelocity = velocityXZ + velocityY * -Mathf.Sign(gravity);

        Physics.gravity = Vector3.up * gravity;
        rb.useGravity = true;
        rb.linearVelocity = finalVelocity;
    }

    /// <summary>
    /// Moves the crosshair by the given direction vector.
    /// </summary>
    /// <param name="direction">Direction vector to move the crosshair</param>
    private void RepositionCrosshair(Vector3 direction)
    {
        _crosshair.Translate(direction);
    }

    /// <summary>
    /// Rotates the cannon weapon upwards within allowed angle limits and adjusts crosshair and firing height accordingly.
    /// </summary>
    public void WeaponRotateUp()
    {
        float rotationY = _cannonWeapon.transform.localEulerAngles.y;
        if (rotationY > 180f) rotationY -= 360f;

        if (rotationY < _maxWeaponAngle)
        {
            _cannonWeapon.transform.Rotate(new Vector3(0, _rotateAmount, 0));
            RepositionCrosshair(Vector3.forward * _crosshairPositionAmount);
            height = Mathf.Min(height + heightStep, maxHeight);
        }
    }

    /// <summary>
    /// Rotates the cannon weapon downwards within allowed angle limits and adjusts crosshair and firing height accordingly.
    /// </summary>
    public void WeaponRotateDown()
    {
        float rotationY = _cannonWeapon.transform.localEulerAngles.y;
        if (rotationY > 180f) rotationY -= 360f;

        if (rotationY > _minWeaponAngle)
        {
            _cannonWeapon.transform.Rotate(new Vector3(0, -_rotateAmount, 0));
            RepositionCrosshair(Vector3.back * _crosshairPositionAmount);
            height = Mathf.Max(height - heightStep, minHeight);
        }
    }
    
    /// <summary>
    /// Draws the projectile's predicted trajectory path using LineRenderer based on current firing parameters.
    /// </summary>
    private void DrawPath()
    {
        if (_lineRenderer == null) return;
    
        Vector3 startPos = _firePoint.position;
        Vector3 targetPos = _crosshair.position;
    
        float displacementY = targetPos.y - startPos.y;
        Vector3 displacementXZ = new Vector3(targetPos.x - startPos.x, 0, targetPos.z - startPos.z);
    
        float time = Mathf.Sqrt(-2 * height / gravity) +
                     Mathf.Sqrt(2 * (displacementY - height) / gravity);
    
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * height);
        Vector3 velocityXZ = displacementXZ / time;
    
        Vector3 initialVelocity = velocityXZ + velocityY * -Mathf.Sign(gravity);
    
        int resolution = pathResolution;
        _lineRenderer.positionCount = resolution + 1;
    
        for (int i = 0; i <= resolution; i++)
        {
            float simulationTime = i / (float)resolution * time;
            Vector3 displacement = initialVelocity * simulationTime +
                                   Vector3.up * gravity * simulationTime * simulationTime / 2f;
            Vector3 drawPoint = startPos + displacement;
            _lineRenderer.SetPosition(i, drawPoint);
        }
    }
}
