using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    private ResourceGeneratorData _resourceGeneratorData;

    private float _timer;
    private float _timerMax;

    private void Awake()
    {
        _resourceGeneratorData =  GetComponent<BuildingTypeHolder>().BuildingType.ResourceGeneratorData;
        _timerMax = _resourceGeneratorData.TimerMax;
    }

    private void Start()
    {

        Collider2D[] col2DArray = Physics2D.OverlapCircleAll(transform.position, _resourceGeneratorData.ResourceDetectionRadius);

        int nearbyResourceAmount = 0;
        foreach (Collider2D col2D in col2DArray)
        {
            ResourceNode resourceNode = col2D.GetComponent<ResourceNode>();
            if (resourceNode != null)
            {
                //It is a resource node
                if (resourceNode.ResourceType == _resourceGeneratorData.ResourceType)
                    nearbyResourceAmount++;
            }
        }

        nearbyResourceAmount = Mathf.Clamp(nearbyResourceAmount, 0, _resourceGeneratorData.MaxResourceNodes);
        if (nearbyResourceAmount == 0)
        {
            //No resource nodes nearby so disable resource generator
            enabled = false;
        }

        else
        {
            _timerMax = (_resourceGeneratorData.TimerMax / 2f) + _resourceGeneratorData.TimerMax * ( 1 - (float)nearbyResourceAmount / _resourceGeneratorData.MaxResourceNodes);
        }
        
        Debug.Log("nearbyResourceAmount: " + nearbyResourceAmount + "; _timerMax: " + _timerMax);

    }

    void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer  <= 0f)
        {
            _timer += _timerMax;
            ResourceManager.Instance.AddResource(_resourceGeneratorData.ResourceType, 1);
        }
    }
}
