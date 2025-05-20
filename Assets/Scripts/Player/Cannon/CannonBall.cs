using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CannonBall : MonoBehaviour
{
   [SerializeField]
   private float _damageAmount;
   
   [Header("Ground Detection")]
   [SerializeField]//remove
   private bool _isGrounded;
   [SerializeField]
   private float _groundRadius;
   [SerializeField]
   private LayerMask _groundLayerMask;
   
   [Header("Building Detection")]
   [SerializeField]//remove
   private bool _onBuilding;
   [SerializeField]
   private float _buildingDetectRadius;
   [SerializeField]
   private LayerMask _buildingLayerMask;

   private void Start()
   {
      _isGrounded = false;
      _onBuilding = false;
   }

   protected void Update()
   {
         CheckIsGrounded();
   }

   private void CheckIsGrounded()
   {
      _isGrounded = Physics.CheckSphere
         (transform.position, _groundRadius, _groundLayerMask);
      
      if (_isGrounded)
      {
         Destroy();
      }
   }
   private void CheckOnBuilding()
   {
      _onBuilding = Physics.CheckSphere
         (transform.position, _buildingDetectRadius, _buildingLayerMask);

      if (_onBuilding)
      {
         Destroy();
      }
   }
   
   private void OnDrawGizmos()
   {
      Gizmos.DrawSphere(transform.position, _groundRadius);
   }

   [SerializeField] 
   private Vector3 _destroyVFXScale;
   private void Destroy()
   {
      Debug.Log("destroyed ball");
      VFXManager.Instance.PlayVFX("Explosion", transform.position, transform.rotation, _destroyVFXScale);
      Destroy(gameObject);
   }
   
   [SerializeField] 
   private GameObject _decalProjectorPrefab;
   void OnCollisionEnter(Collision collision)
   {
      Debug.Log("OnCollisionEnter called");
      
      //ContactPoint contact = collision.contacts[0];
      //Vector3 decalPosition = contact.point;
      //Quaternion decalRotation = Quaternion.LookRotation(contact.normal);
      //
      //// "DecalProjector" nesnesinin konumunu ve rotasyonunu güncelle
      //var decal = Instantiate(_decalProjectorPrefab);
      //decal.transform.position = decalPosition;
      //decal.transform.rotation = decalRotation;
      
      Damageable _damageable = collision.gameObject.GetComponentInParent<Damageable>(true);
      if (_damageable != null)
      {
         Debug.Log("Damaged: " + _damageable.gameObject.name + " " + _damageAmount);
         _damageable.GiveDamage(_damageAmount);
      }
      CheckOnBuilding();
   }
   private void OnTriggerEnter(Collider other)
   {
      //Damageable _damageable = other.GetComponentInParent<Damageable>(true);
      //if (_damageable != null)
      //{
      //   Debug.Log("Damaged: " + _damageable.gameObject.name + " " + _damageAmount);
      //   _damageable.GiveDamage(_damageAmount);
      //}
   }
}
