using System.Collections.Generic;
using UnityEngine;
public class CannonMenu : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement _playerMovement;
    [SerializeField]
    private MouseInput _mouseInput;
    
    [SerializeField]
    private List<Cannon> _cannons = new List<Cannon>();
    [SerializeField]
    private Cannon currentCannon;
    
    [SerializeField]
    private List<GameObject> _crosshairs = new List<GameObject>();
    [SerializeField]
    private GameObject _currentCrosshair;
    private void Awake()
    {
        SetCurrentCannon();
    }

    public void SetCurrentCannon(int number)
    {
        SetCurrentCrosshair(number);
        currentCannon.gameObject.SetActive(false);
        currentCannon = _cannons[number];
        currentCannon.gameObject.SetActive(true);
        SetCurrentCannon();
    }
    private void SetCurrentCannon()
    {
        _playerMovement.SetCurrentCannon(currentCannon);
        _mouseInput.SetCurrentCannon(currentCannon);
    }

    private void SetCurrentCrosshair(int number)
    {
        _currentCrosshair.SetActive(false);
        _currentCrosshair = _crosshairs[number];
        _currentCrosshair.SetActive(true);
    }
}
