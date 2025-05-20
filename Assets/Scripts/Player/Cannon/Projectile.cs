using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.WSA;

public class Projectile : MonoBehaviour
{
    public Rigidbody _ball;
    
    [SerializeField]
    private Transform _target;
    public Transform target{set{ _target = value;}}
    
    public float heightAmount;
    public float height = 25;
    public float minHeight = 25;
    public float maxHeight = 25;
    
    [SerializeField]
    private float gravity = -18;

    public bool debugPath;
    void Awake()
    {
        //_ball.useGravity = false;
    }
    
    void FixedUpdate()
    {
        if (debugPath)
        {
            DrawPath();
        }
    }
    
    public void Launch()
    {
        Physics.gravity = Vector3.up * gravity;
        _ball.useGravity = true;
        _ball.linearVelocity = CalculateLaunchData().initialVelocity;
    }

    private void DrawPath()
    {
        LaunchData launchData = CalculateLaunchData();
        Vector3 previousDrawPoint = _ball.position;

        int resolution = 30;
        for (int i = 1; i <= resolution; i++)
        {
            float simulationTime = i / (float)resolution * launchData.timeToTarget;
            
            Vector3 displacement =
                launchData.initialVelocity * 
                simulationTime + Vector3.up * gravity * simulationTime * simulationTime / 2f;
            //
            Vector3 drawPoint = _ball.position + displacement;
            Debug.DrawLine(previousDrawPoint, drawPoint);
            
            previousDrawPoint = drawPoint;
        }
    }
    
    private LaunchData CalculateLaunchData()
    {
        float displacementY = _target.position.y - _ball.position.y;
        Vector3 displacementXZ = new Vector3
                (_target.position.x - _ball.position.x,
                0,
                _target.position.z - _ball.position.z);
        
        float time = Mathf.Sqrt(-2 * height / gravity)
                      + Mathf.Sqrt(2 * (displacementY - height) / gravity);
        
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * height);
        Vector3 velocityXZ = displacementXZ / time;
        
        return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
    }

    struct LaunchData
    {
        public readonly Vector3 initialVelocity;
        public readonly float timeToTarget;

        public LaunchData(Vector3 initialVelocity, float timeToTarget)
        {
            this.initialVelocity = initialVelocity;
            this.timeToTarget = timeToTarget;
        }
    }

    public void SetHeight(float height)
    {
        this.height = height;
    }
}
