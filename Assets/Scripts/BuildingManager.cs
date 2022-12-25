using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { private set; get; }

    public event EventHandler<OnActiveBuildingTypeChangeEventArgs> OnActiveBuildingTypeChange;

    private Camera _mainCamera;
    private BuildingTypeListSO _buildingTypeList;
    private BuildingTypeSO _activeBuildingType;

    [SerializeField] private Building _hqBuilding; 

    private void Awake()
    {
        Instance = this;

        _buildingTypeList = Resources.Load<BuildingTypeListSO>(nameof(BuildingTypeListSO));
    }

    private void Start()
    {
        _mainCamera = Camera.main;

        _hqBuilding.GetComponent<HealthSystem>().OnDied += HQ_OnDied;
    }

    private void HQ_OnDied(object sender, EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.GameOver);
        GameOverUI.Instance.Show();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (_activeBuildingType != null)
            {
                if (CanSpawnBuilding(_activeBuildingType, UtilsClass.GetMouseWorldPosition(), out String errorMsg))
                {
                    if (ResourceManager.Instance.CanAfford(_activeBuildingType.ConstructionResourceCostArray))
                    {
                        ResourceManager.Instance.SpendResources(_activeBuildingType.ConstructionResourceCostArray);
                        //Instantiate(_activeBuildingType.Prefab, UtilsClass.GetMouseWorldPosition(), Quaternion.identity);
                        BuildingConstruction.Create(UtilsClass.GetMouseWorldPosition(), _activeBuildingType);
                        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingPlaced);
                    }

                    else
                    {
                        TooltipUI.Instance.Show("Cannot afford " + _activeBuildingType.GetConstructionResourceCostString(),new TooltipUI.TooltipTimer { Timer = 2f });
                    }
                }

                else
                {
                    TooltipUI.Instance.Show(errorMsg, new TooltipUI.TooltipTimer { Timer = 2f });
                }
            }
        }
    }

    public bool CanSpawnBuilding (BuildingTypeSO buildingType, Vector3 position, out String errorMessage)
    {
        BoxCollider2D boxCollider2D = buildingType.Prefab.GetComponent<BoxCollider2D>();

        //Rule #1:  Make sure area is clear
        Collider2D[] col2DArray = Physics2D.OverlapBoxAll(position + (Vector3)boxCollider2D.offset, boxCollider2D.size, 0);
        bool isAreaClear = col2DArray.Length == 0;
        if (!isAreaClear)
        {
            errorMessage = "Area is not clear!";
            return false;
        }

        //Rule #2:  Make sure same type of building is not too close - rule for minimum construction radius in which same building cannot be placed
        col2DArray = Physics2D.OverlapCircleAll(position, buildingType.MinConstructionRadius);
        foreach (Collider2D col in col2DArray)
        {
            //Collisions inside the construction radius
            BuildingTypeHolder buildingHolder = col.GetComponent<BuildingTypeHolder>();
            if ( buildingHolder != null)
            {
                //Building has a BuildingTypeHolder
                if (buildingHolder.BuildingType == buildingType)
                {
                    //Already a building of this type in the minimum construction radius, so cannot build again.
                    errorMessage = "Too close to another building of the same type!";
                    return false;
                }
            }
        }
        
        //Not allow placement of building ig no nearby resources for that building type
        if (buildingType.isGenerateResource)
        {
            ResourceGeneratorData resourceData = buildingType.ResourceGeneratorData;
            int nearbyResourceAmount = ResourceGenerator.GetNearbyResourceAmount(resourceData, position);
            if (nearbyResourceAmount == 0)
            {
                errorMessage = "No needed resources nearby!";
                return false;
            }
        }

        //Rule #3:  Make sure building is not placed too far from other buildings - rule building within max construction radius of other buildings
        float maxConstructionDistance = 25f;
        col2DArray = Physics2D.OverlapCircleAll(position, maxConstructionDistance);
        foreach (Collider2D col in col2DArray)
        {
            //Collisions inside the construction radius
            BuildingTypeHolder buildingHolder = col.GetComponent<BuildingTypeHolder>();
            if (buildingHolder != null)
            {
                //Found another building so can put this building as within max construction radius of other buildings
                errorMessage = "";
                return true;
            }
        }

        //Cannot place buildings
        errorMessage = "Too far from any other building!";
        return false;
    }


    public void SetActiveBuildingType(BuildingTypeSO buildingType)
    {
        _activeBuildingType = buildingType;
        OnActiveBuildingTypeChange?.Invoke(this, new OnActiveBuildingTypeChangeEventArgs { ActiveBuildingType = buildingType });
    }

    public BuildingTypeSO GetActiveBuildingType()
    {
        return _activeBuildingType;
    }

    public Building getHQBuilding()
    {
        return _hqBuilding;
    }

}

public class OnActiveBuildingTypeChangeEventArgs : EventArgs
{
    public BuildingTypeSO ActiveBuildingType;
}
