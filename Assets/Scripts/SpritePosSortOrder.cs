using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SpritePosSortOrder : MonoBehaviour
{
    [SerializeField] private bool _isRunOnce;
    [SerializeField] private float _precisionMultiplier = 5f; //5 precision levels per sorting layer.  Note capped at 2 pow 16 => -32000 to + 32000 ish
    private SortingGroup _sortingGroup;

    private void Awake()
    {
        _sortingGroup = GetComponent<SortingGroup>();
    }

    private void LateUpdate()
    {
        _sortingGroup.sortingOrder = (int)(-transform.position.y * _precisionMultiplier);

        if (_isRunOnce)
        {
            Destroy(this);
        }    
    }
}
