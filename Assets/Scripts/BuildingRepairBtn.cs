using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingRepairBtn : MonoBehaviour
{ 
    [SerializeField] private HealthSystem _healthSystem;
    [SerializeField] private ResourceTypeSO _goldResourceType;

    private void Awake()
    {
        transform.Find("button").GetComponent<Button>().onClick.AddListener( () => 
            {
                int missingHealth = _healthSystem.GetHealthAmountMax() - _healthSystem.GetHealthAmount();
                int repairCost = missingHealth / 2;

                ResourceAmount[] resourceAmountCost = new ResourceAmount[] { new ResourceAmount { ResourceType = _goldResourceType, Amount = repairCost } };

                if (ResourceManager.Instance.CanAfford(resourceAmountCost))
                { //Can afford repairs
                    _healthSystem.HealFull();
                    ResourceManager.Instance.SpendResources(resourceAmountCost);
                }
                else
                {
                    //Cannot afford repairs
                    TooltipUI.Instance.Show("Not enough gold for repairs!", new TooltipUI.TooltipTimer { Timer = 2f });
                }
            });
    }


}
