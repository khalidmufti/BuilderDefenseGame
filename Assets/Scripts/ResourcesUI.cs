using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourcesUI : MonoBehaviour
{
    private ResourceTypeListSO _resourceTypeList;
    private Dictionary<ResourceTypeSO, Transform> _resourceTypeTransformDicitonary;

    private void Awake()
    {
        _resourceTypeList = Resources.Load<ResourceTypeListSO>(nameof(ResourceTypeListSO));
        _resourceTypeTransformDicitonary = new Dictionary<ResourceTypeSO, Transform>();            

        Transform resourceTemplate = transform.Find("resourceTemplate");
        resourceTemplate.gameObject.SetActive(false);

        int index = 0;
        foreach (ResourceTypeSO resourceType in _resourceTypeList.NaturalResources)
        {
            Transform resourceTransform = Instantiate(resourceTemplate, transform);
            resourceTransform.gameObject.SetActive(true);

            float offsetAmount = -160f;
            resourceTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);
            resourceTransform.Find("image").GetComponent<Image>().sprite = resourceType.Sprite;

            _resourceTypeTransformDicitonary[resourceType] = resourceTransform;    

            index++;
        }

    }

    private void Start()
    {
        ResourceManager.Instance.OnResourceAmountChanged += ResourceManagerOnResourceAmountChanged;
        UpdateResourceAmount();        
    }

    private void ResourceManagerOnResourceAmountChanged(object sender, System.EventArgs e)
    {
        UpdateResourceAmount();
    }

    private void UpdateResourceAmount()
    {
        foreach (ResourceTypeSO resourceType in _resourceTypeList.NaturalResources)
        {
            Transform resourceTransform = _resourceTypeTransformDicitonary[resourceType];

            int resourceAmount = ResourceManager.Instance.GetResourceAmount(resourceType);
            resourceTransform.Find("text").GetComponent<TextMeshProUGUI>().SetText(resourceAmount.ToString());
        }
    }
}
