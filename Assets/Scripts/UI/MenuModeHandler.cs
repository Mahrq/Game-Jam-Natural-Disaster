using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// Attached to various UI Elements to change the state of the player controller so that
/// other actions can not be performed when clicking through menus.
/// </summary>
public class MenuModeHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = FindObjectOfType<PlayerController>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_playerController.State != PlayerController.ControllerState.BuildMode)
        {
            _playerController.State = PlayerController.ControllerState.MenuMode;
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_playerController.State != PlayerController.ControllerState.BuildMode)
        {
            _playerController.State = _playerController.PreviousState;
        }
    }
}
