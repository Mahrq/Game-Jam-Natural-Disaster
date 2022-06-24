using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Behaviour for the camera controls.
/// Has basic RTS functionality such as panning and key input movement.
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Camera _mainCamera;
    private Transform _thisTransform;
    private Transform _camTransform;

    private float _hInput;
    private float _vInput;
    private float _scrollInput;
    private bool _moveInputPressed => _hInput != 0f || _vInput != 0f;
    [SerializeField]
    private KeyCode _cameraAcceleratorButton = KeyCode.LeftShift;
    [SerializeField]
    [Range(0.1f, 5f)]
    private float _accelerationMultiplier = 2f;
    private bool _acceleratorOn => Input.GetKey(_cameraAcceleratorButton);
    [SerializeField]
    [Range(0.1f, 200f)]
    private float _panSpeed = 20f;
    [SerializeField]
    [Tooltip("Measurement in pixels how close the cursor has to be to trigger mouse panning")]
    [Range(1f, 20f)]
    private float _panBorderThreshold = 10f;
    [SerializeField]
    [Tooltip("The border in this context holds the min and max values for both horizontal movement(x) and vertical movement(y).")]
    private Vector2 _cameraBorderLimit = Vector2.zero;
    [SerializeField]
    [Range(1f, 10000f)]
    private float _zoomSpeed = 50f;
    [SerializeField]
    [Tooltip("Min Zoom (x), Max Zoom (y)")]
    private Vector2 _zoomLimit = Vector2.zero;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _camTransform = _mainCamera.GetComponent<Transform>();
        //Initalise the border if it's not defined in the inspector.
        if (_cameraBorderLimit == Vector2.zero || _zoomLimit == Vector2.zero)
        {
            _cameraBorderLimit = new Vector2(200, 200);
            _zoomLimit = new Vector2(50, 200);
            Debug.LogWarning("Camera bounds/Zoom Limit were not defined in the inspector for CameraController." +
                "\nUsing default values.");
        }
    }
    private void Update()
    {
        //Camera Movement
        _hInput = Input.GetAxisRaw("Horizontal");
        _vInput = Input.GetAxisRaw("Vertical");
        _scrollInput = Input.GetAxisRaw("Mouse ScrollWheel");
        //KeyPress takes precedent over mouse panning.
        if (_moveInputPressed)
        {
            ThisTransform.Translate(KeyPressMovement());
        }
        else
        {
            ThisTransform.Translate(MousePanMovement(Input.mousePosition));
        }
        //Camera Clamping
        ThisTransform.position = ClampToBorder(_cameraBorderLimit, ThisTransform.position);
    }
    private void LateUpdate()
    {
        CameraZoom(_scrollInput);
    }
    private Vector3 KeyPressMovement()
    {
        float acceleration = 1f;
        if (_acceleratorOn)
        {
            acceleration = _accelerationMultiplier;
        }
        Vector3 direction = new Vector3(_hInput, 0f, _vInput) * _panSpeed * acceleration * Time.deltaTime;
        return direction;
    }

    private Vector3 MousePanMovement(Vector3 mousePosition)
    {
        float acceleration = 1f;
        if (_acceleratorOn)
        {
            acceleration = _accelerationMultiplier;
        }
        Vector3 direction = Vector3.zero;
        if (mousePosition.y >= Screen.height - _panBorderThreshold)
        {
            direction = Vector3.forward;
        }
        if (mousePosition.y <= _panBorderThreshold)
        {
            direction = Vector3.back;
        }
        if (mousePosition.x >= Screen.width - _panBorderThreshold)
        {
            direction = Vector3.right;
        }
        if (mousePosition.x <=  _panBorderThreshold)
        {
            direction = Vector3.left;
        }
        return direction * _panSpeed * acceleration * Time.deltaTime;
    }
    private void CameraZoom(float zInput)
    {
        float acceleration = 1f;
        if (_acceleratorOn)
        {
            acceleration = _accelerationMultiplier;
        }
        Vector3 camDirection = _camTransform.InverseTransformDirection(_camTransform.forward);
        Vector3 destination = camDirection.normalized;
        destination *= zInput * acceleration * _zoomSpeed * Time.deltaTime;
        _camTransform.Translate(destination);
        //Zoom Clamp
        float distanceFromLookTarget = Vector3.Distance(_thisTransform.position, _camTransform.position);
        //If further than max do a zoom in correction.
        if (distanceFromLookTarget > _zoomLimit.y)
        {
            destination.Normalize();
            destination *= -(distanceFromLookTarget - _zoomLimit.y);
            _camTransform.Translate(destination);
        }
        //If closer than min do a zoom out correction.
        if (distanceFromLookTarget < _zoomLimit.x)
        {
            destination.Normalize();
            destination *= -(_zoomLimit.x - distanceFromLookTarget);
            _camTransform.Translate(destination);
        }
    }
    private Vector3 ClampToBorder(Vector2 border, Vector3 objectPosition)
    {
        Vector3 clampedPosition = objectPosition;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -border.x, border.x);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, -border.y, border.y);
        return clampedPosition;
    }

    public Transform ThisTransform
    {
        get
        {
            if (_thisTransform == null)
            {
                _thisTransform = this.GetComponent<Transform>();
            }
            return _thisTransform;
        }
    }
}
