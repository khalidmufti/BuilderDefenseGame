using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class BuildingGhost : MonoBehaviour
{
    private GameObject _spriteGameObject;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteGameObject = transform.Find("sprite").gameObject;
        _spriteRenderer = _spriteGameObject.GetComponent<SpriteRenderer>();    

        Hide();
    }

    private void Start()
    {
        BuildingManager.Instance.OnActiveBuildingTypeChange += BuildingManager_OnActiveBuildingTypeChange;
    }

    private void BuildingManager_OnActiveBuildingTypeChange(object sender, OnActiveBuildingTypeChangeEventArgs e)
    {
        if (e.ActiveBuildingType == null)
        {
            Hide();
        }
        else
        {
            Show(e.ActiveBuildingType.Sprite);
        }
    }

    private void Update()
    {
        BuildingManager buildingManager = BuildingManager.Instance;

        transform.position = UtilsClass.GetMouseWorldPosition();

        // TODO almost the same as in Update of buildingManager
        // this is bad because we repeat ourself twice in update method

        if ((buildingManager.GetActiveBuildingType() != null) && !buildingManager.CanSpawnBuilding(buildingManager.GetActiveBuildingType(), transform.position))
        {
            _spriteRenderer.color = new Color(1f, 0f, 0f, 0.5f);
        }
        else
        {
            _spriteRenderer.color = new Color(0f, 1f, 0f, 0.5f);
        }
    }

    private void Show(Sprite ghostSprite)
    {
        _spriteGameObject.SetActive(true);
        _spriteGameObject.GetComponent<SpriteRenderer>().sprite = ghostSprite;
    }

    private void Hide()
    {
        _spriteGameObject.SetActive(false);
    }
}
