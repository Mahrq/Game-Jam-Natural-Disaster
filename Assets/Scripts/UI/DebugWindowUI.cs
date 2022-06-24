using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DebugWindowUI : MonoBehaviour
{
    [SerializeField]
    private Transform _camTransform;
    [SerializeField]
    private Transform _camTargetTransform;
    [SerializeField]
    private Text _txtPlayerControllerState;
    [SerializeField]
    private Text _txtCameraDistanceFromTarget;
    private PlayerController _playerController;


    private void Awake()
    {
        _playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        float camDistance = Vector3.Distance(_camTargetTransform.position, _camTransform.position);
        _txtPlayerControllerState.text = $"Controller State: {_playerController.State}";
        _txtCameraDistanceFromTarget.text = "Camera Distance From Target: " + camDistance.ToString("F00");
    }
}
