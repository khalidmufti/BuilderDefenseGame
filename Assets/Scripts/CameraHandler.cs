
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _vCamera;
    [SerializeField] private PolygonCollider2D cameraBoundsCollider2D;

    private float _orthographicSize;
    private float _targetOrthographicSize;

    private void Start()
    {
        _orthographicSize = _vCamera.m_Lens.OrthographicSize;
        _targetOrthographicSize = _orthographicSize;
    }

    void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = new Vector3(x, y).normalized;
        float moveSpeed = 30f;

        Vector3 movementVector = transform.position + (moveDir * moveSpeed * Time.deltaTime);
        if (cameraBoundsCollider2D.bounds.Contains(movementVector))
        {
            transform.position = movementVector;
        }
    }

    private void HandleZoom()
    {
        float zoomAmount = 2f;
        _targetOrthographicSize += -Input.mouseScrollDelta.y * zoomAmount;

        float minOrthographicSize = 10f;
        float maxOrthographicSize = 30f; 
        _targetOrthographicSize = Mathf.Clamp(_targetOrthographicSize, minOrthographicSize, maxOrthographicSize);

        float zoomSpeed = 5f;
        _orthographicSize = Mathf.Lerp(_orthographicSize, _targetOrthographicSize, Time.deltaTime * zoomSpeed);

        _vCamera.m_Lens.OrthographicSize = _orthographicSize;
    }
}
