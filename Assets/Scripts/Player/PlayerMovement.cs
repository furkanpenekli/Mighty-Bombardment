using UnityEngine;
using UnityEngine.Serialization;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private MouseInput _mouseInput;
    [FormerlySerializedAs("_cannon")] [SerializeField]
    private GameObject _player;

    [SerializeField]
    private float _cannonSpeed;
    
    private float _cannonAngle;
    private float _cannonWeaponAngle;
    
    private Plane _plane;

    private Cannon _currentCannon;
    
    public void SetCurrentCannon(Cannon newCannon)
    {
        _currentCannon = newCannon;
    }
    private void Start()
    {
        _plane = new Plane(Vector3.up, Vector3.zero);
    }
    
    private void Update()
    {
        RotateCannon();
    }
    
    void RotateCannon()
    {
        float distance;
        if(_plane.Raycast(_mouseInput.GetMouseRay(), out distance)) 
        {
            Vector3 target = _mouseInput.GetMouseRay().GetPoint(distance);
            
            Vector3 direction = target - transform.position;
            float rotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            _player.transform.rotation = Quaternion.Euler(0, rotation, 0);
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position,transform.forward,out hit,100))
        {
            
        }
    }
    public void WeaponRotateUp()//to max
    {
        _currentCannon.WeaponRotateUp();
    }

    public void WeaponRotateDown()//to min
    {
        _currentCannon.WeaponRotateDown();
    }
    void RotateCannonWeapon()
    {
        
    }
}
