using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Extensions;
/// <summary>
/// Displays the information of the currently selected object onto various UI elements on the canvas
/// </summary>
public class SelectedInfoUI : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField]
    private Image _imgProfilePicture;
    [SerializeField]
    private GameObject _profileDetailsHolder;
    [SerializeField]
    private Text _txtProfileName;
    [SerializeField]
    private Text _txtProfileHealthValue;
    [SerializeField]
    private Text _txtProfileWorkerCount;
    [SerializeField]
    private Image _imgProfileHealthBar;
    [SerializeField]
    private Image _imgProfileEconomyIcon;
    [SerializeField]
    private Text _txtCurrentResourceAmount;
    [SerializeField]
    private Sprite[] _economyIcons;
    [SerializeField]
    private Color[] _economyIconColours;
    private Sprite _defaultProfilePicture;

    private PlayerController _player;
    private SelectableObjectHandler _selectedObjectData;
    private BuildModeBlueprintBehaviour _selectedBuilding;
    private ResourceBuildingBehaviour _selectedBuildingResource;
    private SelectedDisplayButtonsUI _buttonDisplayer;

    private void Awake()
    {
        _defaultProfilePicture = _imgProfilePicture.sprite;
        _player = FindObjectOfType<PlayerController>();
        _buttonDisplayer = this.GetComponent<SelectedDisplayButtonsUI>();
    }
    private void OnEnable()
    {
        if (_profileDetailsHolder.activeInHierarchy)
        {
            _profileDetailsHolder.SetActive(false);
        }
        _player.OnDeselectedBuilding += OnDeselectedBuildingCallback;
    }
    private void OnDisable()
    {
        _player.OnDeselectedBuilding -= OnDeselectedBuildingCallback;
    }
    private void OnSelectedBuildingCallback(SelectableObjectHandler data)
    {
        _selectedBuilding = data.Blueprint;
        _selectedBuildingResource = data.Resource;
        if (_selectedBuilding != null)
        {
            BuildingActions currentActions = _selectedBuilding.BuildProperties.AvailableActions;
            //To update UI while the building is still selected.
            _selectedBuilding.DamageHandler.OnValueChanged += MapHealthToUI;
            //Auto Deselect the buillding when it is destroyed if it's still selected.
            _selectedBuilding.DamageHandler.OnDeath += OnDeselectedBuildingCallback;
            //Change the picture
            _imgProfilePicture.sprite = _selectedBuilding.BuildProperties.ProfilePicture;
            //Map the details to the panel
            Vector3 healthData = new Vector3(_selectedBuilding.DamageHandler.CurrentHealth, 0, _selectedBuilding.BuildProperties.MaxHealth);
            //Map the name
            _txtProfileName.text = _selectedBuilding.BuildProperties.Name;
            //Map the health to health bar
            MapHealthToUI(healthData);
            //Toggle the action buttons
            _buttonDisplayer.ToggleButtons(currentActions);
            if (_selectedBuildingResource != null)
            {
                //resource Data: current workers(x), worker cap(y), resources left (z)
                Vector3 resourceData = new Vector3(_selectedBuildingResource.CurrentWorkers, 
                                                    _selectedBuildingResource.R_Properties.WorkerCap,
                                                    _selectedBuildingResource.CurrentResourceAmount);
                //Map the workers and resource left
                MapResourceToUI(resourceData, _selectedBuildingResource.ResourceLabel);
            }
            else
            {
                _txtProfileWorkerCount.gameObject.SetActive(false);
                _txtCurrentResourceAmount.gameObject.SetActive(false);
            }

            if (!_profileDetailsHolder.activeInHierarchy)
            {
                _profileDetailsHolder.SetActive(true);
            }
        }
    }
    private void OnDeselectedBuildingCallback()
    {
        ReturnToDefault();
    }
    private void ReturnToDefault()
    {
        _buttonDisplayer.ClearButtons();
        _selectedBuilding.DamageHandler.OnValueChanged -= MapHealthToUI;
        _selectedBuilding.DamageHandler.OnDeath -= OnDeselectedBuildingCallback;
        _imgProfilePicture.sprite = _defaultProfilePicture;
        _selectedBuilding = null;
        _selectedBuildingResource = null;
        if (_profileDetailsHolder.activeInHierarchy)
        {
            _profileDetailsHolder.SetActive(false);
        }
    }

    private void MapHealthToUI(Vector3 health)
    {
        float result = MathUtilities.GetPercentage(health);
        _txtProfileHealthValue.text = $"{health.x}    /    {health.z}";
        _imgProfileHealthBar.fillAmount = result;
    }

    private void MapResourceToUI(Vector3 resourceDetails, PlayerEconomy economyType)
    {
        _txtProfileWorkerCount.text = $"{resourceDetails.x}    /    {resourceDetails.y}";
        _txtCurrentResourceAmount.text = $"{resourceDetails.z}";

        switch (economyType)
        {
            case PlayerEconomy.Money:
                _imgProfileEconomyIcon.sprite = _economyIcons[0];
                _imgProfileEconomyIcon.color = _economyIconColours[0];
                break;
            case PlayerEconomy.BuildMaterial:
                _imgProfileEconomyIcon.sprite = _economyIcons[1];
                _imgProfileEconomyIcon.color = _economyIconColours[1];
                break;
            case PlayerEconomy.Food:
                _imgProfileEconomyIcon.sprite = _economyIcons[2];
                _imgProfileEconomyIcon.color = _economyIconColours[2];
                break;
            default:
                break;
        }
        _txtProfileWorkerCount.gameObject.SetActive(true);
        _txtCurrentResourceAmount.gameObject.SetActive(true);
    }

    public SelectableObjectHandler SelectedObjectData
    {
        set
        {
            _selectedObjectData = value;
            OnSelectedBuildingCallback(_selectedObjectData);
        }
    }
}
