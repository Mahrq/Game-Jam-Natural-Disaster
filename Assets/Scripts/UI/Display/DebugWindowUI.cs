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
    [SerializeField]
    private Text _txtPreservedTimerListAmount;
    private PlayerController _playerController;
    private SelectedDisplayButtonsUI _buttonDisplay;

    private void Awake()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _buttonDisplay = FindObjectOfType<SelectedDisplayButtonsUI>();
    }

    private void Update()
    {
        float camDistance = Vector3.Distance(_camTargetTransform.position, _camTransform.position);
        _txtPlayerControllerState.text = $"Controller State: {_playerController.State}";
        _txtCameraDistanceFromTarget.text = "Camera Distance From Target: " + camDistance.ToString("F00");
        _txtPreservedTimerListAmount.text = $"Preserved Timers Count: {_buttonDisplay.PreservedTimers.Count}";
    }
}
