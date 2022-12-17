using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    private ResourceGeneratorData _resourceGeneratorData;

    private float _timer;
    private float _timerMax;

    public static int GetNearbyResourceAmount(ResourceGeneratorData resourceGeneratorData, Vector3 position)
    {
        Collider2D[] col2DArray = Physics2D.OverlapCircleAll(position, resourceGeneratorData.ResourceDetectionRadius);

        int nearbyResourceAmount = 0;
        foreach (Collider2D col2D in col2DArray)
        {
            ResourceNode resourceNode = col2D.GetComponent<ResourceNode>();
            if (resourceNode != null)
            {
                //It is a resource node
                if (resourceNode.ResourceType == resourceGeneratorData.ResourceType)
                    nearbyResourceAmount++;
            }
        }

        nearbyResourceAmount = Mathf.Clamp(nearbyResourceAmount, 0, resourceGeneratorData.MaxResourceNodes);
        return nearbyResourceAmount;
    }

    private void Awake()
    {
        _resourceGeneratorData =  GetComponent<BuildingTypeHolder>().BuildingType.ResourceGeneratorData;
        _timerMax = _resourceGeneratorData.TimerMax;
    }

    private void Start()
    {
        int nearbyResourceAmount = GetNearbyResourceAmount(_resourceGeneratorData, transform.position);

        if (nearbyResourceAmount == 0)
        {
            //No resource nodes nearby so disable resource generator
            enabled = false;
        }

        else
        {
            _timerMax = (_resourceGeneratorData.TimerMax / 2f) + _resourceGeneratorData.TimerMax * ( 1 - (float)nearbyResourceAmount / _resourceGeneratorData.MaxResourceNodes);
        }
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer  <= 0f)
        {
            _timer += _timerMax;
            ResourceManager.Instance.AddResource(_resourceGeneratorData.ResourceType, 1);
        }
    }

    public ResourceGeneratorData GetResourceGeneratorData()
    {
        return _resourceGeneratorData;
    }

    public float GetTimerNormalized ()
    {
        return _timer / _timerMax;
    }

    public float GetAmountGeneratedPerSecond()
    {
        return 1 / _timerMax;
    }
}
