using System;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    
    //public Texture2D cursorTexture;
    //
    //public Texture2D mortarCursorTexture;
    //public Texture2D standartCursorTexture;
    //public Texture2D scatterCursorTexture;
    //public Texture2D explodingCursorTexture;
    
    [SerializeField]//remove
    private Vector3 _mousePosition;
    public Vector3 mousePosition
    {
        get => _mousePosition;
    }
    
    //[SerializeField]
    private Transform _crosshair;

    private Cannon _currentCannon;
    
    public void SetCurrentCannon(Cannon newCannon)
    {
        _currentCannon = newCannon;
    }
    
    private void Update()
    {
        GetMouseRay();
    }
    
    public void Fire()
    {
        _currentCannon.Fire();
    }

    //[SerializeField]
    private Camera _mouseCamera;
    public Ray GetMouseRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Ray ray2 = _mouseCamera.ScreenPointToRay(Input.mousePosition);
        //
        //_mousePosition = ray2.GetPoint(100);
        return ray;;
    }

    private bool _movingMouse = false;
    public bool movingMouse
    {
        get => _movingMouse;
    }
    private Vector3 _lastMouseCoordinate = Vector3.zero;
    private void CheckIfMouseMoving()
    {
        
        Vector3 mouseDelta = mousePosition - _lastMouseCoordinate;

        if (mouseDelta.x < 0)
        {
            _movingMouse = true;
        }
        else
        {
            _movingMouse = false;
        }
        _lastMouseCoordinate = mousePosition;
    }
    public void SetMouseCursor(Texture2D cursorTexture)
    {
        Cursor.SetCursor(cursorTexture, new Vector2(1, 1), CursorMode.ForceSoftware);
    }
}
