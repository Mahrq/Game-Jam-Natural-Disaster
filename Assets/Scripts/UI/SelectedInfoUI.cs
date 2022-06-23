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
    private Image _imgProfileHealthBar;
    private Sprite _defaultProfilePicture;
    private PlayerController _player;
    private BuildModeBlueprintBehaviour _selectedBuilding;
    private void Awake()
    {
        _defaultProfilePicture = _imgProfilePicture.sprite;
        _player = FindObjectOfType<PlayerController>();
    }
    private void OnEnable()
    {
        if (_profileDetailsHolder.activeInHierarchy)
        {
            _profileDetailsHolder.SetActive(false);
        }
        _player.OnValueChanged += OnSelectedBuildingCallback;
        _player.OnDeselectedBuilding += OnDeselectedBuildingCallback;
    }
    private void OnDisable()
    {
        _player.OnValueChanged -= OnSelectedBuildingCallback;
        _player.OnDeselectedBuilding -= OnDeselectedBuildingCallback;
    }
    private void OnSelectedBuildingCallback(BuildModeBlueprintBehaviour data)
    {
        _selectedBuilding = data;
        //To update UI while the building is still selected.
        _selectedBuilding.DamageHandler.OnValueChanged += MapHealthToUI;
        //Auto Deselect the buillding when it is destroyed if it's still selected.
        _selectedBuilding.DamageHandler.OnDeath += OnDeselectedBuildingCallback;
        Vector3 healthData = new Vector3(_selectedBuilding.DamageHandler.CurrentHealth, 0, _selectedBuilding.BuildProperties.MaxHealth);
        //Change the picture
        _imgProfilePicture.sprite = _selectedBuilding.BuildProperties.ProfilePicture;
        //Map the details to the panel
        _txtProfileName.text = _selectedBuilding.BuildProperties.Name;
        _txtProfileHealthValue.text = $"{healthData.x}    /    {healthData.z}";
        MapHealthToUI(healthData);

        if (!_profileDetailsHolder.activeInHierarchy)
        {
            _profileDetailsHolder.SetActive(true);
        }
    }

    private void OnDeselectedBuildingCallback()
    {
        ReturnToDefault();
    }

    private void ReturnToDefault()
    {
        _selectedBuilding.DamageHandler.OnValueChanged -= MapHealthToUI;
        _selectedBuilding.DamageHandler.OnDeath -= OnDeselectedBuildingCallback;
        _imgProfilePicture.sprite = _defaultProfilePicture;
        _selectedBuilding = null;
        if (_profileDetailsHolder.activeInHierarchy)
        {
            _profileDetailsHolder.SetActive(false);
        }
    }

    private void MapHealthToUI(Vector3 input)
    {
        float result = MathUtilities.GetPercentage(input);
        _txtProfileHealthValue.text = $"{input.x}    /    {input.z}";
        _imgProfileHealthBar.fillAmount = result;
    }
}
