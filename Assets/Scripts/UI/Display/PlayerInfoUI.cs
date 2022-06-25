using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Displays the player's economy to the top right corner of the screen.
/// </summary>
public class PlayerInfoUI : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField]
    private Text _txtPopulation;
    [SerializeField]
    private Text _txtMoney;
    [SerializeField]
    private Text _txtBuildingMaterial;
    [SerializeField]
    private Text _txtFood;

    private PlayerData _playerData;

    private void Awake()
    {
        _playerData = FindObjectOfType<PlayerData>();
    }
    private void OnEnable()
    {
        _playerData.OnValueChanged += MapDataToUI;
    }
    private void OnDisable()
    {
        _playerData.OnValueChanged -= MapDataToUI;
    }

    private void MapDataToUI(PlayerData data)
    {
        _txtPopulation.text = $"{data.Population.Idle}/{data.Population.Current}/{data.Population.MaxLimit}";
        _txtMoney.text = $"{data.Money}";
        _txtBuildingMaterial.text = $"{data.BuildMaterials}";
        _txtFood.text = $"{data.Food}";
    }
}
