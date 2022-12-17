using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private BuildingTypeSO _buildingType;
    private HealthSystem _healthSystem;

    private void Awake()
    {
        _buildingType = GetComponent<BuildingTypeHolder>().BuildingType;
        _healthSystem = GetComponent<HealthSystem>();
        _healthSystem.SetHealthAmountMax(_buildingType.HealthAmountMax, true);
    }

    private void Start()
    {
        _healthSystem.OnDied += _healthSystem_OnDied;
    }

    private void _healthSystem_OnDied(object sender, System.EventArgs e)
    {
        Destroy(gameObject);
    }
}
