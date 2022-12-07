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

    private void Awake()
    {
        Instance = this;

        _buildingTypeList = Resources.Load<BuildingTypeListSO>(nameof(BuildingTypeListSO));
    }

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (_activeBuildingType != null && CanSpawnBuilding(_activeBuildingType, UtilsClass.GetMouseWorldPosition()))
            {
                Instantiate(_activeBuildingType.Prefab, UtilsClass.GetMouseWorldPosition(), Quaternion.identity);
            }
        }
    }

    public bool CanSpawnBuilding (BuildingTypeSO buildingType, Vector3 position)
    {
        BoxCollider2D boxCollider2D = buildingType.Prefab.GetComponent<BoxCollider2D>();

        //Rule #1:  Make sure area is clear
        Collider2D[] col2DArray = Physics2D.OverlapBoxAll(position + (Vector3)boxCollider2D.offset, boxCollider2D.size, 0);
        bool isAreaClear = col2DArray.Length == 0;
        if (!isAreaClear) return false;

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
                    return false;
                }
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
                return true;
            }
        }

        //Cannot place buildings
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
}

public class OnActiveBuildingTypeChangeEventArgs : EventArgs
{
    public BuildingTypeSO ActiveBuildingType;
}

