using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObjectHandler : MonoBehaviour, ISelectableObject<SelectableObjectHandler>
{
    private BuildModeBlueprintBehaviour _blueprint;
    private PlayerController _player;

    public event Action<SelectableObjectHandler> OnCursorEnter;
    public event Action<SelectableObjectHandler> OnCursorExit;
    public event Action<SelectableObjectHandler> OnMouseLeftClick;

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
                if (Player.State != PlayerController.ControllerState.BuildMode)
                {
                    Player.SelectedBuilding = Blueprint;
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
}
