using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceGeneratorOverlay : MonoBehaviour 
{
    [SerializeField] private ResourceGenerator _resourceGenerator;

    private Transform _barTransform;

    private void Start() 
    {
        ResourceGeneratorData resourceGeneratorData = _resourceGenerator.GetResourceGeneratorData();

        _barTransform = transform.Find("bar");

        transform.Find("icon").GetComponent<SpriteRenderer>().sprite = resourceGeneratorData.ResourceType.Sprite;

        transform.Find("text").GetComponent<TextMeshPro>().SetText(_resourceGenerator.GetAmountGeneratedPerSecond().ToString("F1"));
    }

    private void Update() 
    {
        _barTransform.localScale = new Vector3(1 - _resourceGenerator.GetTimerNormalized(), 1, 1);
    }

}
