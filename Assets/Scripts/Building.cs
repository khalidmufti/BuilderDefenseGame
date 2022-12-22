using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private BuildingTypeSO _buildingType;
    private HealthSystem _healthSystem;
    private Transform _buildingDemolishBtn;

    private void Awake()
    {
        _buildingType = GetComponent<BuildingTypeHolder>().BuildingType;
        _healthSystem = GetComponent<HealthSystem>();
        _healthSystem.SetHealthAmountMax(_buildingType.HealthAmountMax, true);
        
        _buildingDemolishBtn = transform.Find("pfBuildingDemolishButton");
        _buildingDemolishBtn?.gameObject.SetActive(false);
    }

    private void Start()
    {
        _healthSystem.OnDied += _healthSystem_OnDied;
    }

    private void _healthSystem_OnDied(object sender, System.EventArgs e)
    {
        Destroy(gameObject);
    }

    private void OnMouseEnter()
    {
        _buildingDemolishBtn?.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        _buildingDemolishBtn?.gameObject.SetActive(false);
    }
}
