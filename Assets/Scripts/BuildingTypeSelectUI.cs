using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTypeSelectUI : MonoBehaviour
{
    [SerializeField] private Sprite arrowSprite;
    [SerializeField] private List<BuildingTypeSO> _ignoreBuildingTypeList;
    private Dictionary<BuildingTypeSO, Transform> _btnTransformDictionary;
    private Transform _arrowBtn;

    private void Awake()
    {
        Transform btnTemplate = transform.Find("btnTemplate");
        btnTemplate.gameObject.SetActive(false);

        BuildingTypeListSO buildingTypeList = Resources.Load<BuildingTypeListSO>(nameof(BuildingTypeListSO));

        _btnTransformDictionary = new Dictionary<BuildingTypeSO, Transform>();

        int index = 0;

        _arrowBtn = Instantiate(btnTemplate, transform);
        _arrowBtn.gameObject.SetActive(true);

        float offsetAmount = 130f;
        _arrowBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);

        _arrowBtn.Find("image").GetComponent<Image>().sprite = arrowSprite;  
        _arrowBtn.Find("image").GetComponent<RectTransform>().sizeDelta = new Vector2(0,-30f);

        _arrowBtn.GetComponent<Button>().onClick.AddListener(() => {
            BuildingManager.Instance.SetActiveBuildingType(null);
        });

        index++;

        foreach (BuildingTypeSO buildingType in buildingTypeList.Buildings)
        {
            if (_ignoreBuildingTypeList.Contains(buildingType))
            {
                continue;
            }

            Transform btnTransform = Instantiate(btnTemplate, transform);
            btnTransform.gameObject.SetActive(true);

            offsetAmount = 130f;
            btnTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);

            btnTransform.Find("image").GetComponent<Image>().sprite = buildingType.Sprite;

            btnTransform.GetComponent<Button>().onClick.AddListener( () => { 
                BuildingManager.Instance.SetActiveBuildingType(buildingType);
            } );

            _btnTransformDictionary[buildingType] = btnTransform;

            index++;
        }
    }

    private void Start()
    {
        BuildingManager.Instance.OnActiveBuildingTypeChange += BuildingManager_OnActiveBuildingTypeChange;
        UpdateActiveBuildingTypeButton();
    }

    private void BuildingManager_OnActiveBuildingTypeChange(object sender, OnActiveBuildingTypeChangeEventArgs e)
    {
        UpdateActiveBuildingTypeButton();
    }

    private void UpdateActiveBuildingTypeButton()
    {
        _arrowBtn.Find("selected").gameObject.SetActive(false);
        foreach (BuildingTypeSO buildingType in _btnTransformDictionary.Keys)
        {
            Transform btnTransform = _btnTransformDictionary[buildingType];
            btnTransform.Find("selected").gameObject.SetActive(false);
        }

        BuildingTypeSO activeBuildingType = BuildingManager.Instance.GetActiveBuildingType();
        if (activeBuildingType == null)
        {
            _arrowBtn.Find("selected").gameObject.SetActive(true);
        }
        else
        {
            _arrowBtn.Find("selected").gameObject.SetActive(false);
            _btnTransformDictionary[activeBuildingType].Find("selected").gameObject.SetActive(true);
        }
    }
}
