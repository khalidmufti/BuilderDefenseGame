using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private HealthSystem _healthSystem;
    private Transform _barTransform;

    private void Awake()
    {
        _barTransform = transform.Find("bar");
    }

    private void Start()
    {
        _healthSystem.OnDamaged += _healthSystem_OnDamaged;
        UpdateBar();
        UpdateHealthBarVisible();
    }

    private void _healthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        UpdateBar();
        UpdateHealthBarVisible();
    }

    private void UpdateBar ()
    {
        _barTransform.localScale = new Vector3(_healthSystem.GetHealthNormalized(), 1, 1);
    }

    private void UpdateHealthBarVisible()
    {
        if (_healthSystem.isFullHealth())
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
    
}
