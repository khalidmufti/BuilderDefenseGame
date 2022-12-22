using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingDemolishBtn : MonoBehaviour
{ 
    [SerializeField] private Building _building;

    private void Awake()
    {
        transform.Find("button").GetComponent<Button>().onClick.AddListener( () => 
            {
                BuildingTypeSO buildingType = _building.GetComponent<BuildingTypeHolder>().BuildingType;

                foreach (ResourceAmount resourceAmount in buildingType.ConstructionResourceCostArray)
                {
                    //Return 60% of cost of building
                    ResourceManager.Instance.AddResource(resourceAmount.ResourceType, Mathf.FloorToInt(resourceAmount.Amount * 0.6f)); 
                }

                Destroy(_building.gameObject);
            });
    }
}
