using System;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private CannonBall _cannonBall;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _fireRate;
    [SerializeField] private float gravity = -18f;

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
    private CannonAnimation _cannonAnimation;
    
    
    [SerializeField] private bool debugPath;
    [SerializeField] private int pathResolution = 30;

    private void Start()
    {
        _timer = 0f;
        _fire = false;
        _cannonAnimation = GetComponent<CannonAnimation>();
    }

    private void Update()
    {
        if (Time.time >= _timer)
            _fire = true;

        if (debugPath)
            DrawPath();
    }


    public void Fire()
    {
        if (!_fire) return;

        if (_cannonAnimation != null)
            _cannonAnimation.StartRecoil();

        VFXManager.Instance.PlayVFX("FireImpact1", _firePoint.position, _firePoint.rotation, _fireVFXScale);

        CannonBall cannonBall = Instantiate(_cannonBall, _firePoint.position, _firePoint.rotation);
        Rigidbody rb = cannonBall.GetComponent<Rigidbody>();
        
        Launch(rb, _crosshair.position);

        _timer = Time.time + 1f / _fireRate;
        _fire = false;
    }

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

    private void RepositionCrosshair(Vector3 direction)
    {
        _crosshair.Translate(direction);
    }

    public void WeaponRotateUp()
    {
        float rotationY = _cannonWeapon.transform.localEulerAngles.y;
        Debug.Log("Angle: " + rotationY + "weapon rotating up.");
        if (rotationY > 180f) rotationY -= 360f;

        if (rotationY < _maxWeaponAngle)
        {
            _cannonWeapon.transform.Rotate(new Vector3(0, _rotateAmount, 0));
            RepositionCrosshair(Vector3.forward * _crosshairPositionAmount);
            height = Mathf.Min(height + heightStep, maxHeight);
        }
    }

    public void WeaponRotateDown()
    {
        float rotationY = _cannonWeapon.transform.localEulerAngles.y;
        Debug.Log("Angle: " + rotationY + "weapon rotating down.");
        if (rotationY > 180f) rotationY -= 360f;

        if (rotationY > _minWeaponAngle)
        {
            _cannonWeapon.transform.Rotate(new Vector3(0, -_rotateAmount, 0));
            RepositionCrosshair(Vector3.back * _crosshairPositionAmount);
            height = Mathf.Max(height - heightStep, minHeight);
        }
    }
    
    private void DrawPath()
    {
        Vector3 startPos = _firePoint.position;
        Vector3 targetPos = _crosshair.position;

        float displacementY = targetPos.y - startPos.y;
        Vector3 displacementXZ = new Vector3(targetPos.x - startPos.x, 0, targetPos.z - startPos.z);

        float time = Mathf.Sqrt(-2 * height / gravity) +
                     Mathf.Sqrt(2 * (displacementY - height) / gravity);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * height);
        Vector3 velocityXZ = displacementXZ / time;

        Vector3 initialVelocity = velocityXZ + velocityY * -Mathf.Sign(gravity);

        Vector3 previousDrawPoint = startPos;

        for (int i = 1; i <= pathResolution; i++)
        {
            float simulationTime = i / (float)pathResolution * time;

            Vector3 displacement = initialVelocity * simulationTime +
                                   Vector3.up * gravity * simulationTime * simulationTime / 2f;

            Vector3 drawPoint = startPos + displacement;
            Debug.DrawLine(previousDrawPoint, drawPoint, Color.yellow);
            previousDrawPoint = drawPoint;
        }
    }

}
