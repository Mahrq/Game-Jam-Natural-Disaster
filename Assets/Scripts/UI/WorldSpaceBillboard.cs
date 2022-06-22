using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WorldSpaceBillboard : MonoBehaviour
{
    private Canvas _canvas;
    private Camera _mainCamera;
    private Transform _cameraTransform;

    private Transform _thisTransform;

    private Quaternion _originalRotation;

    private void Awake()
    {
        _canvas = this.GetComponent<Canvas>();
        _mainCamera = Camera.main;
        _cameraTransform = _mainCamera.GetComponent<Transform>();
        _thisTransform = this.GetComponent<Transform>();
        _canvas.worldCamera = _mainCamera;
    }

    private void Start()
    {
        _originalRotation = _thisTransform.rotation;
    }

    private void LateUpdate()
    {
        _thisTransform.rotation = _cameraTransform.rotation * _originalRotation;
    }
}
