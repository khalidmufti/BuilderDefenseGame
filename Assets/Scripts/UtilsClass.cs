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

    public static Vector3 GetRandomDir()
    {
        return new Vector3(
            Random.Range(-1f, 1f), 
            Random.Range(-1f, 1f)
            ).normalized;
    }

    public static float GetAngleFromVector (Vector3 vector)
    {
        return Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
    }
}
