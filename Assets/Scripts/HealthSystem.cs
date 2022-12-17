using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDamaged;
    public event EventHandler OnDied;

    [SerializeField] private int _healthAmountMax;
    private int _healthAmount;

    private void Awake()
    {
        _healthAmount = _healthAmountMax;     
    }

    public void Damage (int damageAmount)
    {
        _healthAmount -= damageAmount;
        _healthAmount = Mathf.Clamp(_healthAmount, 0, _healthAmountMax);
        OnDamaged?.Invoke(this, EventArgs.Empty);

        if (isDead())
        {
            OnDied?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool isDead ()
    {
        return _healthAmount <= 0;
    }

    public int GetHealthAmount ()
    {
        return _healthAmount;
    }

    public float GetHealthNormalized()
    {
       return (float)_healthAmount / _healthAmountMax;
    }

    public bool isFullHealth ()
    {
        return _healthAmount == _healthAmountMax;
    }

    public void SetHealthAmountMax(int healthAmountMax, bool updateHealthAmount)
    {
        _healthAmountMax = healthAmountMax;

        if (updateHealthAmount)
        {
            _healthAmount = healthAmountMax;
        }
    }
}
