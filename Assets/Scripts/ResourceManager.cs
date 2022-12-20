using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set;}

    public event EventHandler OnResourceAmountChanged;

    [SerializeField] private List<ResourceAmount> _startingResourcesAmountList;

    private Dictionary<ResourceTypeSO, int> _resourceAmountDictionary;



    private void Awake()
    {
        Instance = this;

        _resourceAmountDictionary = new Dictionary<ResourceTypeSO, int>();

        ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(nameof(ResourceTypeListSO));

        foreach (ResourceTypeSO resourceType in resourceTypeList.NaturalResources)
        {
            _resourceAmountDictionary[resourceType] = 0;
        }

        foreach (ResourceAmount amount in _startingResourcesAmountList)
        {
            AddResource(amount.ResourceType, amount.Amount);
        }
    }

    public void AddResource(ResourceTypeSO resourceType, int amount)
    {
        _resourceAmountDictionary[resourceType] += amount;
        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetResourceAmount(ResourceTypeSO resourceType)
    {
        return _resourceAmountDictionary[resourceType];
    }

    public void SpendResources(ResourceAmount[] resourceAmountArray)
    {
        foreach (ResourceAmount resourceAmount in resourceAmountArray)
        {
            _resourceAmountDictionary[resourceAmount.ResourceType] -= resourceAmount.Amount;
        }
    }

    public bool CanAfford(ResourceAmount[] resourceAmountArray)
    {
        foreach (ResourceAmount resourceAmount in resourceAmountArray)
        {
            if (GetResourceAmount(resourceAmount.ResourceType) >= resourceAmount.Amount )
            {
                continue;        
            }

            else
            {
                return false;
            }
        }

        return true;
    }

    private void TESTLogResourceAmountDictionary()
    {
        foreach (ResourceTypeSO resourceType in _resourceAmountDictionary.Keys)
        {
            Debug.Log(resourceType.ResourceName + ": " + _resourceAmountDictionary[resourceType]);
        }
    }
}
