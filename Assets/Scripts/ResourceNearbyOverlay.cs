using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceNearbyOverlay : MonoBehaviour 
{

    private ResourceGeneratorData _resourceGeneratorData;

    private void Awake() 
    {
        Hide();
    }

    private void Update() 
    {
        int nearbyResourceAmount = ResourceGenerator.GetNearbyResourceAmount(_resourceGeneratorData, transform.parent.position);
        float percent = Mathf.RoundToInt((float)nearbyResourceAmount / _resourceGeneratorData.MaxResourceNodes * 100f);
        transform.Find("text").GetComponent<TextMeshPro>().SetText(percent + "%");
    }

    public void Show(ResourceGeneratorData resourceGeneratorData) 
    {
        _resourceGeneratorData = resourceGeneratorData;
        gameObject.SetActive(true);
        transform.Find("icon").GetComponent<SpriteRenderer>().sprite = resourceGeneratorData.ResourceType.Sprite;
    }

    public void Hide() 
    {
        gameObject.SetActive(false);
    }

}
