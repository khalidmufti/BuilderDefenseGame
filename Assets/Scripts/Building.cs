using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private BuildingTypeSO _buildingType;
    private HealthSystem _healthSystem;
    private Transform _buildingDemolishBtn;
    private Transform _buildingRepairBtn;

    private void Awake()
    {
        _buildingType = GetComponent<BuildingTypeHolder>().BuildingType;
        _healthSystem = GetComponent<HealthSystem>();
        _healthSystem.SetHealthAmountMax(_buildingType.HealthAmountMax, true);
        
        _buildingDemolishBtn = transform.Find("pfBuildingDemolishButton");
        _buildingDemolishBtn?.gameObject.SetActive(false);
        _buildingRepairBtn = transform.Find("pfBuildingRepairButton");
        _buildingRepairBtn?.gameObject.SetActive(false);
    }

    private void Start()
    {
        _healthSystem.OnDied += _healthSystem_OnDied;
        _healthSystem.OnDamaged += _healthSystem_OnDamaged;
        _healthSystem.OnHealed += _healthSystem_OnHealed;
    }

    private void _healthSystem_OnHealed(object sender, System.EventArgs e)
    {
        if (_healthSystem.isFullHealth())
        {
            HideBuildingRepairButton();
        }
    }

    private void _healthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDamaged);
        ShowBuildingRepairButton();

    }

    private void _healthSystem_OnDied(object sender, System.EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDestroyed);
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

    private void ShowBuildingRepairButton()
    {
        _buildingRepairBtn?.gameObject.SetActive(true);
    }

    private void HideBuildingRepairButton()
    {
        _buildingRepairBtn?.gameObject.SetActive(false);
    }

}
