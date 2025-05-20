using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonAnimation : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    
    [SerializeField]
    private Transform _initialPosition;
    [SerializeField]
    private Transform _recoilPosition;

    [SerializeField]
    private GameObject _cannonWeapon;
    
    private bool isMovingBack;
    private void Start()
    {
        isMovingBack = false;
    }
    public void StartRecoil()
    {
        if (!isMovingBack)
        {
            StartCoroutine(Recoil());
        }
    }

    private float _errorMargin = 0.07f;
    public IEnumerator  Recoil()
    {
        isMovingBack = true; // Geri tepme hareketini başlat

        while (Vector3.Magnitude(_cannonWeapon.transform.position - _recoilPosition.position) > _errorMargin)
        {
            float step = _speed * Time.deltaTime;
            _cannonWeapon.transform.position =
                Vector3.MoveTowards(_cannonWeapon.transform.position, _recoilPosition.position, step);
            yield return null;
        }
        
        // Geri dönüş hareketi
        while (Vector3.Magnitude(_cannonWeapon.transform.position - _initialPosition.position) > _errorMargin)
        {
            float step = _speed * Time.deltaTime;
            _cannonWeapon.transform.position =
                Vector3.MoveTowards(_cannonWeapon.transform.position, _initialPosition.position, step);
            yield return null;
        }

        // Hareket tamamlandıktan sonra isMovingBack'i false yaparak fonksiyonu sonlandır
        isMovingBack = false;
    }
}
