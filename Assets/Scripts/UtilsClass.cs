using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilsClass
{
    private static Camera _mainCamera;

    public static  Vector3 GetMouseWorldPosition()
    {
        if (_mainCamera == null)
        {
            _mainCamera = Camera.main;
        }

        Vector3 mouseWorldPoistion = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPoistion.z = 0f;
        return mouseWorldPoistion;
    }
}
