using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingConstruction : MonoBehaviour
{
    public static BuildingConstruction Create(Vector3 position, BuildingTypeSO buildingType)
    {
        Transform pfBuildingConstruction = Resources.Load<Transform>("pfBuildingConstruction");
        Transform buildingConstructionTransform = Instantiate(pfBuildingConstruction, position, Quaternion.identity);

        BuildingConstruction buildingConstruction = buildingConstructionTransform.GetComponent<BuildingConstruction>();
        buildingConstruction.SetBuildingType(buildingType);

        return buildingConstruction;
    }

    private float _constructionTimer;
    private float _constructionTimerMax;
    private BuildingTypeSO _buildingType;
    private BoxCollider2D _boxCollider2D;
    private SpriteRenderer _spriteRenderer;
    private BuildingTypeHolder _buildingTypeHolder;
    private Material _constructionMaterial;

    private void Awake()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _buildingTypeHolder = GetComponent<BuildingTypeHolder>();   
        _spriteRenderer = transform.Find("sprite").GetComponent<SpriteRenderer>();
        _constructionMaterial = _spriteRenderer.material;
    }

    private void Update()
    {
        _constructionTimer -= Time.deltaTime;

        _constructionMaterial.SetFloat("_Progress", GetConstructionTimerNormalized());

        if (_constructionTimer <= 0f)
        {
            Instantiate(_buildingType.Prefab, transform.position, Quaternion.identity);
            SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingPlaced);
            Destroy(gameObject);
        }
    }

    private void SetBuildingType (BuildingTypeSO buildingType)
    {
        _buildingType = buildingType;
        _constructionTimerMax = buildingType.ConstructionTimerMax;
        _constructionTimer = _constructionTimerMax;
        _buildingTypeHolder.BuildingType = buildingType;            

        _spriteRenderer.sprite = buildingType.Sprite;
        _boxCollider2D.offset = buildingType.Prefab.GetComponent<BoxCollider2D>().offset;
        _boxCollider2D.size = buildingType.Prefab.GetComponent<BoxCollider2D>().size;
    }

    public float GetConstructionTimerNormalized()
    {
        return 1 - _constructionTimer / _constructionTimerMax;
    }
}
