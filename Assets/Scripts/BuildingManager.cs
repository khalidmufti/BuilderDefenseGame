using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    private Camera _mainCamera;
    private BuildingTypeListSO _buildingTypeList;
    private BuildingTypeSO _buildingType;

    private void Awake()
    {
        _buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);
        _buildingType = _buildingTypeList.Buildings[0];
    }

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(_buildingType.Prefab, GetMouseWorldPosition(), Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            _buildingType = _buildingTypeList.Buildings[0];
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            _buildingType = _buildingTypeList.Buildings[1];
        }

    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseWorldPoistion = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPoistion.z = 0f;
        return mouseWorldPoistion;
    }
}
