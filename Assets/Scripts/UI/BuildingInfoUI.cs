using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Extensions;
/// <summary>
/// Behaviour for mapping the current building information to various UI elements.
/// </summary>
public class BuildingInfoUI : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField]
    private GameObject _healthBar;
    [SerializeField]
    private Image _imgHealthBar;

    private BuildModeBlueprintBehaviour _blueprint;
    private BuildingDamageHandler _damageHandler;
    private ISelectableObject<SelectableObjectHandler> _selectHandler;

    private void Awake()
    {
        _blueprint = this.GetComponentInParent<BuildModeBlueprintBehaviour>();

        _damageHandler = this.GetComponentInParent<BuildingDamageHandler>();
        _selectHandler = this.GetComponentInParent<ISelectableObject<SelectableObjectHandler>>();
    }
    private void OnEnable()
    {
        _healthBar.SetActive(false);
        _damageHandler.OnValueChanged += MapHealthToUI;
        _blueprint.OnInitialise += OnInitialiseCallback;
        _blueprint.OnSelectionChanged += OnSelectionChangedCallback;

        _selectHandler.OnCursorEnter += OnCursorEnterCallback;
        _selectHandler.OnCursorExit += OnCursorExitCallback;
        _selectHandler.OnMouseLeftClick += OnMouseLeftClickCallback;
    }
    private void OnDisable()
    {
        _damageHandler.OnValueChanged -= MapHealthToUI;
        _blueprint.OnInitialise -= OnInitialiseCallback;
        _blueprint.OnSelectionChanged -= OnSelectionChangedCallback;

        _selectHandler.OnCursorEnter -= OnCursorEnterCallback;
        _selectHandler.OnCursorExit -= OnCursorExitCallback;
        _selectHandler.OnMouseLeftClick -= OnMouseLeftClickCallback;
    }

    private void MapHealthToUI(Vector3 input)
    {
        float result = MathUtilities.GetPercentage(input);
        _imgHealthBar.fillAmount = result;
    }
    private void OnInitialiseCallback()
    {

    }

    private void OnCursorEnterCallback(SelectableObjectHandler sender)
    {
        switch (_blueprint.CurrentStage)
        {
            case BuildModeBlueprintBehaviour.BuildingStage.Preparing:
                break;
            case BuildModeBlueprintBehaviour.BuildingStage.Moving:
                break;
            case BuildModeBlueprintBehaviour.BuildingStage.Building:
                break;
            case BuildModeBlueprintBehaviour.BuildingStage.Initialise:
                if (sender.Player.State == PlayerController.ControllerState.Free)
                {
                    if (!_healthBar.activeInHierarchy)
                    {
                        _healthBar.SetActive(true);
                    }
                }
                break;
            default:
                break;
        }
    }
    private void OnCursorExitCallback(SelectableObjectHandler sender)
    {
        switch (_blueprint.CurrentStage)
        {
            case BuildModeBlueprintBehaviour.BuildingStage.Preparing:
                break;
            case BuildModeBlueprintBehaviour.BuildingStage.Moving:
                break;
            case BuildModeBlueprintBehaviour.BuildingStage.Building:
                break;
            case BuildModeBlueprintBehaviour.BuildingStage.Initialise:
                if (sender.Player.State == PlayerController.ControllerState.Free)
                {
                    if (_healthBar.activeInHierarchy)
                    {
                        _healthBar.SetActive(false);
                    }
                }
                break;
            default:
                break;
        }
    }
    private void OnMouseLeftClickCallback(SelectableObjectHandler sender)
    {
        switch (_blueprint.CurrentStage)
        {
            case BuildModeBlueprintBehaviour.BuildingStage.Preparing:
                break;
            case BuildModeBlueprintBehaviour.BuildingStage.Moving:
                break;
            case BuildModeBlueprintBehaviour.BuildingStage.Building:
                break;
            case BuildModeBlueprintBehaviour.BuildingStage.Initialise:
                if (!_healthBar.activeInHierarchy)
                {
                    _healthBar.SetActive(true);
                }
                break;
            default:
                break;
        }
    }

    private void OnSelectionChangedCallback(bool isSelected)
    {
        switch (_blueprint.CurrentStage)
        {
            case BuildModeBlueprintBehaviour.BuildingStage.Preparing:
                break;
            case BuildModeBlueprintBehaviour.BuildingStage.Moving:
                break;
            case BuildModeBlueprintBehaviour.BuildingStage.Building:
                break;
            case BuildModeBlueprintBehaviour.BuildingStage.Initialise:
                if (!isSelected)
                {
                    if (_healthBar.activeInHierarchy)
                    {
                        _healthBar.SetActive(false);
                    }
                }
                break;
            default:
                break;
        }
    }

}
