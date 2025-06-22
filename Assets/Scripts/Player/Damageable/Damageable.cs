using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField]
    protected float _maxDamagePoint;
    [SerializeField]
    private float _damagePoint;
    public float damagePoint
    {
        get => _damagePoint;
        protected set => _damagePoint = value;
    }

    private bool _alive = true;
    public bool alive
    {
        get => _alive;
        private set => _alive = value;
    }
    
    [SerializeField]
    private Bar _bar;
    void Start()
    {
        _bar.SetMaxValue(_maxDamagePoint);
        _bar.SetValue(damagePoint);
    }
    void Update()
    {
        damagePoint = Mathf.Clamp(damagePoint, 0, _maxDamagePoint);   
    }
    public void GiveDamage(float damage)
    {
        damagePoint -= damage;
        CheckAlive();
        _bar.SetValue(damagePoint);
    }
    public void GiveDamagePoint(float damagePoint)
    {
        damagePoint += damagePoint;
        CheckAlive();
        _bar.SetValue(damagePoint);
    }

    public bool CheckAlive()
    {
        if (damagePoint <= 0)
        {
            alive = false;
            Destroy();
        }
        else
        {
            alive = true;
        }
        return alive;
    }

    protected virtual void Destroy()
    {
        Destroy(gameObject);   
    }

        public void ResetHealth()
    {
        SetHealth(_maxDamagePoint);
    }

        /// <summary>
    /// Sets the health (damage points).
    /// </summary>
    public void SetHealth(float health)
    {
        damagePoint = health;
        _bar.SetValue(damagePoint);
    }
}
