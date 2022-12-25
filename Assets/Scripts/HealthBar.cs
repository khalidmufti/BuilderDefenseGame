using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private HealthSystem _healthSystem;
    private Transform _barTransform;
    private Transform _separatorContainer;

    private void Awake()
    {
        _barTransform = transform.Find("bar");
    }

    private void Start()
    {
        _separatorContainer = transform.Find("separatorContainer");
        Transform separatorTemplate = _separatorContainer.Find("separatorTemplate");
        separatorTemplate.gameObject.SetActive(false);

        int healthAmountPerSeparator = 10;
        float barSize = 3f;
        float barOneHealthAmountSize = barSize / _healthSystem.GetHealthAmountMax();
        int healthSeparators = Mathf.FloorToInt(_healthSystem.GetHealthAmountMax() / healthAmountPerSeparator);
        for (int i = 0; i < healthSeparators; i++)
        {
            Transform separatorTransform = Instantiate(separatorTemplate, _separatorContainer);
            separatorTransform.gameObject.SetActive(true);
            separatorTransform.localPosition = new Vector3(barOneHealthAmountSize * i * healthAmountPerSeparator + separatorTemplate.localScale.x / 2, 0, 0);
        }

        _healthSystem.OnDamaged += _healthSystem_OnDamaged;
        _healthSystem.OnHealed += _healthSystem_OnHealed;
        UpdateBar();
        UpdateHealthBarVisible();
    }

    private void _healthSystem_OnHealed(object sender, System.EventArgs e)
    {
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

        gameObject.SetActive(true); //testing - remove
    }
    
}
