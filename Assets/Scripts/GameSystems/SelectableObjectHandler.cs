using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObjectHandler : MonoBehaviour, ISelectableObject<SelectableObjectHandler>
{
    private BuildModeBlueprintBehaviour _blueprint;
    private ResourceBuildingBehaviour _resource;

    private PlayerController _player;
    private SelectedInfoUI _selectedInfoUI;

    public event Action<SelectableObjectHandler> OnCursorEnter;
    public event Action<SelectableObjectHandler> OnCursorExit;
    public event Action<SelectableObjectHandler> OnMouseLeftClick;

    private void Awake()
    {
        _resource = this.GetComponent<ResourceBuildingBehaviour>();
        _blueprint = this.GetComponent<BuildModeBlueprintBehaviour>();
        _selectedInfoUI = FindObjectOfType<SelectedInfoUI>();
    }
    private void OnMouseEnter()
    {
        OnCursorEnter?.Invoke(this);
    }
    private void OnMouseExit()
    {
        OnCursorExit?.Invoke(this);
    }
    private void OnMouseUp()
    {
        switch (Blueprint.CurrentStage)
        {
            case BuildModeBlueprintBehaviour.BuildingStage.Preparing:
                break;
            case BuildModeBlueprintBehaviour.BuildingStage.Moving:
                break;
            case BuildModeBlueprintBehaviour.BuildingStage.Building:
                break;
            case BuildModeBlueprintBehaviour.BuildingStage.Initialise:
                if (Player.State != PlayerController.ControllerState.BuildMode && Player.State != PlayerController.ControllerState.MenuMode)
                {
                    Player.SelectedBuilding = Blueprint;
                    _selectedInfoUI.SelectedObjectData = this;
                    OnMouseLeftClick?.Invoke(this);
                }
                break;
            default:
                break;
        }
    }
    public PlayerController Player
    {
        get
        {
            if (_player == null)
            {
                _player = FindObjectOfType<PlayerController>();
            }
            return _player;
        }
    }
    public BuildModeBlueprintBehaviour Blueprint
    {
        get
        {
            if (_blueprint == null)
            {
                _blueprint = this.GetComponent<BuildModeBlueprintBehaviour>();
            }
            return _blueprint;
        }
    }
    public ResourceBuildingBehaviour Resource
    {
        get
        {
            return _resource;
        }
    }

}
